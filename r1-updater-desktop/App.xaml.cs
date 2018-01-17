using r1_updater_desktop.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace r1_updater_desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running
            // Process command line args
            bool silent = false;
            bool update = false;
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "/silent")
                {
                    silent = true;
                }
                if (e.Args[i] == "/update")
                {
                    update = true;
                }
            }

            // Create main application window, starting minimized if specified
            MainWindow mainWindow = new MainWindow();
            if (silent)
            {
                Updater.Update();
            }
            else
            {
                mainWindow.Show();
                if (update)
                {
                    Updater.Update(exit:false);
                }
            }
        }
    }
}
