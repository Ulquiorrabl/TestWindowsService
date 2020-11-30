using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace TestWindowsService
{
    public partial class TestService : ServiceBase
    {
        NumReturner returner;
        public TestService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            returner = new NumReturner();
            returner.Number += NumLog;
            returner.GiveNumber(125);
        }

        protected void NumLog(object sender, int num)
        {
            string path = @"C:\TestService";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = String.Format("{0}\\ServiceLog.txt", path);
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("Number Received: {0}", num);
            }
        }

    }
}
