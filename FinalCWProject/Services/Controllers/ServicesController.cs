using Cache;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Interfaces;
using System.Configuration;

namespace Services.Controllers
{
    public class ServicesController : ApiController
    {
        private ICache _stockDetails=new CacheLayer();
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
                //string imageDirectoryPath = string.Format("S:/FinalCWProject/FinalCWProject/UI/CarImages/{0}/", stockId);
                //Directory.CreateDirectory(Path.GetDirectoryName(imageDirectoryPath));
                string defaultPath = ConfigurationManager.AppSettings["ImageLocation"];
                string imageDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultPath);
                string imageDirectoryPath1 = string.Format("{0}{1}\\", imageDirectoryPath, stockId);
                Directory.CreateDirectory(Path.GetDirectoryName(imageDirectoryPath1));
                return Ok(string.Format("http://{0}:{1}/api/Stock/{2}",HttpContext.Current.Request.Url.Host,HttpContext.Current.Request.Url.Port, stockId));
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
                return Ok("Stock Updated Successfully");
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
                _stockDetails.DeleteStock(id);
                string defaultPath = ConfigurationManager.AppSettings["ImageLocation"];
                string imageDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultPath);
                string imageDirectoryPath1 = string.Format("{0}{1}\\", imageDirectoryPath, id);
                Directory.Delete(imageDirectoryPath1, true);
                return Ok("Stock Deleted Successfully");
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/Stock/{id}/Image/")]
        [HttpPost]
        public IHttpActionResult GenerateImage(int id, [FromBody]string imgUrl)
        {
            try
            {
                if (id > 0 && imgUrl != null)
                {
                    Produce.Sender(id, imgUrl);
                    return Ok("Your image is uploaded successfully :) ");
                }
                else
                {
                    return BadRequest("ImageURl or Id is not valid.");
                }
                
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
