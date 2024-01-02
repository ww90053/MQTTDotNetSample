# MQTTDotNetSample
透過.Net 8 實作MQTT協定範例，運用套件MQTTnet3.0.14

# MQTTnet套件參考
## nuget: https://www.nuget.org/packages/MQTTnet
## github: https://github.com/dotnet/MQTTnet

# 開發環境
## 建議Visual Studio 2022以上
## .NetFramework版本:8，理論上可以因應開發需要降版，具體請可自行測驗看看

# 專案檔說明
## MQTT_BorkerServer
相當於中介的伺服器，用於拋轉訊息

## MQTT_Publish
發佈端，用於發送訊息

## MQTT_SubScribe
訂閱端，用於接收訊息
