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
            //讀取應用程式參數,假如有的話
            LoadArgs(args);
            Console.WriteLine("5秒後開始 訂閱MQTT Boker伺服器");
            Thread.Sleep(5000);
            // 建立一個新的MQTT Client端連線
            MqttFactory factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MQTT_SERVER_IP, port) // Port is optional
            .Build();
            //設定事件,接收到訂閱訊息之後要做的事情
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
            //連線到伺服器
            mqttClient.ConnectAsync(options);
            Console.WriteLine("5秒後開始 訂閱主題:" + topic);
            Thread.Sleep(5000);
            //開始訂閱主題
            mqttClient.SubscribeAsync(topic);
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// 讀取應用程式參數,假如有的話 
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
    }
}
