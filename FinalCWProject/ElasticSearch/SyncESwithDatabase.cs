using Carwale.DAL.CoreDAL;
using Entities;
using Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DAL;

namespace ElasticSearch
{
    public class SyncESwithDatabase
    {
        public void CreateIndex()
        {
            ElasticClient client = ElasticClientInstance.GetInstance();
            var temp = client.CreateIndex("stockdata_g3_1", c => c
                         .NumberOfReplicas(0)
                         .NumberOfShards(1)
                         .AddMapping<ESGetDetail>(m => m.MapFromAttributes())
                     );
        }
        public void SyncIndex()
        {
            IDataAccess<Stocks> data = new DataAccessLayer();

            IEnumerable<ESGetDetail> allStockData = data.GetAllStockDetail();
            ElasticClient client = ElasticClientInstance.GetInstance();
            //CreateIndex();
            foreach (ESGetDetail read in allStockData)
            {
                client.Index(read, i => i
                    .Index("stockdata_g3_1")
                    .Type("esgetdetail")
                    .Id(read.ID.ToString())
                         );
            }
        }
    }
}
