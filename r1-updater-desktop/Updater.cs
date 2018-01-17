using r1_updater_desktop.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace r1_updater_desktop
{
    public static class Updater
    {
        private static bool autoexit = true;
        private static Dumper dumper;
        private static List<string> argsList = new List<string>()
        {
            "ds",
            "dbname",
            "target",
            "outuser",
            "outpass"
        };
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        public static void Update(bool exit = true)
        {
            autoexit = exit;
            //parseArgs(args);
            LoadSettings();
            initDumper();
            string data = dumper.getData(
                dbName: Properties.Settings.Default.dbname,
                limit: Properties.Settings.Default.limit
                );
            //Updater.outprint(data);
            Sender.setup(
              target: getOption("target"),
              username: getOption("outuser"),
              password: getOption("outpass")
            );
            Task.Run(async () =>
            {
                await Sender.sendAsync(data);
            }).GetAwaiter().GetResult();
            if (autoexit) Updater.exit(0);

        }

        private static void LoadSettings()
        {
            config.Clear();
            config.Add("ds", Properties.Settings.Default.ds);
            config.Add("dbname", Properties.Settings.Default.dbname);
            config.Add("target", Properties.Settings.Default.target);
            config.Add("outuser", Properties.Settings.Default.outuser);
            config.Add("outpass", Properties.Settings.Default.outpass);
        }

        private static void parseArgs(string[] args)
        {
            if (args.Length % 2 != 0)
            {
                exit(1488);
            }

            for (int i = 0; i < args.Length; i = i + 2)
            {
                if (args.Length == 0) break;
                if (!args[i][0].Equals('-'))
                {
                    Updater.exit(1488);
                };
                string arg = args[i];
                arg = arg.Substring(1);
                string value = args[i + 1].Replace("\"", string.Empty).Trim();
                if (string.IsNullOrEmpty(arg) || string.IsNullOrEmpty(value))
                {
                    exit(1488);
                    break;
                }
                if (argsList.Contains(arg))
                {
                    config.Add(arg, value);
                }
                else
                {
                    Updater.outprint(arg);
                    exit(1488);
                    break;
                }


            }
        }
        private static string getOption(string key)
        {
            string result;
            config.TryGetValue(key, out result);
            return result;
        }

        private static void initDumper()
        {
            dumper = new Dumper(
              ds: getOption("ds"),
              dbname: getOption("dbname")
            );

            //dumper.connect();
        }

        public static void exit(int code = 0, string lastMessage = "")
        {
            switch (code)
            {
                case 1488:
                    {
                        outprint("Invalid usage. Valid options:");
                        outprint("r1-updater.exe -ds [data source (default localhost\\sqlexpress)] " +
                            "-dbname [database name] -target [http target (default localhost\\sqlexpress)] " +
                            "-outuser [http username (default admin)] -outpass (default qwe123)");

                        break;
                    }
                case 1490:
                    {
                        outprint("Connection error.");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(lastMessage)) outprint(lastMessage);
            if (autoexit)
            {
                Environment.Exit(code);
            }
        }
        public static void outprint (string message)
        {
            if (autoexit)
            {
                Console.WriteLine(message);
            } else
            {
                MessageBox.Show(message);
            }
        }
    }
}