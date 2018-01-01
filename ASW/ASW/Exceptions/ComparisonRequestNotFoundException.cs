using System;

namespace ASW.Exceptions
{
    public class ComparisonRequestNotFoundException : Exception
    {
        public ComparisonRequestNotFoundException() : base("Comparison Request was not found.")
        {
        }
    }
}