namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axCExpressViewerControl1 = new AxExpressViewerDll.AxCExpressViewerControl();
            ((System.ComponentModel.ISupportInitialize)(this.axCExpressViewerControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axCExpressViewerControl1
            // 
            this.axCExpressViewerControl1.Enabled = true;
            this.axCExpressViewerControl1.Location = new System.Drawing.Point(12, 42);
            this.axCExpressViewerControl1.Name = "axCExpressViewerControl1";
            this.axCExpressViewerControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCExpressViewerControl1.OcxState")));
            this.axCExpressViewerControl1.Size = new System.Drawing.Size(871, 654);
            this.axCExpressViewerControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 724);
            this.Controls.Add(this.axCExpressViewerControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axCExpressViewerControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxExpressViewerDll.AxCExpressViewerControl axCExpressViewerControl1;
    }
}

