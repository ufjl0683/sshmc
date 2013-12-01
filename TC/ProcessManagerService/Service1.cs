using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

using System.ServiceProcess;
using System.Text;

namespace ProcessManagerService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        public ProcessManager process_manager;
        protected override void OnStart(string[] args)
        {
            process_manager = new ProcessManager();
        }

        protected override void OnStop()
        {
            process_manager.KillAll();
        }
    }
}
