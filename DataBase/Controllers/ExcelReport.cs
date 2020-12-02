using Data;
using Data.Context;
using Data.ModelsComparers;
using OfficeOpenXml;
using Parser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Report.Controllers
{
    public static class ExcelReport
    {
        /// <summary>
        /// Метод, реализующий SQL-выборку и последующее заполнение Excel-документа
        /// </summary>
        /// <param name="TrainNumber"></param>
        /// <param name="LastStation"></param>
        /// <param name="WhenLastOperation"></param>
        static public void ToExcel(int TrainNumber, string LastStation, DateTime WhenLastOperation)
        {
            using (var db = new ContextTrain())
            {
                //List<TrainTable> Train_res = db.Database.SqlQuery<TrainTable>("SELECT TrainIndex,Carriges.CarNumber, " +
                //    "Carriges.InvoiceNum, WhenLastOperation, Operations.LastOperationName, StationName, PositionInTrain, " +
                //    "Cargoes.CargoId, FreightEtsngName, FreightTotalWeightKg  FROM Trains JOIN  Carriges ON Trains.CarrigeId = " +
                //    "Carriges.CarrigeId JOIN Operations ON Carriges.OperationId = Operations.OperationId JOIN Cargoes ON " +
                //    "Carriges.CargoId=Cargoes.CargoId JOIN Stations ON Operations.LastStationId=Stations.StationId WHERE " +
                //    "TrainNumber = " + TrainNumber + " AND StationName = '" + LastStation + "' AND WhenLastOperation = '" +
                //    WhenLastOperation + "' ORDER BY PositionInTrain ASC").ToList();

                List<TrainTable> Train_res = (from t in db.Trains
                              join c in db.Carriges on t.CarrigeId equals c.CarrigeId
                              join o in db.Operations on c.OperationId equals o.OperationId
                              join s in db.Stations on o.LastStationId equals s.StationId
                              join cg in db.Cargos on c.CargoId equals cg.CargoId
                              where s.StationName == LastStation && t.TrainNumber == TrainNumber && o.WhenLastOperation == WhenLastOperation
                              orderby c.PositionInTrain ascending
                              select new TrainTable
                              {
                                  TrainIndex = t.TrainIndex,
                                  CarNumber = c.CarNumber,
                                  InvoiceNum = c.InvoiceNum,
                                  WhenLastOperation = o.WhenLastOperation,
                                  LastOperationName = o.LastOperationName,
                                  LastStationName = s.StationName,
                                  PositionInTrain = c.PositionInTrain,
                                  CargoId = cg.CargoId,
                                  FreightEtsngName = cg.FreightEtsngName,
                                  FreightTotalWeightKg = c.FreightTotalWeightKg
                              }).ToList();

                FileInfo file = new FileInfo(@"C:\Users\Acer\Downloads\Тестовое задание для программиста систем отчетности\NL_Template.xlsx");

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    //Заполенение номера поезда, номера станции, станции дислокации и даты
                    worksheet.Cells[3, 3].Value = TrainNumber;
                    worksheet.Cells[4, 3].Value = Train_res[0].TrainIndex;
                    worksheet.Cells[3, 5].Value = LastStation;
                    worksheet.Cells[4, 5].Value = WhenLastOperation.ToString("dd.MM.yyyy");

                    //переменная для нумерации вагонов
                    int count = 0;
                    //переменная, равная началу вывода выборки 
                    int index = 7;

                    foreach (TrainTable t in Train_res)
                    {
                        worksheet.Cells[index, 1].Value = ++count;
                        worksheet.Cells[index, 2].Value = t.CarNumber;
                        worksheet.Cells[index, 3].Value = t.InvoiceNum;
                        worksheet.Cells[index, 4].Value = WhenLastOperation.ToString("dd.MM.yyyy");
                        worksheet.Cells[index, 5].Value = t.FreightEtsngName;
                        worksheet.Cells[index, 6].Value = Convert.ToDouble(t.FreightTotalWeightKg) / 1000;
                        worksheet.Cells[index, 7].Value = t.LastOperationName;
                        index++;
                    }

                    //Список всех типов груза в выборке
                    List<Cargo> CargoType = new List<Cargo>();

                    foreach (TrainTable t in Train_res)
                    {
                        CargoType.Add(new Cargo
                        {
                            CargoId = t.CargoId,
                            FreightEtsngName = t.FreightEtsngName
                        });
                    }

                    //Удаление дубликатов
                    CargoType = CargoType.Distinct(new CargoComparer()).ToList();

                    //переменная для подсчета вагонов по грузам
                    int carrigeCount = 0;

                    //Переменные для общего количества груза и по вагонам соответственно
                    double kgSum = 0;
                    double kg = 0;

                    //Вывод итоговых подсчетов
                    for (int i = 0; i < CargoType.Count(); i++)
                    {
                        worksheet.Cells[index + i, 5].Value = CargoType[i].FreightEtsngName;

                        foreach (TrainTable t in Train_res)
                        {
                            if (t.FreightEtsngName == CargoType[i].FreightEtsngName)
                            {
                                kg += Convert.ToDouble(t.FreightTotalWeightKg) / 1000;
                                worksheet.Cells[index + i, 2].Value = ++carrigeCount;
                            }
                        }
                        worksheet.Cells[index + i, 6].Value = kg;
                        kgSum += kg;
                        kg = 0;
                        carrigeCount = 0;
                    }

                    worksheet.Cells[index + CargoType.Count(), 6].Value = kgSum;
                    worksheet.Cells[index + CargoType.Count(), 1].Value = "Всего:";
                    worksheet.Cells[index + CargoType.Count(), 2].Value = Train_res.Count();
                    worksheet.Cells[index + CargoType.Count(), 5].Value = CargoType.Count();

                    worksheet.Cells["A" + index + ":G" + (index + CargoType.Count())].Style.Font.Bold = true;

                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    excelPackage.SaveAs(file);
                }
            }
        }
    }
}
