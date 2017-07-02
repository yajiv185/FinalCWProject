using Entities;
using Interfaces;
using System;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;


namespace DAL
{
    public class DataAccessLayer : IDataAccess<Stocks>
    {
        private string _connString = ConfigurationManager.ConnectionStrings["DatabaseConncet"].ConnectionString;
        public ReadStock CreateDbStock(Stocks stock)
        {
            ReadStock stockDetail = new ReadStock();
            try
            {
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    stockDetail = conn.Query<ReadStock>("sp_UsedCarsCreate", stock, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return stockDetail;
        }

        public ReadStock EditDbStock(int stockId, Stocks stock)
        {
            ReadStock stockDetail = new ReadStock();
            try
            {
                stock.StockId = stockId;
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    stockDetail = conn.Query<ReadStock>("sp_UsedCarsEdit", stock, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return stockDetail;
        }

        public void DeleteDbStock(int stockId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("stockId", stockId, direction: ParameterDirection.Input);
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    conn.Execute("sp_UsedCarsDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ReadStock ReadDbStock(int stockId)
        {

            ReadStock stockDetail = new ReadStock();
            try
            {
                var param = new DynamicParameters();
                param.Add("stockId", stockId, direction: ParameterDirection.Input);
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    stockDetail = conn.Query<ReadStock>("sp_UsedCarsGetData", param, commandType: CommandType.StoredProcedure).AsList()[0];
                }
            }
            catch (Exception)
            {
                throw;
            }

            return stockDetail;
        }

        public IEnumerable<ESGetDetail> GetAllStockDetail()
        {
            IEnumerable<ESGetDetail> stockDetail;
            using (IDbConnection conn = new MySqlConnection(_connString))
            {
                stockDetail = conn.Query<ESGetDetail>("sp_AllUsedCarsGetData", commandType: CommandType.StoredProcedure);
            }
            return stockDetail;
        }
    }
}