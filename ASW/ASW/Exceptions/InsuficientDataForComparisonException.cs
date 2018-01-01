using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASW.Entities.Enums;

namespace ASW.Exceptions
{
    public class InsuficientDataForComparisonException : Exception
    {
        public InsuficientDataForComparisonException() : base("Insuficient Data for Comparison") { }
    }
}
