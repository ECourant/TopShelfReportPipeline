using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Xml;
namespace TopShelfReportPipeline.Network
{
    internal class RequestHandler
    {
        internal RequestHandler(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
            this.Init();
        }

        internal ConcurrentQueue<ReportRequestWrapper<IReportRequest>> Requests { get; set; }

        internal ConcurrentDictionary<Guid, Newtonsoft.Json.Linq.JToken> Results { get; set; }

        internal Thread ProcessThread { get; set; }

        private HttpClient Client { get; set; }

        private HttpResponseMessage ResponseMessage { get; set; }

        private string Output = "XML";

        private string Username { get; set; }

        private string Password { get; set; }

        private string ViewState { get; set; }

        private string ViewStateGenerator { get; set; }

        private string EventValidation { get; set; }

        internal void Init()
        {
            Requests = new ConcurrentQueue<ReportRequestWrapper<IReportRequest>>();
            Results = new ConcurrentDictionary<Guid, Newtonsoft.Json.Linq.JToken>();
            ProcessThread = new Thread(() => Process());
            ProcessThread.IsBackground = true;
            ProcessThread.Priority = ThreadPriority.Normal;
            ProcessThread.Start();
        }

        internal Newtonsoft.Json.Linq.JToken[] RetrieveReport<T>(T ReportRequest) where T : IReportRequest
        {
            ReportRequestWrapper<IReportRequest> Request = new ReportRequestWrapper<IReportRequest>(ReportRequest);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"TopShelf Report: Queueing Report Request [{Request.RequestID}] For Report [{Request.ReportRequest.ReportPath}]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Requests.Enqueue(Request);
            Task WaitForResponse = new Task(() =>
            {
                while (!Results.ContainsKey(Request.RequestID))
                    Thread.Sleep(10);
            });
            WaitForResponse.Start();
            if (WaitForResponse.Wait(TimeSpan.FromMinutes(10)))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"TopShelf Report: Returning Report Results [{Request.RequestID}] For Report [{Request.ReportRequest.ReportPath}]");
                Console.ForegroundColor = ConsoleColor.Gray;
                if (Results.ContainsKey(Request.RequestID))
                {
                    Newtonsoft.Json.Linq.JToken Token;
                    Results.TryRemove(Request.RequestID, out Token);
                    Console.WriteLine(Token.ToString());
                    if (Token == null || Token.Count() == 0)
                        return null;
                    else if (Token["Table1"].Type != Newtonsoft.Json.Linq.JTokenType.Array)
                        return new Newtonsoft.Json.Linq.JToken[] { Token["Table1"] };
                    else if (Token["Table1"].Type == Newtonsoft.Json.Linq.JTokenType.Array)
                        return Token["Table1"].ToArray();
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                throw new TimeoutException($"The request for report [{ReportRequest.ReportPath}] has timed out.");
        }

