using System;
using System.Collections.Generic;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics.Eventing.Reader;

namespace Chat.gpt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        string key = "sk-or-vv-62662940486a56355700ac4b32043a4e34a26fb4c36b6da2b72e61623fa2b1cb";
        string urlBase = "https://api.vsegpt.ru/v1/";

        HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", key);

            string promt = input.Text;

            List<dynamic> messages = new List<dynamic>();
            messages.Add(new { role = "user", content = promt });

            var requestData = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = messages,
                temperature = 0.7,
                n = 1,
                max_tokens = 3000
            };

            var jsonRequest = Newtonsoft
                .Json.JsonConvert.SerializeObject(requestData);

            var content = new StringContent(
                jsonRequest,
                Encoding.UTF8,
                "application/json"
                
            );

            var response = await client.PostAsync(
                urlBase + "chat/complections",
                content
                );

            string result;
            Console.WriteLine(response.StatusCode);
            if(response.IsSuccessStatusCode)
            {
                var jsonResponse =
                    await response.Content.ReadAsStringAsync();
                dynamic responseData =
                    Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                string responseContent = responseData.choices[0].message.content;
                result = responseContent;
            }
            else
            {
                result = "Ошибка: " + response.ReasonPhrase;
            }

            answer.Content = result;

            send.IsEnabled = true;
        }

        
    }
}
