using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestWindowsService
{
    public partial class TestService : ServiceBase
    {
        EventLog eventLog;
        NumReturner returner;
        private int eventId = 1;
        public TestService()
        {
            InitializeComponent();
            eventLog = new EventLog();
            returner = new NumReturner();
            returner.Number += OnNumReceived;
        }

        public void OnNumReceived(object sender, int num)
        {
            string message = "Number got" + num.ToString();
            eventLog.WriteEntry(message, EventLogEntryType.Information, eventId++);
        }

        protected override void OnStart(string[] args)
        {
            string message = "Service started";
            eventLog.WriteEntry(message, EventLogEntryType.Information, eventId++);
        }

        protected override void OnStop()
        {
            returner.GiveNumber(125);
        }
    }
}
