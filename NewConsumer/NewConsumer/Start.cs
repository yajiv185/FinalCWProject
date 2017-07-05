using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace NewConsumer
{
    public class Start
    {
        public static void Main()
        {
            Receive imgReceiver = new Receive();
            imgReceiver.Receiver();

        }
    }
}