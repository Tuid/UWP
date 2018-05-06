using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Data.Json;
using System.Net;
using System.Text;
using System.Diagnostics;
using Windows.Web.Http;
using Newtonsoft.Json;
using API.Models;
using Windows.Data.Xml.Dom;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace API
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private string location = "";
        private string ip = "";

        private async void getWeatherXMLButtonClick(object sender, RoutedEventArgs e)
        {
               location = selectTextBox.Text;
               string url = " http://v.juhe.cn/weather/index?cityname="+location+"&dtype=xml&format=2&key=8f9462857e209325b1ffc655525ce3a5";
                 Uri uri = new Uri(url);
                 HttpClient httpClient = new HttpClient();
                 HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                 string httpResponseBody ="" ;

                 try
                 {
                     httpResponseMessage = await httpClient.GetAsync(uri);
                     httpResponseMessage.EnsureSuccessStatusCode();
                     httpResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                 }
                 catch ( Exception exception)
                 {
                     httpResponseBody = "ERROR " + exception.HResult.ToString("X") + "Message " + exception.Message;
                 }

                 XmlDocument xmlDocument = new XmlDocument();
                 xmlDocument.LoadXml(httpResponseBody);
                 XmlNodeList list = xmlDocument.GetElementsByTagName("weather");
                 IXmlNode node = list.Item(0);
                 resultWeatherXML.Text = node.InnerText;

        }

        private async void getWeatherJSONButtonClick(object sender, RoutedEventArgs e)
        {
            location = selectTextBox.Text;
            string url = "http://v.juhe.cn/weather/index?cityname="+location+"&type=json&format=2&key=8f9462857e209325b1ffc655525ce3a5";
            Uri uri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                httpResponseMessage = await httpClient.GetAsync(uri);
                httpResponseMessage.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception exception)
            {
                httpResponseBody = "ERROR " + exception.HResult.ToString("X") + "Message " + exception.Message;
            }
            JsonObject jsonObject = new JsonObject();
            jsonObject =  JsonObject.Parse(httpResponseBody);
            JsonReader jsonReader = new JsonTextReader(new StringReader(httpResponseBody));
           while (jsonReader.Read())
            {
             //   Console.WriteLine(jsonReader.TokenType + "\t\t" + jsonReader.ValueType + "\t\t" + jsonReader.Value);
                if (jsonReader.Path == "result.today.weather")
                {
                    resultWeatherJSON.Text = jsonReader.Value.ToString();
                }
            }

            
           
        }


        private async void getIPCpuntryButtonClick(object sender, RoutedEventArgs e)
        {
            ip = selectIPTextBox.Text;
            string url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
            Uri uri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                httpResponseMessage = await httpClient.GetAsync(uri);
                httpResponseMessage.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception exception)
            {
                httpResponseBody = "ERROR " + exception.HResult.ToString("X") + "Message " + exception.Message;
            }

            JsonReader jsonReader = new JsonTextReader(new StringReader(httpResponseBody));
            while (jsonReader.Read())
            {
                //   Console.WriteLine(jsonReader.TokenType + "\t\t" + jsonReader.ValueType + "\t\t" + jsonReader.Value);
                if (jsonReader.Path == "data.country")
                {
                    resultIP.Text = jsonReader.Value.ToString();
                }
            }
        }
    }
}