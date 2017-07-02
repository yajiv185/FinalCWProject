using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using DAL;
using Entities;
using ElasticSearch;

namespace Cache
{
    public class CacheLayer
    {
        private DataAccessLayer _dataAccessLayer = new DataAccessLayer();
        public ReadStock GetStock(int stockId)
        {
            ReadStock carDetails = null;
            try
            {
                string cacheKey = CreateKey(stockId);
                using (MemcachedClient _client = new MemcachedClient("memcached"))
                {
                    carDetails = (ReadStock)_client.Get(cacheKey);
                    if (carDetails == null)
                    {
                        carDetails = _dataAccessLayer.ReadDbStock(stockId);
                        _client.Store(StoreMode.Add, cacheKey, carDetails, DateTime.Now.AddMinutes(60));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return carDetails;
        }
        public void UpdateStock(int stockId, Stocks stock)
        {
            string cacheKey = CreateKey(stockId);
            ReadStock getUpdatedData = new ReadStock();
            ElasticSearchClient updateES = new ElasticSearchClient();
            ESGetDetail getUpdatedESData = new ESGetDetail();
            try
            {

                getUpdatedData = _dataAccessLayer.EditDbStock(stockId, stock);
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {
                    client.Store(StoreMode.Set, cacheKey, getUpdatedData, DateTime.Now.AddMinutes(60));
                }
                getUpdatedESData = ConvertCachetoESData(getUpdatedData);
                updateES.UpdateESStock(stockId, getUpdatedESData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateStock(Stocks stock)
        {
            int _newStockid = 0;
            try
            {
                ReadStock getCreatedData = new ReadStock();
                ESGetDetail getCreatedESData = new ESGetDetail();
                ElasticSearchClient createES = new ElasticSearchClient();
                getCreatedData = _dataAccessLayer.CreateDbStock(stock);
                string cacheKey = CreateKey(getCreatedData.ID);
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {
                    client.Store(StoreMode.Add, cacheKey, getCreatedData, DateTime.Now.AddMinutes(15));
                }
                getCreatedESData = ConvertCachetoESData(getCreatedData);
                createES.CreateESStock(getCreatedESData);
                _newStockid = getCreatedESData.ID;
            }
            catch (Exception)
            {
                throw;
            }
            return _newStockid;
        }
        public int DeleteStock(int stockId)
        {
            try
            {
                ElasticSearchClient deleteES = new ElasticSearchClient();
                deleteES.DeleteESStock(stockId);
                _dataAccessLayer.DeleteDbStock(stockId);
            }
            catch (Exception)
            {
                throw;
            }
            return stockId;
        }

        public string CreateKey(int carId)
        {
            return string.Format("UsedCar_{0}", carId);
        }

        private ESGetDetail ConvertCachetoESData(ReadStock getUpdatedData)
        {
            ESGetDetail convertedData = new ESGetDetail();
            convertedData.carcompany = getUpdatedData.carcompany;
            convertedData.ID = getUpdatedData.ID;
            convertedData.Price = getUpdatedData.Price;
            convertedData.Year = getUpdatedData.Year;
            convertedData.Kilometers = getUpdatedData.Kilometers;
            convertedData.FuelType = getUpdatedData.FuelType;
            convertedData.City = getUpdatedData.City;
            convertedData.modelname = getUpdatedData.modelname;
            convertedData.carversionname = getUpdatedData.carversionname;
            return convertedData;
        }
    }
}