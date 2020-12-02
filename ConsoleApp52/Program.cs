using Data.Controllers;
using Parser.Models;
using Report.Controllers;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Resources;
using System.Reflection;
using System.IO;

namespace ConsoleApp52
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(Properties.Resources.Data);
            
            List<TrainTable> tt = Parser.Controllers.Parser.ParseToList(xd);

            ///<example>
            ///Для создание таблиц в бд использовать AddingToDatabase.AddToDB(tt);
            ///</example>

            Console.WriteLine("Номер поезда:");
            int TrainNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Станция дислокации:");
            string LastStation = Console.ReadLine().ToUpper();

            Console.WriteLine("Дата и время:");
            DateTime WhenLastOperation = Convert.ToDateTime(Console.ReadLine());

            ExcelReport.ToExcel(TrainNumber, LastStation, WhenLastOperation);

            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
   
