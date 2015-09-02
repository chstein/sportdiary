using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sporty.Business.Interfaces
{
    public interface IRepository<T>
    {
        T GetElement(Expression<Func<T, bool>> expressionToIdentify);
        void Add(T element);
        void Delete(Expression<Func<T, bool>> expressionToIdentify);
    }
}