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
using System.Reflection;

namespace TestWindowsService
{
    public partial class TestService : ServiceBase
    {
        private List<INumReturner> returnList;
        private readonly string directory = @"C:\TestService";
        public TestService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log("Service started!");
            returnList = new List<INumReturner>();
            LoadPlugin();
            for (int i = 0; i < returnList.Count(); i++)
            {
                returnList[i].Number += NumLog;
                returnList[i].GiveNumber(i);
            }
        }

        void Log(string message)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string filePath = String.Format("{0}\\ServiceLog.txt", directory);
            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine(message + " ==" + DateTime.Now.ToString("h:mm:ss tt") + "==");
            }
        }

        void NumLog(object sender, int num)
        {
            Log("Num returned " + num);
        }
        void LoadPlugin()
        {
            string path = Path.Combine(directory, "plugins");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var libraries = Directory.GetFiles(path);
            foreach (var library in libraries)
            {
                Assembly asm = Assembly.LoadFrom(library);
                var types = asm.GetTypes().
                           Where(t => t.GetInterfaces().
                           Where(i => i.FullName == typeof(INumReturner).FullName).Any());
                foreach (var type in types)
                {
                    var plugin = asm.CreateInstance(type.FullName) as INumReturner;
                    returnList.Add(plugin);
                }
            }
        }

    }
}
