namespace TestIDFApp;

public class Helper
{
    private static readonly List<string> _proxyList = new (){"4.184.84.223:80", "51.103.74.105:80"};

    public static string SelectProxy()
    {
        var random = new Random();
        var index = random.Next(_proxyList.Count);
        return _proxyList[index];
    }
}