// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using MqttClientTest;
using Serilog;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var testConfig = config.GetSection("TestConfig").Get<TestConfig>();




LoggerConfiguration loggerConfiguration = new LoggerConfiguration();

if (testConfig.LogType == 1)
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File("app.log")
        .CreateLogger();
}
else
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();
}
Log.Information("Ah, there you are!");

Log.Information("开始....");
for (int i = 0; i < testConfig.Thread * testConfig.ConnPerThread; i++)
{
    Thread thread = new Thread(()=>MqttTools.Test(testConfig, i.ToString()));
    thread.Start();
}

while (true)
{
    Log.Information($"当前订阅总数:{Total.TOTAL_SUB}, 已接收消息:{Total.TOTAL_MSG}, 已发送消息:{Total.TOTAL_SEND}");
    Thread.Sleep(2000);
    if (Total.TOTAL_SUB < testConfig.Thread * testConfig.ConnPerThread)
    {
        var xian = testConfig.Thread * testConfig.ConnPerThread - Total.TOTAL_SUB;
        Log.Information($"差{xian.ToString()}");
        for (int i = 0; i <xian ; i++)
        {
            Thread thread = new Thread(()=>MqttTools.Test(testConfig, Guid.NewGuid().ToString("N")));
            thread.Start();
            
            Log.Information(i.ToString());
        }
        Thread.Sleep(5000);

    }
}
