using ASquare.WindowsTaskScheduler;
using ASquare.WindowsTaskScheduler.Models;
using r1_updater_desktop.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace r1_updater_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Settings settings = new Settings();
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
  
        }
        protected void LoadSettings()
        {
            this._ds.Text = Properties.Settings.Default.ds;
            this._dbname.Text = Properties.Settings.Default.dbname;
            this._limit.Text = Properties.Settings.Default.limit.ToString();
            this._target.Text = Properties.Settings.Default.target;
            this._outuser.Text = Properties.Settings.Default.outuser;
            this._outpass.Text = Properties.Settings.Default.outpass;
            this._autorun.IsChecked = Properties.Settings.Default.autorun;
            this._runevery.Text = Properties.Settings.Default.runevery.ToString();
        }

        protected void SaveSettings()
        {
            Properties.Settings.Default.ds = string.IsNullOrEmpty(this._ds.Text) ? "localhost\\sqlexpress" : this._ds.Text;
            Properties.Settings.Default.dbname = string.IsNullOrEmpty(this._dbname.Text) ? "Devpark.Fitness.r2" : this._dbname.Text;
            Properties.Settings.Default.limit = string.IsNullOrEmpty(this._limit.Text) ? 0 : int.Parse(this._limit.Text);
            Properties.Settings.Default.target = string.IsNullOrEmpty(this._target.Text) ? "http://r1sport.by/bot/r1-updater.php" : this._target.Text;
            Properties.Settings.Default.outuser = string.IsNullOrEmpty(this._outuser.Text) ? "admin" : this._outuser.Text;
            Properties.Settings.Default.outpass = string.IsNullOrEmpty(this._outpass.Text) ? "qwe123" : this._outpass.Text;
            Properties.Settings.Default.autorun = this._autorun.IsChecked.Value;
            Properties.Settings.Default.runevery = string.IsNullOrEmpty(this._runevery.Text) ? 5 : int.Parse(this._runevery.Text);
            Properties.Settings.Default.Save();
            LoadSettings();
        }

        private void _saveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            if (Properties.Settings.Default.autorun)
            {
                setSchedule();
            } else
            {
                deleteSchedule();
            }
        }
        private void deleteSchedule()
        {
            WindowTaskScheduler
                .Configure().DeleteTask("r1-updater");

        }
        private void setSchedule()
        {
            string applicationPath = System.Reflection.Assembly.GetEntryAssembly().Location + " /silent /update";


            SchedulerResponse response = WindowTaskScheduler
            .Configure()
            .CreateTask("r1-updater", applicationPath)
            .RunDaily()
            .RunEveryXMinutes(Properties.Settings.Default.runevery*60)
            .RunDurationFor(new TimeSpan(18, 0, 0))
            .SetStartDate(new DateTime(2015, 8, 8))
            .SetStartTime(new TimeSpan(8, 0, 0))
            .Execute();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Updater.Update(exit:false);
        }

        private void _runevery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                e.Handled = true;
            }
        }
    }
}
