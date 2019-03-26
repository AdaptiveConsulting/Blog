using ExpressionParser.ReferentialData.Repository;

namespace ExpressionParser.ReferentialData
{
    public class UnitConverter : IUnitConverter
    {
        private readonly IUnitOfMeasureRepository _unitOfMeasureRepository = 
            new SimpleUnitOfMeasureRepository();
        
        public decimal GetConversionFactor(string fromCode, string toCode)
        {
            if (fromCode == toCode) return 1m;
            
            var fromUnitOfMeasure = _unitOfMeasureRepository.GetUnitOfMeasureByCode(fromCode);
            var toUnitOfMeasure = _unitOfMeasureRepository.GetUnitOfMeasureByCode(toCode);

            return 1m / fromUnitOfMeasure.UnitsPerMetricTon * toUnitOfMeasure.UnitsPerMetricTon;
        }
    }
}