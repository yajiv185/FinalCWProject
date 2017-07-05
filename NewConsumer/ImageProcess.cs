using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace NewConsumer
{
    public class ImageProcess
    {
       
        public void SaveImage(string url , int id_img)
        {
            int countImg;
           
            string _connString = ConfigurationManager.ConnectionStrings["DatabaseConncet"].ConnectionString;

            string defaultPath = ConfigurationManager.AppSettings["ImageSaveLocation"].ToString();
            string replaceP = ConfigurationManager.AppSettings["replacePath"].ToString();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imageDirectoryPath = baseDirectory.Replace(replaceP, defaultPath);
            string directoryName = string.Format(imageDirectoryPath + "{0}\\", id_img);



            var param = new DynamicParameters();
            param.Add("stockid", id_img, direction: ParameterDirection.Input);
            param.Add("count",dbType:DbType.Int32,direction: ParameterDirection.Output);

            try
            {
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    conn.Query("sp_UsedCarsGetImageCount", param, commandType: CommandType.StoredProcedure);
                    countImg = param.Get<int>("count");
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            //countImg = (countImg / 3);

            string saveOriginalPath = string.Format("{0}original_{1}.jpg", directoryName, countImg);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(@url, @saveOriginalPath);
            }
            Bitmap bitmap = new Bitmap(saveOriginalPath);
            Bitmap smallImage = StockImages.ResizeImage(bitmap, 310, 174);
            Bitmap largeImage = StockImages.ResizeImage(bitmap, 640, 348);


            string smallImgPath = string.Format("{0}/small_{1}.jpg", directoryName, countImg); ;
            string largeImgPath = string.Format("{0}/large_{1}.jpg", directoryName, countImg); ;

            smallImage.Save(@smallImgPath, ImageFormat.Jpeg);
            largeImage.Save(@largeImgPath, ImageFormat.Jpeg);

        }
        

    }
}