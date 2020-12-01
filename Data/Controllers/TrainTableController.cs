using System;
using System.Collections.Generic;
using Parser.Models;

namespace Data.Controllers
{
    public class TrainTableController
    {
        //Занесение таблиц Грузы, Операции и Станции в общую таблицу вместе с Id
        static public void AddStationCargoOperationId(List<TrainTable> tt, List<Station> Stations, List<Cargo> Cargoes, List<Operation> Operations)
        {
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
        }

        // Добавление Id Вагонов в общую таблицу

        public static void AddCarrigeId(List<TrainTable> tt, List<Carrige> Carriges)
        {
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
        }
    }
}
