using System.Net;

namespace TestIDFApp;

public class ManagedProxy: IWebProxy
{
    private object proxyLock = new();
    private int _currentProxyIndex = 0;
    private readonly List<Uri> _proxies;

    public ICredentials Credentials { get; set; }

    public ManagedProxy()
    {
        var proxyA = new Uri("http://4.184.84.223:80");
        var proxyB = new Uri("http://51.103.74.105:80");

        _proxies = new List<Uri> {proxyA, proxyB};
    }
    
    public Uri? GetProxy(Uri destination)
    {
        lock (proxyLock) // May affect the performance
        {
            var proxy = _proxies.Count > 0 ? _proxies[_currentProxyIndex] : null;
            _currentProxyIndex = (_currentProxyIndex + 1) >= _proxies.Count ? 0 : _currentProxyIndex + 1;
            return proxy;
        }
    }

    public bool IsBypassed(Uri host)
    {
        // You can implement custom bypass logic here if needed.
        return false;
    }
    
}
