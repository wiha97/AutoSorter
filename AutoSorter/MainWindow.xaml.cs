using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AutoSorter.Backend;

namespace AutoSorter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logic log;
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            Feeder();
            log = new Logic();
            //log.GetProcName("notepad");
            log.GetDirName(srcBox.Text);
            log.FillList();
            Ticker();
            log.DirTimer();
            //Processes();
        }

        void Ticker()
        {
            try
            {
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(Timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 1);
                //timer.Start();
            }
            catch (Exception e)
            {

            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                //log.GetProcess("notepad");
                this.Dispatcher.Invoke(() =>
                {
                    if (all.IsChecked == true)
                    {
                        log.GetProcs();
                        Processes(log.AProcs);
                    }
                    if (log.GetProcess())
                    {
                        label.Content = string.Format("Saving to {0}", log.Proc);
                    }
                    else
                    {
                        label.Content = "No saved found, last process: " + log.Proc;
                    }
                });
            });
        }

        void Processes(List<string> procs)
        {
            ListView list = new ListView();
            ScrollViewer scroll = new ScrollViewer();
            stacky.Children.Clear();
            list.Background = new SolidColorBrush(Colors.DarkGray);
            list.ItemsSource = procs;
            list.MouseDoubleClick += new MouseButtonEventHandler(listDbl);
            stacky.Children.Add(list);
        }

        private void listDbl(object sender, EventArgs e)
        {
            ListView list = sender as ListView;
            procBox.Text = list.SelectedItem.ToString();
        }

        private List<string> Procs()
        {
            List<string> procs = new List<string>();
            foreach (Process proc in Process.GetProcesses().OrderBy(p => p.ProcessName))
            {
                procs.Add(proc.ProcessName);
            }
            return procs;
        }

        public void CheckProcess()
        {
            Logic log = new Logic();
            //log.CheckProcess("notepad");
        }

        public void Feeder()
        {
            srcBox.Text = File.ReadLines(@"config\path.txt").First();

        }

        private void procBtn_Click(object sender, RoutedEventArgs e)
        {
            log.AddToList(procBox.Text);
        }

        private void autoBox_Checked(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void autoBox_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void autoFile_Checked(object sender, RoutedEventArgs e)
        {
            log.Timer.Stop();
        }

        private void autoFile_Unchecked(object sender, RoutedEventArgs e)
        {
            log.Timer.Stop();
        }

        private void getProc_Click(object sender, RoutedEventArgs e)
        {
            log.GetProcName(procBox.Text);
            label.Content = "Saving to " + log.Proc;
        }

        private void saved_Checked(object sender, RoutedEventArgs e)
        {
            Processes(log.Procs);
        }
    }
}
