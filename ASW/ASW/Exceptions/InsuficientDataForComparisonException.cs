using System;

namespace ASW.Exceptions
{
    public class InsuficientDataForComparisonException : Exception
    {
        public InsuficientDataForComparisonException() : base("Insuficient Data for Comparison")
        {
        }
    }
}