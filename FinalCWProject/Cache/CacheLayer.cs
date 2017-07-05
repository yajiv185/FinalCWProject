using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using DAL;
using Interfaces;
using Entities;
using ElasticSearch;

namespace Cache
{
    public class CacheLayer : ICache
    {
        private IDataAccess _dataAccessLayer;
        public CacheLayer()
        { 
         _dataAccessLayer = new DataAccessLayer();
        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="stockId"></param>
     /// <returns></returns>
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
                        if (_client.Store(StoreMode.Add, cacheKey + "_lock", "lock", DateTime.Now.AddSeconds(30)))
                        {
                            carDetails = _dataAccessLayer.ReadDbStock(stockId);
                            _client.Store(StoreMode.Add, cacheKey, carDetails, DateTime.Now.AddMinutes(15));
                            _client.Remove(cacheKey + "_lock");
                        }
                        else
                        {
                            carDetails = _dataAccessLayer.ReadDbStock(stockId);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return carDetails;
        }
        public void UpdateStock(int stockId, Stocks stock)
        {
            string cacheKey = CreateKey(stockId);
            ReadStock updatedData = new ReadStock();
            ElasticSearchClient updateES = new ElasticSearchClient();
            ESGetDetail updatedESData = new ESGetDetail();
            try
            {

                updatedData = _dataAccessLayer.EditDbStock(stockId, stock);
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {
                    client.Store(StoreMode.Set, cacheKey, updatedData, DateTime.Now.AddMinutes(60));
                }
                updatedESData = ConvertCachetoESData(updatedData);
                //getCreatedData = Mapper.Map<ReadStock, ESGetDetail>(getCreatedData);
                updateES.UpdateESStock(stockId, updatedESData);
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
                ReadStock createdData = new ReadStock();
                ESGetDetail createdESData = new ESGetDetail();
                ElasticSearchClient createES = new ElasticSearchClient();
                createdData = _dataAccessLayer.CreateDbStock(stock);
                string cacheKey = CreateKey(createdData.ID);
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {
                    client.Store(StoreMode.Add, cacheKey, createdData, DateTime.Now.AddMinutes(15));
                }
                createdESData = ConvertCachetoESData(createdData);
                //getCreatedData = Mapper.Map<ReadStock, ESGetDetail>(getCreatedData);
                createES.CreateESStock(createdESData);
                _newStockid = createdESData.ID;
            }
            catch (Exception)
            {
                throw;
            }
            return _newStockid;
        }
        public void DeleteStock(int stockId)
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
           
        }
        public IEnumerable<Cities> GetCityList()
        {
            IEnumerable<Cities> _cities = null;
            try
            {
                const string cacheKey = "UsedCar_City_G3";
                using (MemcachedClient _client = new MemcachedClient("memcached"))
                {
                    _cities = (IEnumerable<Cities>)_client.Get(cacheKey);
                    if (_cities == null)
                    {
                        _cities = _dataAccessLayer.GetCityList();
                        _client.Store(StoreMode.Add, cacheKey, _cities,DateTime.Now.AddMonths(1));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _cities;
        }

        private string CreateKey(int carId)
        {
            return string.Format("UsedCar_{0}", carId);
        }

        private ESGetDetail ConvertCachetoESData(ReadStock getUpdatedData)
        {
            ESGetDetail convertedData = new ESGetDetail();
            if (getUpdatedData!=null)
            {
                convertedData.CarCompany = getUpdatedData.CarCompany;
                convertedData.ID = getUpdatedData.ID;
                convertedData.Price = getUpdatedData.Price;
                convertedData.Year = getUpdatedData.Year;
                convertedData.Kilometers = getUpdatedData.Kilometers;
                convertedData.FuelType = getUpdatedData.FuelType;
                convertedData.City = getUpdatedData.City;
                convertedData.ModelName = getUpdatedData.ModelName;
                convertedData.CarVersion = getUpdatedData.CarVersion; 
            }
            return convertedData;
        }
    }
}