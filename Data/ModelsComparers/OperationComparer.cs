using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ModelsComparers
{
    public class OperationComparer : IEqualityComparer<Operation>
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
}
