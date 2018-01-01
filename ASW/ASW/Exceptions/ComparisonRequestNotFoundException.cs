using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASW.Exceptions
{
    public class ComparisonRequestNotFoundException : Exception
    {
        public ComparisonRequestNotFoundException() : base("Comparison Request was not found.") { }
    }
}
