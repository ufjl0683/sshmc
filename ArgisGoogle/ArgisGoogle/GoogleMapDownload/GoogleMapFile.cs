using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GoogleMapDownload
{
   public  class GoogleMapFile
    {
       static string GoogleMapUrl = "http://mt1.google.com/vt/lyrs=m@129&hl=zh-TW&x={0}&y={1}&z={2}";
      static object lockobj = new object();
      public static  void LongitudeLatitude2GoogleTileXY(double longitude, double latitude, int level, out int tilex, out int tiley, out int pixX, out int pixY)
      //level 0~17
      // long:-180~180  lat: -85.0511288~85.0511288
      {
          //  latitude = 90 - latitude;

          //correct the longitude to go from 0 to 360
          // longitude = 180 + longitude;

          //find tile size from zoom level
          // double latTileSize =  180 /(Math.Pow(2, level));
          //  double longTileSize = 360 / (Math.Pow(2, level));

          //find the tile coordinates
          // tilex = (int)(longitude / longTileSize);
          //  tiley = (int)(latitude / latTileSize);

          double sinLatitude = Math.Sin(latitude * Math.PI / 180);

          double pixelX = ((longitude + 180) / 360) * 256 * Math.Pow(2, level);

          double pixelY = (0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI)) * 256 * Math.Pow(2, level);

          tilex = (int)(pixelX / 256);
          tiley = (int)(pixelY / 256);

          pixX = (int)pixelX % 256;
          pixY = (int)pixelY % 256;
      }
      public static bool IsMapCollectionTileMapExist(string PathFileName, int x, int y, int z)
       {
         
               long mapinx = 0;
               long mapbegin = 0;
               // long mapLength = 0;
               FileStream fs;
               mapinx = x % 100 * 100 + y % 100;
               fs = File.Open(PathFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
               fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin); // find the inxbeg
               //  fs.Seek(sizeof(long), SeekOrigin.Current); //skip inx
               byte[] tempbytes = new byte[sizeof(long)];
               fs.Read(tempbytes, 0, sizeof(long));
               mapbegin = BitConverter.ToInt64(tempbytes, 0);
               //fs.Read(tempbytes, 0, sizeof(long));
               //mapLength = BitConverter.ToInt64(tempbytes, 0);

               return mapbegin != 0;
          
           
           //if (mapbegin == 0)  //new map
       }
       public static bool IsMapCollectionTileMapExist(int x, int y, int z)
       {

         
               string filename = GetMapCollectionFileName(x, y, z);

               return System.IO.File.Exists(filename) && IsMapCollectionTileMapExist(filename, x, y, z);
         
          
       }
       
       // public static System.IO.Stream GetMapStream(int x, int y, int z)
       //{
       //    string url = GoogleMapUrl;
       //    string urlstr = string.Format(url, x, y, z);
       //    System.Net.WebHeaderCollection whc = new System.Net.WebHeaderCollection();

       //    System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlstr);// e(urlstr);

       //    wreq.UserAgent = "Mozilla/5.0";
       //    System.IO.Stream stream = wreq.GetResponse().GetResponseStream(); //  System.IO.Stream stream=  wc.OpenRead(string.Format(url, 0, 0, 0));


       //    return stream;
       //}

       public static System.Drawing.Bitmap GetMap(int x, int y, int z)
       {
           string url = GoogleMapUrl;
           string urlstr = string.Format(url, x, y, z);
           // System.Net.WebHeaderCollection whc=new System.Net.WebHeaderCollection();

           //  whc.Add(urlstr);
           // MessageBox.Show(urlstr);
           System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlstr);// e(urlstr);
           //  System.IO.MemoryStream ms = new System.IO.MemoryStream();
           //wreq.Headers = whc;
           //   byte[] imagebytes=wreq.DownloadData(urlstr);
           //   ms.Write(imagebytes,0,imagebytes.Length);
           // wreq.Headers.Add(System.Net.HttpRequestHeader.Referer, "http://http://mt1.google.com");
           // wreq.Headers.Add("user-agent", @"Mozilla/5.0");
           wreq.UserAgent = "Mozilla/5.0";
           System.IO.Stream stream = wreq.GetResponse().GetResponseStream();
           //  System.IO.Stream stream=  wc.OpenRead(string.Format(url, 0, 0, 0));


           return System.Drawing.Bitmap.FromStream(stream) as System.Drawing.Bitmap;

       }
       public static System.Drawing.Bitmap GetCollectionFileBitMap(string PathFileName, int x, int y)
       {
           long mapinx = 0;
           long mapbegin = 0;
           long mapLength = 0;
           FileStream fs;
           mapinx = x % 100 * 100 + y % 100;
           fs = File.Open(PathFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
           fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin); // find the inxbeg
           //  fs.Seek(sizeof(long), SeekOrigin.Current); //skip inx
           byte[] tempbytes = new byte[sizeof(long)];
           fs.Read(tempbytes, 0, sizeof(long));
           mapbegin = BitConverter.ToInt64(tempbytes, 0);
           fs.Read(tempbytes, 0, sizeof(long));
           mapLength = BitConverter.ToInt64(tempbytes, 0);
           fs.Seek(mapbegin, SeekOrigin.Begin);
           System.IO.MemoryStream ms = new MemoryStream();
           byte[] data=new byte[mapLength];
           fs.Read(data, 0, data.Length);
           ms.Write(data, 0, data.Length);
           ms.Seek(0, SeekOrigin.Begin);
           System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
           return bmp;

       }
       public static System.Drawing.Bitmap GetCollectionFileBitMap( int x, int y,int z)
       {
           long mapinx = 0;
           long mapbegin = 0;
           long mapLength = 0;
           FileStream fs;
           string PathFileName = GetMapCollectionFileName(x, y, z);
           mapinx = x %100 * 100 + y % 100;
           fs = File.Open(PathFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
           fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin); // find the inxbeg
           //  fs.Seek(sizeof(long), SeekOrigin.Current); //skip inx
           byte[] tempbytes = new byte[sizeof(long)];
           fs.Read(tempbytes, 0, sizeof(long));
           mapbegin = BitConverter.ToInt64(tempbytes, 0);
           fs.Read(tempbytes, 0, sizeof(long));
           mapLength = BitConverter.ToInt64(tempbytes, 0);
           fs.Seek(mapbegin, SeekOrigin.Begin);
           System.IO.MemoryStream ms = new MemoryStream();
           byte[] data = new byte[mapLength];
           fs.Read(data, 0, data.Length);
           ms.Write(data, 0, data.Length);
           ms.Seek(0, SeekOrigin.Begin);
           System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
           return bmp;

       }
       public  static System.IO.Stream GetMapStream(int x, int y, int z)
       {
           string url = GoogleMapUrl;
           string urlstr = string.Format(url, x, y, z);

           System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(urlstr);// e(urlstr);

           wreq.UserAgent = "Mozilla/5.0";
           return wreq.GetResponse().GetResponseStream();
       }
       public static void AddCollectionMapFile(string PathFileName,Stream mapstream,int x ,int y,bool bUpdate)
       {

          
               long mapinx = 0;
               long mapbegin = 0;
               long mapLength = 0;
               FileStream fs;

               if (!System.IO.File.Exists(PathFileName))
               {
                   fs = System.IO.File.Open(PathFileName, FileMode.CreateNew, FileAccess.ReadWrite);

                   byte[] inxbytes = new byte[sizeof(long)];

                   for (int i = 0; i < 10000; i++)
                   {
                       // fs.Write(inxbytes, 0, inxbytes.Length);  //inx
                       fs.Write(inxbytes, 0, inxbytes.Length);   //map beg pointer
                       fs.Write(inxbytes, 0, inxbytes.Length);  //length
                   }
                   fs.Flush();

               }
               else
                   fs = File.Open(PathFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);



               mapinx = x % 100 * 100 + y % 100;

               fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin); // find the inxbeg
               //  fs.Seek(sizeof(long), SeekOrigin.Current); //skip inx
               byte[] tempbytes = new byte[sizeof(long)];
               fs.Read(tempbytes, 0, sizeof(long));
               mapbegin = BitConverter.ToInt64(tempbytes, 0);
               fs.Read(tempbytes, 0, sizeof(long));
               mapLength = BitConverter.ToInt64(tempbytes, 0);

               if (mapbegin == 0 || bUpdate)  //new map
                   mapbegin = fs.Seek(0, SeekOrigin.End);
               else


                   return;  //已經存在 離開



               byte[] buffer = new byte[512];
               int len = 0;

               while ((len = mapstream.Read(buffer, 0, buffer.Length)) > 0)
               {
                   mapLength += len;
                   fs.Write(buffer, 0, len);
               }

               fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin);
               fs.Write(BitConverter.GetBytes(mapbegin), 0, sizeof(long));  //begin
               fs.Write(BitConverter.GetBytes(mapLength), 0, sizeof(long)); //len
               fs.Flush();
               fs.Close();
               fs.Dispose();
         

       }

       public static void AddCollectionMapFile( Stream mapstream, int x, int y,int z, bool bUpdate)
       {
          
               //long mapinx = 0;
               //long mapbegin = 0;
               //long mapLength = 0;
               //FileStream fs;
               string path = Environment.CurrentDirectory + "\\Map\\" + z + "\\" + (x / 100) + "_" + (y / 100);
               if (!System.IO.Directory.Exists(path))
               {
                   System.IO.Directory.CreateDirectory(path);
               }
               string PathFileName = GetMapCollectionFileName(x, y, z);
               AddCollectionMapFile(PathFileName, mapstream, x, y,bUpdate);

               //if (!System.IO.File.Exists(PathFileName))
               //{
               //    fs = System.IO.File.Create(PathFileName);

               //    byte[] inxbytes = new byte[sizeof(long)];

               //    for (int i = 0; i < 10000; i++)
               //    {
               //        // fs.Write(inxbytes, 0, inxbytes.Length);  //inx
               //        fs.Write(inxbytes, 0, inxbytes.Length);   //map beg pointer
               //        fs.Write(inxbytes, 0, inxbytes.Length);  //length
               //    }
               //    fs.Flush();

               //}
               //else
               //    fs = File.Open(PathFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);



               //mapinx = x % 100 * 100 + y % 100;

               //fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin); // find the inxbeg
               ////  fs.Seek(sizeof(long), SeekOrigin.Current); //skip inx
               //byte[] tempbytes = new byte[sizeof(long)];
               //fs.Read(tempbytes, 0, sizeof(long));
               //mapbegin = BitConverter.ToInt64(tempbytes, 0);
               //fs.Read(tempbytes, 0, sizeof(long));
               //mapLength = BitConverter.ToInt64(tempbytes, 0);

               //if (mapbegin == 0 || bUpdate)  //new map
               //    mapbegin = fs.Seek(0, SeekOrigin.End);
               //else


               //    return;  //已經存在 離開



               //byte[] buffer = new byte[512];
               //int len = 0;

               //while ((len = mapstream.Read(buffer, 0, buffer.Length)) > 0)
               //{
               //    mapLength += len;
               //    fs.Write(buffer, 0, len);
               //}

               //fs.Seek(mapinx * sizeof(long) * 2, SeekOrigin.Begin);
               //fs.Write(BitConverter.GetBytes(mapbegin), 0, sizeof(long));  //begin
               //fs.Write(BitConverter.GetBytes(mapLength), 0, sizeof(long)); //len
               //fs.Flush();
               //fs.Close();
               //fs.Dispose();
         


       }
       public static  string GetMapCollectionFileName(int x, int y, int z)
       {
           string path = Environment.CurrentDirectory + "\\Map\\" + z + "\\" + (x / 100) + "_" + (y / 100);
           //if (!System.IO.Directory.Exists(path))
           //{
           //    System.IO.Directory.CreateDirectory(path);
           //}
           return path + "\\Map.col";
       }
       public static string GetMapFileName(int x, int y, int z)
       {
           string path = Environment.CurrentDirectory + "\\Map\\" + z + "\\" + (x / 100) + "_" + (y / 100);
           if (!System.IO.Directory.Exists(path))
           {
               System.IO.Directory.CreateDirectory(path);
           }
           return path + "\\" + x + "_" + y + ".png";
       }
       public static  void SaveMap(System.Drawing.Bitmap bmp, int x, int y, int z)
       {
         //  this.pictureBox1.Image = bmp as Image;
           string path = Environment.CurrentDirectory + "\\Map\\" + z + "\\" + (x / 100) + "_" + (y / 100);
           if (!System.IO.Directory.Exists(path))
           {
               System.IO.Directory.CreateDirectory(path);
           }
           bmp.Save(path + "\\" + x + "_" + y + ".png");
       }
      
       public static  bool IsTileMapExist(int x, int y, int z)
       {
           string path = Environment.CurrentDirectory + "\\Map\\" + z + "\\" + (x / 100) + "_" + (y / 100);
           //if (!System.IO.Directory.Exists(path))
           //{
           //    //  System.IO.Directory.CreateDirectory(path);
           //    return false;
           //}

           return System.IO.File.Exists(path + "\\" + x + "_" + y + ".png");
       }
   }
}
