using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ModelsComparers
{
    /// <summary>
    /// Реализациz методf Equals интерфейса IEqualityComparer для класса Station, используется для сравнения значений в List<> между собой
    /// </summary>
    public class StationComparer : IEqualityComparer<Station>
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
}
