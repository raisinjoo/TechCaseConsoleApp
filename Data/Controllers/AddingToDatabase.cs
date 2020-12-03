using Data;
using Data.Context;
using Data.ModelsComparers;
using Parser.Models;
using System.Collections.Generic;
using System.Linq;

namespace Data.Controllers
{
    public static class AddingToDatabase
    {
        /// <summary>
        /// Метод, декомпозирущий общую спарсенную таблицу и загружающий их в базу данных
        /// </summary>
        /// <param name="tt"></param>
        public static void AddToDB(List<TrainTable> tt)
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

            TrainTableController.AddStationCargoOperationId(tt, Stations, Cargoes, Operations);

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

            TrainTableController.AddCarrigeId(tt, Carriges);

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
                        OperationId = c.OperationId,
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
    }
}
