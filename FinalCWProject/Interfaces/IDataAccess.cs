using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Interfaces
{
    public interface IDataAccess<T>
    {
        ReadStock CreateDbStock(T a);
        ReadStock ReadDbStock(int id);
        void DeleteDbStock(int id);
        ReadStock EditDbStock(int id, T a);
        IEnumerable<ESGetDetail> GetAllStockDetail();
    }
}
