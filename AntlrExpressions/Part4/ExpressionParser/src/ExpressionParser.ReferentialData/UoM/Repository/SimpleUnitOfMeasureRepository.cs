using System.Collections.Generic;

namespace ExpressionParser.ReferentialData.UoM.Repository
{
    // This simple UOM repo, knows about a *few* units of measure. There are definitely a few more :)
    public class SimpleUnitOfMeasureRepository : IUnitOfMeasureRepository
    {
        private readonly Dictionary<string, UnitOfMeasure> _knownUnitsOfMeasure =
            new Dictionary<string, UnitOfMeasure>
            {
                {"gram", new UnitOfMeasure("gram", 1000000m)},
                {"Kg", new UnitOfMeasure("Kg", 1000m)},
                {"lbs", new UnitOfMeasure("lbs", 2204.621m)},
                {"LT", new UnitOfMeasure("LT", 0.984206m)},
                {"MT", new UnitOfMeasure("MT", 1m)},
                {"ST", new UnitOfMeasure("ST", 1.1023m)},
            };
        
        public UnitOfMeasure GetUnitOfMeasureByCode(string code)
        {
            return _knownUnitsOfMeasure[code]; //throw if not found
        }

        public IReadOnlyDictionary<string, UnitOfMeasure> GetAllUnitsOfMeasure()
        {
            return _knownUnitsOfMeasure;
        }
    }
}