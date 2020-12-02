using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ModelsComparers
{
    /// <summary>
    /// Реализациz методf Equals интерфейса IEqualityComparer для класса Train, используется для сравнения значений в List<> между собой
    /// </summary>
    public class TrainComparer : IEqualityComparer<Train>
    {
        public bool Equals(Train x, Train y)
        {
            if (x.TrainNumber == y.TrainNumber &&
                x.TrainIndex == y.TrainIndex &&
                x.TrainIndexCombined == y.TrainIndexCombined &&
                x.FromStationId == y.FromStationId &&
                x.ToStationId == y.ToStationId &&
                x.CarrigeId == y.CarrigeId)
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
}
