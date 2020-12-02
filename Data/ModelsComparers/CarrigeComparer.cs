using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ModelsComparers
{
    /// <summary>
    /// Реализациz методf Equals интерфейса IEqualityComparer для класса Carrige, используется для сравнения значений в List<> между собой
    /// </summary>
    public class CarrigeComparer : IEqualityComparer<Carrige>
    {
        public bool Equals(Carrige x, Carrige y)
        {
            if (x.CarNumber == y.CarNumber &&
                x.PositionInTrain == y.PositionInTrain &&
                x.InvoiceNum == y.InvoiceNum &&
                x.OperationId == y.OperationId &&
                x.CargoId == y.CargoId &&
                x.FreightTotalWeightKg == y.FreightTotalWeightKg)
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
}
