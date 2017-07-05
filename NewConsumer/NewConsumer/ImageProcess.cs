using System;
using System.Collections.Generic;
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
            int countImg = 1;

            string directoryName = string.Format("S:/FinalCWProject/FinalCWProject/UI/CarImages/{0}", id_img);
            countImg = Directory.GetFiles(directoryName).Length;
            countImg = (countImg / 3) + 1;

            string saveOriginalPath = string.Format("{0}/original_{1}.jpg", directoryName, countImg);
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