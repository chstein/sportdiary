using System.Collections.Generic;
using System.Data.Linq;

namespace Sporty.DataModel
{
    public interface ITableWrapper<TEntity>
        where TEntity : class
    {
        IEnumerable<TEntity> Collection { get; }
        void InsertOnSubmit(TEntity entity);
    }

    public class TableWrapper<TEntity> : ITableWrapper<TEntity>
        where TEntity : class
    {
        private readonly Table<TEntity> table;

        public TableWrapper(Table<TEntity> table)
        {
            this.table = table;
        }

        #region ITableWrapper<TEntity> Members

        public IEnumerable<TEntity> Collection
        {
            get { return table; }
        }

        public void InsertOnSubmit(TEntity entity)
        {
            table.InsertOnSubmit(entity);
        }

        #endregion
    }
}