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

         
            this.axCExpressViewerControl1.Dock = DockStyle.Fill;
            this.axCExpressViewerControl1.SourcePath = "rjdj009sfs.dwf";
           // axCExpressViewerControl1.ECompositeViewer
            CompositeViewer = (ECompositeViewer.IAdECompositeViewer)axCExpressViewerControl1.ECompositeViewer;
            CompositeViewer.BackColor =(uint) Color.Red.ToArgb();
            CompositeViewer.ExecuteCommand("SHOWALL");
           // CompositeViewer.UserInterfaceEnabled = false;
           CompositeViewer.WaitForSectionLoaded();
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
            AdCommon.IAdCollection objects = Content.get_Objects(0);
            foreach (ECompositeViewer.IAdObject obj in objects)
            {
                foreach (AdCommon.IAdProperty prop in obj.Properties)
                {
                    if (prop.Name == "Id" && prop.Value == id/* || prop.Value=="475848")*/)
                    {
                        myCollection.AddNamedItem(obj, prop.Value);
                        System.Diagnostics.Debug.Print(prop.Name + "," + prop.Value);

                    }
                }
            }
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
            CompositeViewer.ExecuteCommand("FITTOWINDOW");  //Isolates the selected object 
        }

        void axCExpressViewerControl1_OnEndLoadItem(object sender, AxExpressViewerDll.IAdViewerEvents_OnEndLoadItemEvent e)
        {
           
            //throw new NotImplementedException();
        }

        int tindex = 0;
        string[] ids = { "475841", "475848", "475855", "475862" };
        private void timer1_Tick(object sender, EventArgs e)
        {
            SelectObject(ids[tindex]);
            tindex = (tindex + 1) % 4;

        }
    }
}
