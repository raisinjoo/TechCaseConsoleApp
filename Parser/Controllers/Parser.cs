using System;
using System.Collections.Generic;
using System.Xml;
using Parser.Models;

namespace Parser.Controllers
{
    public static class Parser
    {
        /// <summary>
        /// Метод для парсинга xml-документа
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<TrainTable> ParseToList(XmlDocument doc)
        {
            List<TrainTable> Trains = new List<TrainTable>();
            var tt = new TrainTable();
            
                foreach (XmlNode root in doc.DocumentElement)
                {
                foreach (XmlNode row in root.ChildNodes)
                {
                    if (row.Name == "TrainNumber")
                    {
                        tt.TrainNumber = Convert.ToInt32(row.InnerText);
                    }
                    if (row.Name == "TrainIndexCombined")
                    {
                        tt.TrainIndex = Convert.ToInt32(row.InnerText.Split('-')[1]);
                        tt.TrainIndexCombined = row.InnerText;
                    }
                    if (row.Name == "FromStationName")
                    {
                        tt.FromStationName = row.InnerText;
                    }
                    if (row.Name == "ToStationName")
                    {
                        tt.ToStationName = row.InnerText;
                    }
                    if (row.Name == "LastStationName")
                    {
                        tt.LastStationName = (row.InnerText);
                    }
                    if (row.Name == "WhenLastOperation")
                    {
                        tt.WhenLastOperation = Convert.ToDateTime(row.InnerText);
                    }
                    if (row.Name == "LastOperationName")
                    {
                        tt.LastOperationName = row.InnerText;
                    }
                    if (row.Name == "InvoiceNum")
                    {
                        tt.InvoiceNum = row.InnerText;
                    }
                    if (row.Name == "PositionInTrain")
                    {
                        tt.PositionInTrain = Convert.ToInt32(row.InnerText);
                    }
                    if (row.Name == "CarNumber")
                    {
                        tt.CarNumber = Convert.ToInt32(row.InnerText);
                    }
                    if (row.Name == "FreightEtsngName")
                    {
                        tt.FreightEtsngName = row.InnerText;
                    }
                    if (row.Name == "FreightTotalWeightKg")
                    {
                        tt.FreightTotalWeightKg = Convert.ToInt32(row.InnerText);

                        Trains.Add(new TrainTable
                        {
                            TrainNumber = tt.TrainNumber,
                            TrainIndex = tt.TrainIndex,
                            TrainIndexCombined = tt.TrainIndexCombined,
                            FromStationName = tt.FromStationName,
                            ToStationName = tt.ToStationName,
                            LastStationName = tt.LastStationName,
                            LastOperationName = tt.LastOperationName,
                            WhenLastOperation = tt.WhenLastOperation,
                            CarNumber = tt.CarNumber,
                            PositionInTrain = tt.PositionInTrain,
                            InvoiceNum = tt.InvoiceNum,
                            FreightEtsngName=tt.FreightEtsngName,
                            FreightTotalWeightKg=tt.FreightTotalWeightKg
                        }) ;
                    }
                }
            }
            return Trains;
        }
    }
}
