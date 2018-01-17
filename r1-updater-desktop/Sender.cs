using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace r1_updater_desktop
{

    public static class Sender
    {

        private static readonly HttpClient client = new HttpClient();
        private static string _target { get; set; }
        private static string _username { get; set; }
        private static string _password { get; set; }

        public static async System.Threading.Tasks.Task sendAsync(string sqldata)
        {
            var data = new Dictionary<string, string>{
                { "data", sqldata },
            };
            try
            {
                var content = new FormUrlEncodedContent(data);
                var byteArray = Encoding.ASCII.GetBytes(_username + ":" + _password);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var response = await client.PostAsync(_target, content);

                var responseString = await response.Content.ReadAsStringAsync();
                var result = responseString == "done.";
                //Updater.outprint(responseString);
                if (!result)
                {
                    Updater.exit(1490, responseString);
                } else
                {
                    Updater.outprint("Successfully updated.");
                }
            }
            catch (Exception ex)
            {
                Updater.exit(1491, ex.Message);
            }
        }
        public static void setup(string target, string username, string password)
        {
            _target = string.IsNullOrWhiteSpace(target) ? "http://r1sport.by/bot/r1-updater.php" : target;
            _username = string.IsNullOrWhiteSpace(username) ? "admin" : username;
            _password = string.IsNullOrWhiteSpace(password) ? "qwe123" : password;

        }
    }
}
