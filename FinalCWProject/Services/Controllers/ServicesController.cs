using Cache;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace Services.Controllers
{
    public class ServicesController : ApiController
    {
        private CacheLayer _stockDetails = new CacheLayer();
        [Route("api/Stock/")]
        [HttpPost]
        public IHttpActionResult CreateStock([FromBody] Stocks stock)
        {
            try
            {
                if (!Validate(stock))
                {
                    return BadRequest("Enter correct values for stock");
                }
                int stockId = _stockDetails.CreateStock(stock);
                string imageDirectoryPath = string.Format("S:/FirstCWProject/UsedCars/Services/CarImages/{0}/", stockId);
                Directory.CreateDirectory(Path.GetDirectoryName(imageDirectoryPath));
                return Ok(stockId);
            }
            catch
            {
                return InternalServerError();
            }

        }

        [Route("api/Stock/{stockId}")]
        [HttpPut]
        public IHttpActionResult UpdateStock(int stockId, [FromBody] Stocks stock)
        {
            try
            {
                if (stockId < 0 && (!Validate(stock)))
                {
                    return BadRequest("Bad Input");
                }
                _stockDetails.UpdateStock(stockId, stock);
                return Ok(stockId);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/Stock/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteStock(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Bad Input");
                }
                int stockId = _stockDetails.DeleteStock(id);
                return Ok(stockId);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/Stock/{id}/Image/")]
        [HttpPost]
        public IHttpActionResult GenerateImage(int id, [FromBody]Display stockImage)
        {
            try
            {
                if (id > 0 && stockImage.imgUrl != null)
                {
                    Produce sendobj = new Produce();
                    sendobj.sender(id, stockImage.imgUrl);
                }
                return Ok("Your image is uploaded successfully :) ");
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        private bool Validate(Stocks stock)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            if (stock.Price < 10000 && stock.Price > 50000000)
            {
                return false;
            }
            if (stock.Year < 2010 && stock.Year > DateTime.Now.Year)
            {
                return false;
            }
            if (stock.Kilometers <= 100 && stock.Kilometers >= 300000)
            {
                return false;
            }
            if (stock.FuelEconomy != 0 && stock.FuelEconomy < 1 && stock.FuelEconomy > 50)
            {
                return false;
            }
            return true;
        }
    }
}
