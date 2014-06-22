using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        ECompositeViewer.IAdECompositeViewer CompositeViewer;
        public Form1()
        {
            InitializeComponent();
          //  this.axCExpressViewerControl1.EditMode = false;

            Form1_Resize(null, null);
         //   this.axCExpressViewerControl1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.axCExpressViewerControl1.SourcePath = "rjdj009sfs.dwf";
           // axCExpressViewerControl1.ECompositeViewer
            CompositeViewer = (ECompositeViewer.IAdECompositeViewer)axCExpressViewerControl1.ECompositeViewer;
            CompositeViewer.BackColor =(uint) Color.Red.ToArgb();
           
            CompositeViewer.ExecuteCommand("SHOWALL");
         //   CompositeViewer.ExecuteCommand("FULLWINDOW");
           // CompositeViewer.UserInterfaceEnabled = false;
           CompositeViewer.WaitForSectionLoaded();
          
        //  CompositeViewer.ExecuteCommand("STANDARD_VIEW_FRONTTOPRIGHT");
           ParseObjects();
           SelectObject(ids[0]);
        //  timer1.Enabled = true;
         
//CompositeViewer.ExecuteCommand("FULLSCREEN");
            //this.axCExpressViewerControl1.OnEndLoadItem += (s,a) =>
            //    {

            //        if (a.bstrItemName == "SECTION")
            //        {
            //            ECompositeViewer.IAdSection CurrentSection = (ECompositeViewer.IAdSection)CompositeViewer.Section;
            //            MessageBox.Show(CurrentSection.Title);
            //        }
            //    };
          
        }
        System.Collections.Hashtable list = new System.Collections.Hashtable();
        void ParseObjects()
        {
            ECompositeViewer.IAdSection CurrentSection = (ECompositeViewer.IAdSection)CompositeViewer.Section;

            ECompositeViewer.IAdContent Content = (ECompositeViewer.IAdContent)CurrentSection.Content;

            ECompositeViewer.IAdSection SectionChk = (ECompositeViewer.IAdSection)CompositeViewer.Section;
            ECompositeViewer.IAdSectionType SectionTypeChk = (ECompositeViewer.IAdSectionType)SectionChk.SectionType;

            string TypeName = SectionTypeChk.Name;
            AdCommon.IAdUserCollection myCollection;
            //     MessageBox.Show(TypeName);
            myCollection = Content.CreateUserCollection();
            AdCommon.IAdCollection objects = Content.get_Objects(0);
            foreach (ECompositeViewer.IAdObject obj in objects)
            {
                foreach (AdCommon.IAdProperty prop in obj.Properties)
                {
                   for(int i=0;i<4;i++)
                    if (prop.Name == "Id" && prop.Value == ids[i]/* || prop.Value=="475848")*/)
                    {
                        if (!list.ContainsKey(ids[i]))
                        {
                            list.Add(ids[i], obj);
                            continue;
                        }
                       // myCollection.AddNamedItem(obj, prop.Value);
                      //  System.Diagnostics.Debug.Print(prop.Name + "," + prop.Value);
                        

                    }
                }
            }
        }
        void SelectObject(string id)
        {
            ECompositeViewer.IAdSection CurrentSection = (ECompositeViewer.IAdSection)CompositeViewer.Section;

            ECompositeViewer.IAdContent Content = (ECompositeViewer.IAdContent)CurrentSection.Content;

            ECompositeViewer.IAdSection SectionChk = (ECompositeViewer.IAdSection)CompositeViewer.Section;
            ECompositeViewer.IAdSectionType SectionTypeChk = (ECompositeViewer.IAdSectionType)SectionChk.SectionType;

            string TypeName = SectionTypeChk.Name;
            AdCommon.IAdUserCollection myCollection;
            //     MessageBox.Show(TypeName);
            myCollection = Content.CreateUserCollection();
            if (!list.ContainsKey(id))
                return;
            myCollection.AddNamedItem(list[id], id);
            //AdCommon.IAdCollection objects = Content.get_Objects(0);
            //foreach (ECompositeViewer.IAdObject obj in objects)
            //{
            //    foreach (AdCommon.IAdProperty prop in obj.Properties)
            //    {
            //        if (prop.Name == "Id" && prop.Value == id/* || prop.Value=="475848")*/)
            //        {
            //            myCollection.AddNamedItem(obj, prop.Value);
            //            System.Diagnostics.Debug.Print(prop.Name + "," + prop.Value);

            //        }
            //    }
            //}
            int Idx = 0;
            AdCommon.IAdCollection Commands = (AdCommon.IAdCollection)CompositeViewer.Commands;
            //Idx = 1;
            //foreach (ECompositeViewer.IAdCommand Command in Commands) { if (Commands.get_ItemName(Idx++) == "FULLVIEW") {
            //    Command.Enabled = true;
            //} }
            //Make the selection active.
            Content.set_Objects(1, myCollection);

            if (SectionTypeChk.Name == "com.autodesk.dwf.eModel")
            {
                EModelViewer.IAdEModelSection Section = (EModelViewer.IAdEModelSection)CompositeViewer.Section;
                EModelViewer.IAdEModelCamera Camera = (EModelViewer.IAdEModelCamera)Section.Camera;

                //Set the camera field
                AdCommon.IAdPoint PointField;//ew AdCommon.CAdPointClass();
                //      PointField.X = 88.82; PointField.Y = 78.51; Camera.Field = PointField; 
                //Get the camera field 
                PointField = (AdCommon.IAdPoint)Camera.Field;
                PointField.X = 100;
                PointField.Y = 100;
                Camera.Field = PointField;
                //       PointField.X = decimal.Parse (PointField.X.ToString ())  - 50; PointField.Y = decimal.Parse (PointField.Y.ToString ()) - 50; Camera.Field = PointField;
                //Set the new point to zoom in
                Section.Camera = (EModelViewer.IAdEModelCamera)Camera;
                //Set the new camera
            }


        //    CompositeViewer.ExecuteCommand("SELECT");
            CompositeViewer.ExecuteCommand("FITTOWINDOW");  //Isolates the selected object 
            //
            //CompositeViewer.ExecuteCommand("INVERTSELECTION");
            //CompositeViewer.ExecuteCommand("TRANSPARENT");
         
        }

        void axCExpressViewerControl1_OnEndLoadItem(object sender, AxExpressViewerDll.IAdViewerEvents_OnEndLoadItemEvent e)
        {
           
            //throw new NotImplementedException();
        }

        int tindex = 0;
      // string[] ids = { "sensor000035", "sensor000036", "sensor000136", "sensor000135", "sensor000039", "sensor000040", "sensor000139","sensor000140" };
        string[] ids = { "475841", "475848", "475855", "475862" };
        private void timer1_Tick(object sender, EventArgs e)
        {
            SelectObject(ids[tindex]);
            tindex = (tindex + 1) % 4;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectObject(ids[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectObject(ids[1]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelectObject(ids[2]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SelectObject(ids[3]);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.panel1.Dock = DockStyle.Right;
            this.axCExpressViewerControl1.Left = 0;
            this.axCExpressViewerControl1.Top = 0;
            this.axCExpressViewerControl1.Width = this.Width - panel1.Width-15;
            this.axCExpressViewerControl1.Height = this.Height;
        }
    }
}
