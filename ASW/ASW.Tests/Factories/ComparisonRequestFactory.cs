using ASW.Entities;
using ASW.Entities.Enums;

namespace ASW.Tests.Factories
{
    public static class ComparisonRequestFactory
    {
        public static ComparisonRequestEntity GetSidesNull(long id)
        {
            return new ComparisonRequestEntity
            {
                Id = id
            };
        }

        public static ComparisonRequestEntity GetSingleSideNull(long id, Side sideNullValue, string value)
        {
            return new ComparisonRequestEntity
            {
                Id = id,
                Left = sideNullValue == Side.Left ? null : value,
                Right = sideNullValue == Side.Right ? null : value
            };
        }

        public static ComparisonRequestEntity GetValid(long id, string rightValue, string leftValue)
        {
            return new ComparisonRequestEntity
            {
                Id = id,
                Left = leftValue,
                Right = rightValue
            };
        }
    }
}