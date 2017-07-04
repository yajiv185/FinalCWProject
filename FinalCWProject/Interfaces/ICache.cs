using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICache
    {
        IEnumerable<Cities> GetCityList();
        void DeleteStock(int stockId);
        int CreateStock(Stocks stock);
        void UpdateStock(int stockId, Stocks stock);
        ReadStock GetStock(int stockId);
    }
}
