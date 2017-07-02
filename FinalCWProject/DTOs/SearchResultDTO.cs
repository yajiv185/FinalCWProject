using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTOs
{
    public class SearchResultDTO
    {
        public IEnumerable<Entities.ESGetDetail> ResultList{get;set;}
        public int NextPageId { get; set; }
        public int PageSize { get; set; }
        public int PreviousPageId { get; set; }
        public SearchResultDTO()
        {
            PageSize = 2;
        }
    }
}