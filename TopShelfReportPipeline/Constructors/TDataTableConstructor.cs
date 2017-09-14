using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TopShelfReportPipeline.Constructors
{
    public class TDataTableConstructor : IConstructor<DataTable>
    {
        public DataTable Construct(JToken[] Tokens)
        {
            DataTable Table = new DataTable();
            List<string> Columns = new List<string>();
            foreach (var Token in Tokens)
            {
                JObject jObject = JObject.Parse(Token.ToString());
                foreach (var Field in jObject.Properties())
                {
                    if (!Columns.Contains(Field.Name))
                        Columns.Add(Field.Name);
                }
            }
            Columns.ForEach(Column =>
            {
                Table.Columns.Add(Column);
            });
            foreach (var Token in Tokens)
            {
                JObject jObject = JObject.Parse(Token.ToString());
                List<object> Values = new List<object>();
                foreach (var Column in Columns)
                {
                    try
                    {
                        Values.Add(jObject[Column]);
                    }
                    catch
                    {
                        Values.Add(null);
                    }
                }
                Table.Rows.Add(Values.ToArray());
            }
            return Table;
        }
    }
}
