using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.UI.WebControls;
using Sporty.DataModel;

namespace Sporty.Business.Interfaces
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected SportyEntities context;

        public BaseRepository(SportyEntities context)
        {
            this.context = context;
        }

        #region IRepository<TEntity> Members

        //public List<TEntity> GetAll()
        //{
        //    return context..GetTable<TEntity>().ToList<TEntity>();
        //}

        public TEntity GetElement(Expression<Func<TEntity, bool>> expressionToIdentify)
        {
            return context.Set<TEntity>().Where(expressionToIdentify).FirstOrDefault();
        }

        public void Add(TEntity element)
        {
            context.Set<TEntity>().Add(element);
            Save();
        }

        public void Delete(Expression<Func<TEntity, bool>> expressionToIdentify)
        {
            TEntity element = GetElement(expressionToIdentify);
            context.Set<TEntity>().Remove(element);
            Save();
        }

        #endregion

        private int Save()
        {
            return context.SaveChanges();
        }

        public void Update()
        {
            Save();
        }

        protected IEnumerable<TEntity> OrderBy<T>(IEnumerable<TEntity> list, string sortColumnName,
                                                  SortDirection sortDirection)
        {
            if (sortColumnName.Length > 0)
            {
                PropertyInfo prop = typeof (TEntity).GetProperty(sortColumnName);

                if (prop == null)
                {
                    throw new Exception("No property '" + sortColumnName + "' in + " + typeof (T).Name + "'");
                }

                if (sortDirection == SortDirection.Descending)
                    return list.OrderByDescending(x => prop.GetValue(x, null));
                return list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }
    }
}