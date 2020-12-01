using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp52
{
    /// <summary>
    /// Реализации методов Equals интерфейса IEqualityComparer для каждого класса, используется для сравнения значений в List<> между собой
    /// <example>
    /// Cargoes = Cargoes.Distinct(new CargoComparer()).ToList();
    /// </example>
    /// </summary>
    class CargoComparer :IEqualityComparer<Cargo>
    {
        public bool Equals(Cargo x, Cargo y)
        {
            return x.FreightEtsngName == y.FreightEtsngName;
        }
        public int GetHashCode(Cargo obj)
        {
            return obj.FreightEtsngName.GetHashCode();
        }
    }
    class StationComparer : IEqualityComparer<Station>
    {
        public bool Equals(Station x, Station y)
        {
            return x.StationName == y.StationName;
        }
        public int GetHashCode(Station obj)
        {
            return obj.StationId.GetHashCode();
        }
    }
    class CarrigeComparer : IEqualityComparer<Carrige>
    {
        public bool Equals(Carrige x, Carrige y)
        {
            if(x.CarNumber == y.CarNumber&& 
                x.PositionInTrain==y.PositionInTrain&&
                x.InvoiceNum==y.InvoiceNum&&
                x.OperationId==y.OperationId&&
                x.CargoId==y.CargoId&&
                x.FreightTotalWeightKg==y.FreightTotalWeightKg)
            {
                return true;
            }
            return false;
        }
        public int GetHashCode(Carrige obj)
        {
            return obj.CarNumber.GetHashCode();
        }
    }
    class TrainComparer : IEqualityComparer<Train>
    {
        public bool Equals(Train x, Train y)
        {
            if (x.TrainNumber == y.TrainNumber && 
                x.TrainIndex == y.TrainIndex && 
                x.TrainIndexCombined == y.TrainIndexCombined && 
                x.FromStationId == y.FromStationId && 
                x.ToStationId == y.ToStationId && 
                x.CarrigeId==y.CarrigeId)
            {
                return true;
            }
            return false;
        }
        public int GetHashCode(Train obj)
        {
            return obj.TrainNumber.GetHashCode();
        }
    }
    class OperationComparer : IEqualityComparer<Operation>
    {
        public bool Equals(Operation x, Operation y)
        {
            if (x.WhenLastOperation == y.WhenLastOperation 
                && x.LastOperationName == y.LastOperationName 
                && x.LastStationId == y.LastStationId)
            {
                return true;
            }
            return false;
        }
        public int GetHashCode(Operation obj)
        {
            return obj.OperationId.GetHashCode();
        }
    }

    /// <summary>
    /// Класс, описывающий необходимые поля для парсинга и дальнейшей работы с таблицами
    /// </summary>
    class TrainTable
    {
        public int TrainNumber { get; set; }
        public int TrainIndex { get; set; }
        public string TrainIndexCombined { get; set; }
        public int FromStationNameId { get; set; }
        public string FromStationName { get; set; }
        public int ToStationNameId { get; set; }
        public string ToStationName { get; set; }
        public int CarrigeId { get; set; }
        public int LastStationNameId { get; set; }
        public string LastStationName { get; set; }
        public DateTime WhenLastOperation { get; set; }
        public string LastOperationName { get; set; }
        public string InvoiceNum { get; set; }
        public int PositionInTrain { get; set; }
        public int OperationId { get; set; }
        public int CarNumber { get; set; }
        public int CargoId { get; set; }
        public string FreightEtsngName { get; set; }
        public int FreightTotalWeightKg { get; set; }

    }
    class Program
    {
        /// <summary>
        /// Метод, реализующий SQL-выборку и последующее заполнение Excel-документа
        /// </summary>
        /// <param name="TrainNumber"></param>
        /// <param name="LastStation"></param>
        /// <param name="WhenLastOperation"></param>
        static private void ToExcel(int TrainNumber, string LastStation, DateTime WhenLastOperation)
        {
            using (var db = new ContextTrain())
            {
                List<TrainTable> Train_res = db.Database.SqlQuery<TrainTable>("SELECT TrainIndex,Carriges.CarNumber, " +
                    "Carriges.InvoiceNum, WhenLastOperation, Operations.LastOperationName, StationName, PositionInTrain, " +
                    "Cargoes.CargoId, FreightEtsngName, FreightTotalWeightKg  FROM Trains JOIN  Carriges ON Trains.CarrigeId = " +
                    "Carriges.CarrigeId JOIN Operations ON Carriges.OperationId = Operations.OperationId JOIN Cargoes ON " +
                    "Carriges.CargoId=Cargoes.CargoId JOIN Stations ON Operations.LastStationId=Stations.StationId WHERE " +
                    "TrainNumber = " + TrainNumber + " AND StationName = '" + LastStation + "' AND WhenLastOperation = '" +
                    WhenLastOperation + "' ORDER BY PositionInTrain ASC").ToList();

                string excelPath = @"C:\Users\Acer\Downloads\Тестовое задание для программиста систем отчетности\NL_Template.xlsx";

                FileInfo file = new FileInfo(excelPath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    excelPackage.Workbook.CalcMode = ExcelCalcMode.Manual;

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
                        worksheet.Cells[index, 6].Value = Convert.ToDouble(t.FreightTotalWeightKg)/1000;
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
                        worksheet.Cells[index+i, 5].Value = CargoType[i].FreightEtsngName;

                        foreach (TrainTable t in Train_res)
                        {
                            if (t.FreightEtsngName == CargoType[i].FreightEtsngName)
                            {
                                kg += Convert.ToDouble(t.FreightTotalWeightKg)/1000;
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

                    worksheet.Cells["A"+index+":G" + (index + CargoType.Count())].Style.Font.Bold = true;

                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells["A7:G" + (index + CargoType.Count())].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    excelPackage.SaveAs(file);
                }
            }
        }

        /// <summary>
        /// Метод, декомпозирущий общую спарсенную таблицу и загружающий их в базу данных
        /// </summary>
        /// <param name="tt"></param>
        static private void AddToDB(List<TrainTable> tt)
        {
            int IdCount = 0;

            //Создание таблицы Грузы

            var Cargoes = new List<Cargo>();
            foreach (TrainTable t in tt)
            {
                Cargoes.Add(new Cargo
                {
                    FreightEtsngName = t.FreightEtsngName,
                });
            }
            Cargoes = Cargoes.Distinct(new CargoComparer()).ToList();

            foreach (Cargo c in Cargoes)
            {
                c.CargoId = ++IdCount;
            }

            IdCount = 0;
            //Создание таблицы Станции

            var Stations = new List<Station>();
            foreach (TrainTable t in tt)
            {
                Stations.Add(new Station
                {
                    StationName = t.FromStationName
                });
                Stations.Add(new Station
                {
                    StationName = t.ToStationName
                });
                Stations.Add(new Station
                {
                    StationName = t.LastStationName
                });
            }
            Stations = Stations.Distinct(new StationComparer()).ToList();

            foreach (Station s in Stations)
            {
                s.StationId = ++IdCount;
            }

            IdCount = 0;

            //Создание таблицы Операции

            var Operations = new List<Operation>();
            foreach (TrainTable t in tt)
            {
                foreach (Station s in Stations)
                {
                    if (t.LastStationName == s.StationName)
                    {
                        Operations.Add(new Operation
                        {
                            WhenLastOperation = t.WhenLastOperation,
                            LastOperationName = t.LastOperationName,
                            LastStationId = s.StationId
                        });
                    }
                }
            }
            Operations = Operations.Distinct(new OperationComparer()).ToList();

            foreach (var o in Operations)
            {
                o.OperationId = ++IdCount;
            }

            IdCount = 0;

            //Занесение таблиц Грузы, Операции и Станции в общую таблицу вместе с Id

            foreach (TrainTable t in tt)
            {
                foreach (Station s in Stations)
                {
                    if (t.FromStationName == s.StationName)
                    {
                        t.FromStationNameId = s.StationId;
                    }
                    if (t.ToStationName == s.StationName)
                    {
                        t.ToStationNameId = s.StationId;
                    }
                    if (t.LastStationName == s.StationName)
                    {
                        t.LastStationNameId = s.StationId;
                    }
                }
                foreach (Cargo c in Cargoes)
                {
                    if (t.FreightEtsngName == c.FreightEtsngName)
                    {
                        t.CargoId = c.CargoId;
                    }
                }
                foreach (Operation o in Operations)
                {
                    if (t.WhenLastOperation == o.WhenLastOperation
                        && t.LastOperationName == o.LastOperationName
                        && t.LastStationNameId == o.LastStationId)
                    {
                        t.OperationId = o.OperationId;
                    }
                }
            }
            //Создание таблицы Вагоны

            var Carriges = new List<Carrige>();

            foreach (TrainTable t in tt)
            {
                Carriges.Add(new Carrige
                {
                    OperationId = t.OperationId,
                    PositionInTrain = t.PositionInTrain,
                    InvoiceNum = t.InvoiceNum,
                    CarNumber = t.CarNumber,
                    CargoId = t.CargoId,
                    FreightTotalWeightKg = t.FreightTotalWeightKg
                });
            }

            Carriges = Carriges.Distinct(new CarrigeComparer()).ToList();

            foreach (var cr in Carriges)
            {
                cr.CarrigeId = ++IdCount;
            }

            IdCount = 0;

            // Добавление Id Вагонов в общую таблицу

            foreach (TrainTable t in tt)
            {
                foreach (Carrige cr in Carriges)
                {
                    if (t.CarNumber == cr.CarNumber &&
                        t.PositionInTrain == cr.PositionInTrain &&
                        t.InvoiceNum == cr.InvoiceNum &&
                        t.OperationId == cr.OperationId &&
                        t.CargoId == cr.CargoId &&
                        t.FreightTotalWeightKg == cr.FreightTotalWeightKg)
                    {
                        t.CarrigeId = cr.CarrigeId;
                    }
                }
            }

            //Создание таблицы Поезда

            var Trains = new List<Train>();

            foreach (TrainTable t in tt)
            {
                Trains.Add(new Train
                {
                    TrainNumber = t.TrainNumber,
                    TrainIndex = t.TrainIndex,
                    TrainIndexCombined = t.TrainIndexCombined,
                    FromStationId = t.FromStationNameId,
                    ToStationId = t.ToStationNameId,
                    CarrigeId = t.CarrigeId
                });
            }

            Trains = Trains.Distinct(new TrainComparer()).ToList();

            foreach (var t in Trains)
            {
                t.TrainId = ++IdCount;
            }

            //Заполнение таблиц в базе данных

            using (ContextTrain db = new ContextTrain())
            {
                foreach (var c in Cargoes)
                {
                    db.Cargos.Add(new Cargo
                    {
                        CargoId = c.CargoId,
                        FreightEtsngName = c.FreightEtsngName
                    });
                }
                foreach (var s in Stations)
                {
                    db.Stations.Add(new Station
                    {
                        StationId = s.StationId,
                        StationName = s.StationName
                    });
                }
                foreach (var c in Carriges)
                {
                    db.Carriges.Add(new Carrige
                    {
                        CarrigeId = c.CarrigeId,
                        CarNumber = c.CarNumber,
                        PositionInTrain = c.PositionInTrain,
                        InvoiceNum = c.InvoiceNum,
                        OperationId=c.OperationId,
                        CargoId = c.CargoId,
                        FreightTotalWeightKg = c.FreightTotalWeightKg
                    });
                }
                foreach (var o in Operations)
                {
                    db.Operations.Add(new Operation
                    {
                        OperationId = o.OperationId,
                        LastOperationName = o.LastOperationName,
                        LastStationId = o.LastStationId,
                        WhenLastOperation = o.WhenLastOperation
                    });
                }
                foreach (var t in Trains)
                {
                    db.Trains.Add(new Train
                    {
                        TrainId = t.TrainId,
                        TrainIndex = t.TrainIndex,
                        TrainIndexCombined = t.TrainIndexCombined,
                        TrainNumber = t.TrainNumber,
                        FromStationId = t.FromStationId,
                        ToStationId = t.ToStationId,
                        CarrigeId = t.CarrigeId,
                    });
                }
                db.SaveChanges();
            }
        }
        
        /// <summary>
        /// Метод для парсинга xml-документа
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        static private List<TrainTable> ParseToList(XmlDocument doc)
        {
            int TrainNumber_temp = 0;
            int TrainIndex_temp = 0;
            string TrainIndexCombined_temp = "";
            string FromStationName_temp = "";
            string ToStationName_temp = "";
            string LastStationName_temp = "";
            DateTime WhenLastOperation_temp = DateTime.Today;
            string LastOperationName_temp = "";
            string InvoiceNum_temp = "";
            int PositionInTrain_temp = 0;
            int CarNumber_temp = 0;
            string FreightEtsngName_temp = "";
            int FreightTotalWeightKg_temp = 0;

            List<TrainTable> Trains = new List<TrainTable>();

            //Счетчик столбцов (тэгов в xml-документе). Всего обходится 12 тэгов.
            int CountRow = 0;

            foreach (XmlNode root in doc.DocumentElement)
            {
                foreach (XmlNode row in root.ChildNodes)
                {
                    if (CountRow < 12)
                    {
                        if (row.Name == "TrainNumber")
                        {
                            TrainNumber_temp = Convert.ToInt32(row.InnerText);
                            CountRow++;
                        }
                        if (row.Name == "TrainIndexCombined")
                        {
                            TrainIndexCombined_temp = row.InnerText;
                            TrainIndex_temp = Convert.ToInt32(row.InnerText.Split('-')[1]);
                            CountRow++;
                        }
                        if (row.Name == "FromStationName")
                        {
                            FromStationName_temp = row.InnerText;
                            CountRow++;
                        }
                        if (row.Name == "ToStationName")
                        {
                            ToStationName_temp = row.InnerText;
                            CountRow++;
                        }
                        if (row.Name == "LastStationName")
                        {
                            LastStationName_temp = (row.InnerText);
                            CountRow++;
                        }
                        if (row.Name == "WhenLastOperation")
                        {
                            WhenLastOperation_temp = Convert.ToDateTime(row.InnerText);
                            CountRow++;
                        }
                        if (row.Name == "LastOperationName")
                        {
                            LastOperationName_temp = row.InnerText;
                            CountRow++;
                        }
                        if (row.Name == "InvoiceNum")
                        {
                            InvoiceNum_temp = row.InnerText;
                            CountRow++;
                        }
                        if (row.Name == "PositionInTrain")
                        {
                            PositionInTrain_temp = Convert.ToInt32(row.InnerText);
                            CountRow++;
                        }
                        if (row.Name == "CarNumber")
                        {
                            CarNumber_temp = Convert.ToInt32(row.InnerText);
                            CountRow++;
                        }
                        if (row.Name == "FreightEtsngName")
                        {
                            FreightEtsngName_temp = row.InnerText;
                            CountRow++;
                        }
                        if (row.Name == "FreightTotalWeightKg")
                        {
                            FreightTotalWeightKg_temp = Convert.ToInt32(row.InnerText);
                            CountRow++;
                        }
                    }

                    if (CountRow >= 12)
                    {
                        Trains.Add(new TrainTable
                        {
                            TrainNumber = TrainNumber_temp,
                            TrainIndex= TrainIndex_temp,
                            TrainIndexCombined = TrainIndexCombined_temp,
                            FromStationName = FromStationName_temp,
                            ToStationName = ToStationName_temp,
                            LastStationName = LastStationName_temp,
                            WhenLastOperation = WhenLastOperation_temp,
                            LastOperationName = LastOperationName_temp,
                            InvoiceNum = InvoiceNum_temp,
                            PositionInTrain = PositionInTrain_temp,
                            CarNumber = CarNumber_temp,
                            FreightEtsngName = FreightEtsngName_temp,
                            FreightTotalWeightKg = FreightTotalWeightKg_temp
                        });
                        CountRow = 0;
                    }
                }
            }
            return Trains;
        }

        static void Main(string[] args)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(@"C:\Users\Acer\Downloads\Тестовое задание для программиста систем отчетности\Data.xml");

            List<TrainTable> tt = ParseToList(xd);

            ///<example>
            ///Для создание таблиц в бд использовать AddToDB(tt);
            ///</example>

            Console.WriteLine("Номер поезда:");
            int TrainNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Станция дислокации:");
            string LastStation = Console.ReadLine().ToUpper();

            Console.WriteLine("Дата и время:");
            DateTime WhenLastOperation = Convert.ToDateTime(Console.ReadLine());

            ToExcel(TrainNumber, LastStation, WhenLastOperation);

            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
   
