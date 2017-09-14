using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline_Unit_Tests
{
    public class DocumentConstructor : TopShelfReportPipeline.IReportConstructor<DocumentsArray, Document>
    {
        public DocumentsArray Construct(Newtonsoft.Json.Linq.JToken[] Tokens)
        {
            DocumentArrayRow[] Rows = Tokens.Select(Row => Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentArrayRow>(Row.ToString())).ToArray();
            DocumentsArray Documents = new DocumentsArray();
            Dictionary<string, Document> Docs = new Dictionary<string, Document>();
            foreach (var Row in Rows)
            {
                if (Docs.ContainsKey(Row.DocumentNumber))
                {
                    if (Docs[Row.DocumentNumber].Items.ContainsKey(Row.PartName))
                    {
                        Docs[Row.DocumentNumber].Items[Row.PartName].DocumentLines = Docs[Row.DocumentNumber].Items[Row.PartName].DocumentLines.Concat(new DocumentLine[]
                        {
                            new DocumentLine()
                            {
                                DocumentLineID = Row.DocumentLineID,
                                QTY = Row.QTY,
                                QTYComplete = Row.QTYComplete
                            }
                        }).ToArray();
                    }
                    else
                        Docs[Row.DocumentNumber].Items.Add(Row.PartName, new DocumentItem()
                        {
                            DocumentLines = new DocumentLine[]
                            {
                                new DocumentLine()
                                {
                                    DocumentLineID = Row.DocumentLineID,
                                    QTY = Row.QTY,
                                    QTYComplete = Row.QTYComplete
                                }
                            },
                            PartID = Row.PartID,
                            PartName = Row.PartName,
                            RequiresSerialNumber = Row.RequiresSerialNumber
                        });
                }
                else
                    Docs.Add(Row.DocumentNumber, new Document()
                    {
                        DocumentID = Row.DocumentID,
                        DocumentNumber = Row.DocumentNumber,
                        Items = new Dictionary<string, DocumentItem>()
                        {
                            [Row.PartName] = new DocumentItem()
                            {
                                DocumentLines = new DocumentLine[]
                                {
                                    new DocumentLine()
                                    {
                                        DocumentLineID = Row.DocumentLineID,
                                        QTY = Row.QTY,
                                        QTYComplete = Row.QTYComplete
                                    }
                                },
                                PartID = Row.PartID,
                                PartName = Row.PartName,
                                RequiresSerialNumber = Row.RequiresSerialNumber
                            }
                        }
                    });
            }
            Documents.Items = Docs.Values.ToArray();
            return Documents;
        }
    }
}
