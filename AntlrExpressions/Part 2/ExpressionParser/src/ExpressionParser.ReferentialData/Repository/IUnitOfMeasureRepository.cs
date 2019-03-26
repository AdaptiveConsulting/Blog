using System.Collections.Generic;

namespace ExpressionParser.ReferentialData.Repository
{
    public interface IUnitOfMeasureRepository
    {
        UnitOfMeasure GetUnitOfMeasureByCode(string code);
        IReadOnlyDictionary<string, UnitOfMeasure> GetAllUnitsOfMeasure();
    }
}