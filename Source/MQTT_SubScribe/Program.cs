using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTT_SubScribe
{
    class Program
    {
        /// <summary>
        /// 走的port號
        /// </summary>
        static int port = 1883;

        /// <summary>
        /// 要轉訂閱的主題
        /// </summary>
        static string topic = "#";

        /// <summary>
        /// 訂閱的伺服器IP
        /// </summary>
        static string MQTT_SERVER_IP = "localhost";

        /// <summary>
        /// 參數 
        /// args[0] IP
        /// args[1] port
        /// EX:  MQTT_SubScribe.exe localhost 8883
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LoadArgs(args);

            Console.WriteLine("5秒後開始 訂閱MQTT Boker伺服器");
            Thread.Sleep(5000);
            // Create a new MQTT client.
            MqttFactory factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MQTT_SERVER_IP, port) // Port is optional
            .Build();
            
            mqttClient.UseApplicationMessageReceivedHandler(
              e =>
              {

                  string Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                  Console.WriteLine("### 接收到MQTT訊息 ###");
                  Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                  Console.WriteLine($"+ Payload = {Payload}");
                  Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                  Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
              }
                );

            mqttClient.ConnectAsync(options);

            Console.WriteLine("5秒後開始 訂閱主題:" + topic);
            Thread.Sleep(5000);
            mqttClient.SubscribeAsync(topic);


            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// 抓取參數 
        /// args[0] IP
        /// args[1] port
        /// </summary>
        /// <param name="args"></param>
        public static void LoadArgs(string[] args)
        {
            if (args == null)
                return;
            if (args.Length < 2)
            {
                return;
            }
            MQTT_SERVER_IP = args[0];
            port = int.Parse(args[1]);
        }
        /*
        public static async Task SendHttpRequest(string jsondata, string topic)
        {
            try
            {
                string targetUrl = HTTP_API_URL;
                //string parame = "p=Arvin";

                HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
                request.Method = "POST";
                //用json格式資料
                //request.ContentType = "application/json;charset=utf-8";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;

                //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
                NameValueCollection postParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
                postParams.Add("topic", topic);
                postParams.Add("jsondata", jsondata);
                postParams.Add("createdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                //參數列物件 轉成字串 再依照UTF8編碼轉成 byte Array
                byte[] postData = Encoding.UTF8.GetBytes(postParams.ToString());

                // 寫入 Post Body Message 資料流
                //using (Stream st = request.GetRequestStream())
                using (Stream st = await request.GetRequestStreamAsync())
                {
                    st.Write(postData, 0, postData.Length);
                }

                // 取得回應資料
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        string result = await sr.ReadToEndAsync();
                        Console.WriteLine($"topic: {topic} , 回傳訊息: {result} ");
                    }
                }
                //return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //return "error";
            }
        }
        */
    }
}
