using System.Text;
using MQTTnet;
using MQTTnet.Client;
using Serilog;

namespace MqttClientTest;

public class MqttTools
{

    public static async Task Test(TestConfig testConfig,string clientId)
    {
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(testConfig.Ip, testConfig.Port)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(testConfig.KeepAlive))
            .WithTimeout(TimeSpan.FromSeconds(testConfig.ConnectionTimeout))
            .WithCredentials(testConfig.UserName,testConfig.Password)
            .Build();
        await mqttClient.ConnectAsync(options);
        if (mqttClient.IsConnected)
        {
            Log.Information($"{clientId} 已连接");
            Total.AddTotalSub();
        }
        else
        {
            Thread.Sleep(testConfig.ReconnectDelay * 1000);
            await mqttClient.ReconnectAsync();
        }
        mqttClient.ConnectedAsync += MqttClientOnConnectedAsync;
        

        await mqttClient.SubscribeAsync($"{clientId}|sub");
        var message = new MqttApplicationMessageBuilder()
            .WithTopic($"{clientId}|sub")
            .WithPayload("testPayload")
            .Build();
        mqttClient.ApplicationMessageReceivedAsync += MqttClientOnApplicationMessageReceivedAsync;
        mqttClient.DisconnectedAsync += MqttClientOnDisconnectedAsync;
        try
        {

            while (true)
            {
                await mqttClient.PublishAsync(message);
                Total.AddTotalSend();
                Thread.Sleep(testConfig.SendMessageDelay*1000);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    private static async Task MqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        Total.AddTotalMsg();
    }

    private static async Task MqttClientOnDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
    {
        Total.RemoveTotalSub();
    }

    private static async Task MqttClientOnConnectedAsync(MqttClientConnectedEventArgs arg)
    {
      Total.AddTotalSub();
    }
}