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

namespace MQTT_Publish
{
    class Program
    {

        /// <summary>
        /// 走的port號
        /// </summary>
        static int port = 1883;

        /// <summary>
        /// 要轉發布的主題
        /// </summary>
        static string topic = "MyTopic";

        /// <summary>
        /// 訂閱的伺服器IP
        /// </summary>
        static string MQTT_SERVER_IP = "localhost";

        /// <summary>
        /// 主程式
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("5秒後開始 連線到MQTT Boker伺服器");
            Thread.Sleep(5000);

            // Create a new MQTT client.
            MqttFactory factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MQTT_SERVER_IP, port) // Port is optional
            .Build();

            var t = mqttClient.ConnectAsync(options).Result;
            if (MQTT_SERVER_IP != "")
            {
                Console.WriteLine("連線伺服器為:" + MQTT_SERVER_IP);
            }
            //
            string inputLine = "";
            while (inputLine != "exit")
            {
                Console.WriteLine("請輸入要發佈的訊息(若要中斷操作,請輸入exit)");
                inputLine = Console.ReadLine();
                //建立發佈參數
                var msg = new MqttApplicationMessageBuilder()
                    //主題 
                    .WithTopic(topic)
                     //內文
                     .WithPayload(inputLine)
                     //Qos:1,AtLeastOnce
                     .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                     //不保留訊息
                     .WithRetainFlag(false)
                     .Build();
                mqttClient.PublishAsync(msg);
            }

            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");


        }
    }
}