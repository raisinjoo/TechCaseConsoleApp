using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ModelsComparers
{
    /// <summary>
    /// Реализациz методf Equals интерфейса IEqualityComparer для класса Cargo, используется для сравнения значений в List<> между собой
    /// <example>
    /// Cargoes = Cargoes.Distinct(new CargoComparer()).ToList();
    /// </example>
    /// </summary>
    public class CargoComparer : IEqualityComparer<Cargo>
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
}