        private void Process()
        {
            try
            {
                while (!string.IsNullOrWhiteSpace(this.Username) && !string.IsNullOrWhiteSpace(this.Password))
                {
                    try
                    {
                        ReportRequestWrapper<IReportRequest> Request = null;
                        Requests.TryPeek(out Request);
                        if (Request != null)
                        {
                            Requests.TryDequeue(out Request);
                            Console.WriteLine($"Processing Report Request [{Request.RequestID}] For Report [{Request.ReportRequest.ReportPath}]");
                            string ReportPathURL = System.Web.HttpUtility.UrlEncode(Request.ReportRequest.ReportPath);
                            this.InitSession();
                            this.ResponseMessage = this.Client.GetAsync($"https://{TDefaults.Site}{TDefaults.ReportTarget}?rn={ReportPathURL}").Result;
                            this.Client.DefaultRequestHeaders.Add("Referer", $"https://{TDefaults.Site}{TDefaults.ReportTarget}?rn={ReportPathURL}");
                            #region Filter Code
                            int ReFilterCount = 0;
                            ReFilter:
                            if (ReFilterCount >= 3)
                                throw new Exception();
                            if (Request.ReportRequest.Filters?.Count() > 0)
                            {
                                bool NeedsFilterIdentifiers = false;
                                foreach (var Filter in Request.ReportRequest.Filters)
                                {
                                    if (string.IsNullOrWhiteSpace(Filter.Uid))
                                        NeedsFilterIdentifiers = true;
                                    if (string.IsNullOrWhiteSpace(Filter.GUID))
                                        NeedsFilterIdentifiers = true;
                                    if (string.IsNullOrWhiteSpace(Filter.Column))
                                        NeedsFilterIdentifiers = true;
                                    if (NeedsFilterIdentifiers)
                                        break;
                                }
                                if (NeedsFilterIdentifiers)
                                {
                                    Console.WriteLine("Filters need to be updated!");
                                    #region Get Filter Data
                                    var GetFilterData = new FormUrlEncodedContent(new[]
                                    {
                                        new KeyValuePair<string, string>("wscmd", "getfiltersdata"),
                                    });
                                    this.ResponseMessage = this.Client.PostAsync($"https://{TDefaults.Site}/AssetTracking/Reports/Custom/rs.aspx", GetFilterData).Result;
                                    RequestFilterHandler filterHandler = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestFilterHandler>(this.ResponseMessage.Content.ReadAsStringAsync().Result);
                                    foreach (var Filter in filterHandler.Filters)
                                    {
                                        if (Request.ReportRequest.Filters.Select(RequestFilter => RequestFilter.Alias).Contains(Filter.Alias))
                                        {
                                            Console.WriteLine($"Updating filter [{Filter.Alias}] with identity!");
                                            var RequestFilter = Request.ReportRequest.Filters.Where(RFilter => RFilter.Alias == Filter.Alias).FirstOrDefault();
                                            int Index = Request.ReportRequest.Filters.IndexOf(Request.ReportRequest.Filters.Where(RFilter => RFilter.Alias == Filter.Alias).FirstOrDefault());
                                            RequestFilter.Uid = Filter.Uid;
                                            RequestFilter.GUID = Filter.GUID;
                                            RequestFilter.Column = Filter.Column;
                                            Request.ReportRequest.Filters[Index] = RequestFilter;
                                            Console.WriteLine($"Filter [{Filter.Alias}] has been updated!");
                                        }
                                    }
                                    ReFilterCount++;
                                    goto ReFilter;
                                    #endregion
                                }
                                #region Update Filter
                                string UpdateFilterJSON = Newtonsoft.Json.JsonConvert.SerializeObject(Request.ReportRequest.Filters);
                                var updateFilter = new FormUrlEncodedContent(new[]
                                {
                                    new KeyValuePair<string, string>("wscmd", "setfiltersdata"),
                                    new KeyValuePair<string, string>("wsarg0", UpdateFilterJSON),
                                });
                                this.ResponseMessage = this.Client.PostAsync($"https://{TDefaults.Site}/AssetTracking/Reports/Custom/rs.aspx", updateFilter).Result;
                                #endregion
                            }
                            #endregion
                            #region Queue XML Report
                            this.ResponseMessage = this.Client.GetAsync($"https://{TDefaults.Site}/AssetTracking/Reports/Custom/rs.aspx?output={Output}&refrn={Request.ReportRequest.ReportPath}&taskId={Request.RequestID}").Result;
                            string XML = this.ResponseMessage.Content.ReadAsStringAsync().Result;
                            this.Logout();
                            XmlDocument Document = new XmlDocument();
                            Document.LoadXml(XML.Replace("xmlns=\"numeric\"", "").Replace("GenLongFormat:", "").Replace("xmlns:GenLongFormat=\"datetime\"", ""));
                            string JSON = Newtonsoft.Json.JsonConvert.SerializeXmlNode(Document["Reports"]["Detail"], Newtonsoft.Json.Formatting.Indented);
                            Newtonsoft.Json.Linq.JObject Obj = Newtonsoft.Json.Linq.JObject.Parse(JSON);
                            #endregion
                            if (Results.ContainsKey(Request.RequestID))
                                Results[Request.RequestID] = Obj["Detail"];
                            else
                                Results.TryAdd(Request.RequestID, Obj["Detail"]);
                        }
                        Thread.Sleep(1);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("ERROR");
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void InitSession()
        {
            if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password))
                throw new UnauthorizedAccessException("The credentials provided are not valid!");
            else
            {
                Client = new HttpClient();
                Client.Timeout = TimeSpan.FromMinutes(10);
                ResponseMessage = Client.GetAsync($"https://{TDefaults.Site}{TDefaults.LoginTarget}").Result;
                ViewState = ResponseMessage.Content.ReadAsStringAsync().Result.Split(new string[] { "id=\"__VIEWSTATE\" value=\"" }, StringSplitOptions.None).Last().Split(new string[] { "\" />" }, StringSplitOptions.None).First();
                ViewStateGenerator = ResponseMessage.Content.ReadAsStringAsync().Result.Split(new string[] { "id=\"__VIEWSTATEGENERATOR\" value=\"" }, StringSplitOptions.None).Last().Split(new string[] { "\" />" }, StringSplitOptions.None).First();
                EventValidation = ResponseMessage.Content.ReadAsStringAsync().Result.Split(new string[] { "id=\"__EVENTVALIDATION\" value=\"" }, StringSplitOptions.None).Last().Split(new string[] { "\" />" }, StringSplitOptions.None).First();
                Console.WriteLine($"View State: {ViewState}");
                Console.WriteLine($"View State Generator: {ViewStateGenerator}");
                Console.WriteLine($"Event Validation: {EventValidation}");
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("__EVENTTARGET", ""),
                    new KeyValuePair<string, string>("__EVENTARGUMENT", ""),
                    new KeyValuePair<string, string>("__VIEWSTATE", ViewState),
                    new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", ViewStateGenerator),
                    new KeyValuePair<string, string>("__SCROLLPOSITIONX", "0"),
                    new KeyValuePair<string, string>("__SCROLLPOSITIONy", "0"),
                    new KeyValuePair<string, string>("__EVENTVALIDATION", EventValidation),
                    new KeyValuePair<string, string>("Login1$UserName", Username),
                    new KeyValuePair<string, string>("Login1$Password", Password),
                    new KeyValuePair<string, string>("Login1$LoginButton", "Login")
                });
                ResponseMessage = Client.PostAsync($"https://{TDefaults.Site}{TDefaults.LoginTarget}?ReturnUrl=%2fAssetTracking%2fNav%2fMain.aspx", formContent).Result;
            }
        }

        private void Logout()
        {
            ResponseMessage = Client.GetAsync($"https://{TDefaults.Site}/AssetTracking/Logout.aspx").Result;
        }
    }
}
