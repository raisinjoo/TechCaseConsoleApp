using System;

namespace Parser.Models
{
    /// <summary>
    /// Класс, описывающий необходимые поля для парсинга и дальнейшей работы с таблицами
    /// </summary>
    public class TrainTable
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
}
