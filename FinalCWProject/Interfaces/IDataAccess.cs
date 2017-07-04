using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Interfaces
{
    public interface IDataAccess
    {
        ReadStock CreateDbStock(Stocks a);
        ReadStock ReadDbStock(int id);
        void DeleteDbStock(int id);
        ReadStock EditDbStock(int id, Stocks a);
        IEnumerable<ESGetDetail> GetAllStockDetail();
        IEnumerable<Cities> GetCityList();
    }
}
