using Carwale.DAL.CoreDAL;
using Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    public class ElasticSearchClient
    {
        private ElasticClient _client = ElasticClientInstance.GetInstance();
        private QueryContainer _queryStore;
        private TermQuery _stocksByCity = new TermQuery()
        {
            Field = "city"
        };
        private RangeQuery _stocksByBudget = new RangeQuery()
        {
            Field = "price"
        };

        private SearchRequest MakeSearchRequest(int page, int pageSize, QueryContainer query)
        {
            var searchRequest = new SearchRequest
            {
                From = page,
                Size = pageSize,
                Query = query
            };
            return searchRequest;
        }

        public IEnumerable<ESGetDetail> GetAllStocks(int page, int pageSize)
        {
            _queryStore = new MatchAllQuery();
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }

        public IEnumerable<ESGetDetail> GetStocksByCity(string cityName, int page, int pageSize)
        {
            _stocksByCity.Value = cityName;
            _queryStore = _stocksByCity;
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }

        public IEnumerable<ESGetDetail> GetStocksByBudget(string minValue, string maxValue, int page, int pageSize)
        {
            _stocksByBudget.GreaterThanOrEqualTo = (minValue);
            _stocksByBudget.LowerThanOrEqualTo = (maxValue);
            _queryStore = _stocksByBudget;
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }

        public IEnumerable<ESGetDetail> GetStocksByCityAndPrice(string cityName, string minValue, string maxValue, int page, int pageSize)
        {
            _stocksByCity.Value = cityName;
            _stocksByBudget.GreaterThanOrEqualTo = minValue;
            _stocksByBudget.LowerThanOrEqualTo = maxValue;
            _queryStore = _stocksByCity && _stocksByBudget;
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }


        public void CreateESStock(ESGetDetail createdStock)
        {
            int id = createdStock.ID;
            _client.Index(createdStock, i => i
                .Id(id.ToString())
                     );
        }

        public void UpdateESStock(int stockId, ESGetDetail updatedStock)
        {
            _client.Update<ESGetDetail, object>(u => u
                .Id(stockId)
                .Doc(updatedStock)
                .RetryOnConflict(3)
                .Refresh()
            );
        }
        public void DeleteESStock(int stockId)
        {
            _client.DeleteByQuery<ESGetDetail>(q => q
            .AllIndices()
            .Query(rq => rq
                .Term(f => f.ID, stockId)
               )
           );
        }
    }
}
