using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MQTT_BrokerServer
{
    class Program
    {
        public static IMqttServer mqttServer;
        static int port = 1883;
        /// <summary>
        /// 參數說明: args[0]:port號
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            LoadArgs(args);
            StartMqttServer();
        }
        /// <summary>
        /// 抓取參數 args[0] port
        /// </summary>
        /// <param name="args"></param>
        public static void LoadArgs(string[] args)
        {
            if (args == null)
                return;
            if (args.Length == 0)
            {
                return;
            }
            port = int.Parse(args[0]);
        }

        //啟動Mqtt服務器
        private static async void StartMqttServer()
        {
            try
            {
                //驗證客戶端資訊
                var options = new MqttServerOptions
                {
                    //連接驗證
                    ConnectionValidator = new MqttServerConnectionValidatorDelegate(p =>
                    {
                        if (p.ClientId == "SpecialClient")
                        {
                            if (p.Username != "USER" || p.Password != "PASS")
                            {
                                p.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            }
                        }
                    })
                };
                //設定埠號
                options.DefaultEndpointOptions.Port = port;

                //創建Mqtt服務器
                mqttServer = new MqttFactory().CreateMqttServer();

                //開啟訂閱事件
                mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(
                    e =>
                    {
                        //客戶端Id
                        var ClientId = e.ClientId;
                        var Topic = e.TopicFilter.Topic;
                        Console.WriteLine($"客戶端[{ClientId}]已訂閱主題：{Topic}");
                    }
               );

                //取消訂閱事件
                mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(
                    e =>
                    {
                        //客戶端Id
                        var ClientId = e.ClientId;
                        var Topic = e.TopicFilter;
                        Console.WriteLine($"客戶端[{ClientId}]已取消訂閱主題：{Topic}");
                    }
                    );

                //客戶端訊息事件
                mqttServer.UseApplicationMessageReceivedHandler(
                   e =>
                   {
                       var ClientId = e.ClientId;
                       var Topic = e.ApplicationMessage.Topic;
                       var Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                       var Qos = e.ApplicationMessage.QualityOfServiceLevel;
                       var Retain = e.ApplicationMessage.Retain;
                       Console.WriteLine($"客戶端[{ClientId}]>> 主題：[{Topic}] 負載：[{Payload}] Qos：[{Qos}] 保留：[{Retain}]");
                   }
                );

                //客戶端連接事件
                mqttServer.UseClientConnectedHandler(
                    e =>
                    {
                        var ClientId = e.ClientId;
                        Console.WriteLine($"客戶端[{ClientId}]已連接");
                    }
                );

                ////客戶端斷開事件
                mqttServer.UseClientDisconnectedHandler(e =>
                {
                    var ClientId = e.ClientId;
                    Console.WriteLine($"客戶端[{ClientId}]已斷開連接");
                });

                //啟動服務器
                await mqttServer.StartAsync(options);

                Console.WriteLine("服務啟動成功！輸入任意按鈕停止服務！");
                Console.ReadLine();

                await mqttServer.StopAsync();
            }
            catch (Exception e)
            {
                Console.Write($"服務器啟動失敗 Msg：{e}");
            }

        }
    }
}
