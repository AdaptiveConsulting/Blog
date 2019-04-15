using System;
using System.Collections.Generic;

namespace ExpressionParser.ReferentialData.Product
{
    public interface IProductRepository
    {
        IObservable<IEnumerable<Product>> GetAll();
    }
}