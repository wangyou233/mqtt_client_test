namespace MqttClientTest;

public class Total
{
    public static int TOTAL_SUB = 0;
    public static int TOTAL_MSG = 0;
    public static int TOTAL_SEND = 0;

    public static void AddTotalSub()
    {
        object o = new object();
        lock (o)
        {
            TOTAL_SUB++;
        }
    }

    public static void AddTotalSend()
    {
        object o = new object();
        lock (o)
        {
            TOTAL_SEND++;
        }
    }

    public static void AddTotalMsg()
    {
        object o = new object();
        lock (o)
        {
            TOTAL_MSG++;
        }
    }

    public static void RemoveTotalSub()
    {
        object o = new object();
        lock (o)
        {
            TOTAL_SUB--;
        }
    }
}