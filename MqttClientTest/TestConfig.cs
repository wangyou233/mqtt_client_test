namespace MqttClientTest;

public struct TestConfig
{
    public string Ip { get; set; }
    
    public int Port { get; set; }
    
    public float Thread { get; set; }
    
    public int ConnPerThread { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public int ConnectionTimeout { get; set; }
    
    public int SendMessageDelay { get; set; }
    
    public int ReconnectDelay { get; set; }
    public int KeepAlive { get; set; }
    
    public float LogType { get; set; }

}