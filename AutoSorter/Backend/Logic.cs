using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Threading;
using System.IO;

namespace AutoSorter.Backend
{
    public class Logic
    {
        private string proc;
        private string dir;
        private List<string> procs;
        private List<string> aProcs;
        private DispatcherTimer timer;

        public string Proc
        {
            get { return proc; }
            set { proc = value; }
        }
        public string Dir
        {
            get { return dir; }
            set { dir = value; }
        }
        public void GetProcName(string name)
        {
            proc = name;
        }
        public void GetDirName(string dName)
        {
            dir = dName;
        }
        public List<string> Procs
        {
            get { return procs; }
            set { procs = value; }
        }
        public DispatcherTimer Timer
        {
            get { return timer; }
        }
        public void FillList()
        {
            procs = File.ReadAllLines(@"config\apps.txt").ToList();
        }
        public void AddToList(string app)
        {
            procs.Add(app);
            File.AppendAllText(@"config\apps.txt", app);
        }
        public List<string> AProcs
        {
            get { return aProcs; }
            set { aProcs = value; }
        }
        public void GetProcs()
        {
            aProcs = new List<string>();
            //aProcs = Process.GetProcesses().ToList();
            foreach(Process process in Process.GetProcesses().OrderBy(p => p.ProcessName))
            {
                aProcs.Add(process.ProcessName);
            }
        }
        public bool GetProcess()
        {
            bool bl = false;
            try
            {
                //string procName = "";
                //var processes = Process.GetProcesses();
                //foreach (Process process in processes)
                //{
                //    procName = process.ProcessName;
                //    if ((procName == proc) || (File.ReadAllLines(@"config\apps.txt").ToList().Contains(procName)))
                //    {
                //        proc = procName;
                //        bl = true;
                //        break;
                //    }
                //}
                foreach (string app in procs)
                {
                    if (Process.GetProcessesByName(app).Length > 0)
                    {
                        proc = app;
                        bl = true;
                        break;
                    }
                }
            }
            catch(Exception e)
            {

            }
            return bl;
        }
        public void DirTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TickTick);
            timer.Interval = new TimeSpan(0, 0, 5);
            //timer.Start();
        }

        async void TickTick(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                CheckDir();
            });
        }

        public void CheckDir()
        {
            try
            {
                string[] files = Directory.GetFiles(dir);
                if (files.Count() > 0)
                {
                    if (!Directory.Exists(dir + @"\" + proc))
                    {
                        Directory.CreateDirectory(dir + @"\" + proc);
                    }
                    foreach (string file in files)
                    {
                        File.Move(file, dir + @"\" + proc + @"\" + Path.GetFileName(file));
                    }
                }
            }
            catch(Exception e)
            {

            }
        }

    }
}
