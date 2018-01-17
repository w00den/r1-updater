using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace r1_updater_desktop
{
    public class Dumper
    {
        protected string dataSource { get; set; }
        protected string dbname { get; set; }
        protected SqlConnection cnn { get; set; }

        public Dumper(string ds, string dbname)
        {
            if (string.IsNullOrEmpty(ds))
            {
                ds = "localhost\\sqlexpress";
            }
            this.dataSource = ds;
            this.dbname = dbname;
        }

        public string getData(string dbName = "pizda", int limit = 0)
        {
            string connetionString = "Data Source=" + dataSource + ";Initial Catalog=" + dbname + ";Integrated Security=True;";
            cnn = new SqlConnection(connetionString);

            string result = "";
            string sLimit = limit > 0 ? "TOP " + limit : "";
            string oString = @"select " + sLimit + @" [OID], [Name], [LastName], [MiddleName], [LastVisitDateTime], [TelefoneCell], [Account].[Summ], [SaledService].[Count] as [visits_left]
from [" + dbname + @"].[dbo].[MyPerson]
left join [" + dbname + @"].[dbo].[Account] on [" + dbname + @"].[dbo].[MyPerson].[OID] = [" + dbname + @"].[dbo].[Account].[Client]
	AND [" + dbname + @"].[dbo].[Account].[Type_ID] = 1
left join [" + dbname + @"].[dbo].[SaledService] on [" + dbname + @"].[dbo].[MyPerson].[OID] = [" + dbname + @"].[dbo].[SaledService].[Client_ID]
	AND [" + dbname + @"].[dbo].[SaledService].[Status_ID] = 2";

            SqlCommand oCmd = new SqlCommand(oString, cnn);
            //oCmd.Parameters.AddWithValue("@Fname", fName);
            try
            {
                cnn.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        result = result +
                            oReader["OID"].ToString() + "|" +
                            oReader["Name"].ToString() + "|" +
                            oReader["LastName"].ToString() + "|" +
                            oReader["MiddleName"].ToString() + "|" +
                            oReader["LastVisitDateTime"].ToString() + "|" +
                            oReader["TelefoneCell"].ToString() + "|" +
                            oReader["Summ"].ToString() + "|" +
                            oReader["visits_left"].ToString() +"\n";
                    }
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                Updater.exit(1489, ex.Message);
            }
            return result;
        }

        public void connect()
        {
            string connetionString = "Data Source=" + dataSource + ";Initial Catalog=" + dbname + ";Integrated Security=True;";
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                //Updater.outprint("Connection Open");
                cnn.Close();

            }
            catch (Exception ex)
            {
                Updater.outprint("Can not open connection");
                Updater.exit(1489, ex.Message);
            }
        }



    }
}
