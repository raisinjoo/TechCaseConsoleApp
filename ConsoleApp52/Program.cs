using Data.Context;
using Data.Controllers;
using Parser.Models;
using Report.Controllers;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Properties.Resources.Data);
            
            List<TrainTable> tt = Parser.Controllers.Parser.ParseToList(xd);

            using (ContextTrain db = new ContextTrain())
            {
                if (db.Database.Exists() == false)
                {
                    AddingToDatabase.AddToDB(tt);
                }
            }

            int TrainNumber = 2236;
            string LastStation = "ЧЕРНОРЕЧЕНСКАЯ";
            DateTime WhenLastOperation = Convert.ToDateTime("30.06.2019 14:07:00");

            Console.WriteLine("Задать значения для отчета? Y/N");
            string answer = Console.ReadLine();

            if (answer == "Y")
            {
                Console.WriteLine("Номер поезда:");
                TrainNumber = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Станция дислокации:");
                LastStation = Console.ReadLine().ToUpper();

                Console.WriteLine("Дата и время:");
                WhenLastOperation = Convert.ToDateTime(Console.ReadLine());
            }

            ExcelReport.ToExcel(TrainNumber, LastStation, WhenLastOperation);

            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
   
