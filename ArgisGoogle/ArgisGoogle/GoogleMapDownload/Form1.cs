using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GoogleMapDownload
{
    public partial class Form1 : Form
    {

        const int MAXThread = 600;
     
      
        public Form1()
        {
            InitializeComponent();

            System.Threading.ThreadPool.SetMaxThreads(MAXThread, MAXThread);
         
            //121.299405, 25.000298
        }
       

   

        long finishcnt = 0;
        long taskcnt=0;
        long finishbasecnt = 0;
        void downlaodTask(object state)
        {
            bool bsuccess=true;;
            do
            {
                Job job = (Job)state;
                try
                {
                    System.Drawing.Bitmap bmp = GoogleMapFile.GetMap(job.XTile, job.YTile, job.Level);

                    GoogleMapFile.SaveMap(bmp, job.XTile, job.YTile, job.Level);
                    bsuccess = true;
                }
                catch(Exception ex)
                {
                    bsuccess = false;
                    System.Threading.Thread.Sleep(1000 * 10);
                }
            } while (!bsuccess);
           finishcnt++;
        }

        void downlaodMapCollectionFileTask(object state)
        {
            bool bsuccess = true; ;
            do
            {
                Job job = (Job)state;
                try
                {
                    Stream stream = GoogleMapFile.GetMapStream(job.XTile, job.YTile, job.Level);
                    lock(this)
                    {
                    GoogleMapFile.AddCollectionMapFile(stream, job.XTile, job.YTile, job.Level,false);
                    }
                   // System.Drawing.Bitmap bmp = GoogleMapFile.GetMap(job.XTile, job.YTile, job.Level);

                  //  GoogleMapFile.SaveMap(bmp, job.XTile, job.YTile, job.Level);
                    bsuccess = true;
                }
                catch (Exception ex)
                {
                    bsuccess = false;
                    System.Threading.Thread.Sleep(1000 * 10);
                }
            } while (!bsuccess);
            finishcnt++;
        }

      
      

      
     

      
        private void button1_Click_1(object sender, EventArgs e)
        {
            int xtile, ytile, level, picx, picy;
            level = int.Parse(textBox3.Text);
            GoogleMapFile.LongitudeLatitude2GoogleTileXY(double.Parse(textBox1.Text), double.Parse(textBox2.Text),
                level, out xtile, out ytile, out picx, out picy);
            lblResult.Text = string.Format("x:{0},y:{1}", xtile, ytile);


            // old method  for single png
           //System.Drawing.Bitmap bmp = GoogleMapFile.GetMap(xtile, ytile, level);

            // new methods for mapcollection file

            if (!GoogleMapFile.IsMapCollectionTileMapExist(xtile, ytile, level))
            {
               
                System.IO.Stream stream = GoogleMapFile.GetMapStream(xtile, ytile, level);
                GoogleMapFile.AddCollectionMapFile(stream, xtile, ytile, level, false);
            }
            System.Drawing.Bitmap bmp = GoogleMapFile.GetCollectionFileBitMap(xtile, ytile, level);
           

            this.pictureBox1.Image = (Image)bmp;
            if (chkSaveMap.Checked)
                GoogleMapFile.SaveMap(bmp, xtile, ytile, level);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            double xmin, xmax, ymin, ymax;
            int level;
            xmin = double.Parse(txtXMin.Text);
            xmax = double.Parse(txtXmax.Text);
            ymin = double.Parse(txtYmin.Text);
            ymax = double.Parse(txtYMax.Text);
            level = (int)this.numericUpDown1.Value;
            int xStart, xEnd, yStart, yEnd,x,y;
            GoogleMapFile.LongitudeLatitude2GoogleTileXY(xmin, ymin, level,out xStart,out yEnd,out x,out y);
            GoogleMapFile.LongitudeLatitude2GoogleTileXY(xmax, ymax, level, out xEnd, out yStart, out x, out y);
            finishcnt = 0;
            this.progressBar1.Value = 0;
            Application.DoEvents();
            for (int i = xStart; i <= xEnd; i++)
            {
                for (int j = yStart; j <= yEnd; j++)
                {

                 //   if (GoogleMapFile.IsTileMapExist(i, j, level))
                    if(GoogleMapFile.IsMapCollectionTileMapExist(i,j,level))
                    {
                        finishcnt++;
                        finishbasecnt++;
                        if (finishcnt % 100 == 0)
                        {
                            this.progressBar1.Value = (int)((double)finishcnt / ((double)(xEnd - xStart + 1) * (yEnd - yStart + 1)) * 100);
                            lblcnt.Text = finishcnt.ToString();
                            Application.DoEvents();
                        }
                        continue;
                    }

                    bool ret = false;
                    do
                    {
                        try
                        {
                          //  ret = System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(downlaodTask), new Job() { Level = level, XTile = i, YTile = j });
                            ret = System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(downlaodMapCollectionFileTask), new Job() { Level = level, XTile = i, YTile = j });

                            if (ret)
                            {
                                taskcnt++;
                                break;
                            }
                        }
                        catch
                        {
                            ret = false;

                        }


                        if (!ret)
                            System.Threading.Thread.Sleep(1000);
                    }
                    while (true);




                }
                this.progressBar1.Value = (int)((double)finishcnt / ((double)(xEnd - xStart + 1) * (yEnd - yStart + 1)) * 100);
                lblcnt.Text = finishcnt.ToString();
                Application.DoEvents();

               while (taskcnt - (finishcnt-finishbasecnt) > 2*MAXThread)
                {
                    System.Threading.Thread.Sleep(1000);
                    this.progressBar1.Value = (int)((double)finishcnt / ((double)(xEnd - xStart + 1) * (yEnd - yStart + 1)) * 100);
                    lblcnt.Text = finishcnt.ToString();
                    Application.DoEvents();
                }
            }
            int wcnt, iocnt;
            do
            {
                this.progressBar1.Value =  (int)  ((double)finishcnt/ ( (double) (xEnd - xStart + 1) * (yEnd - yStart + 1))*100);
                lblcnt.Text = finishcnt.ToString();
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);
                System.Threading.ThreadPool.GetAvailableThreads(out wcnt, out iocnt);
            }
            while (wcnt != MAXThread);
            this.progressBar1.Value = 100;
            MessageBox.Show("Download completed!");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

    }

    class Job
    {
       public int XTile, YTile, Level;
    }
}
