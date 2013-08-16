WcfServiceClientHelper
======================

Based on this SO post: [What is the best workaround for the WCF client `using` block issue?](http://stackoverflow.com/questions/573872/what-is-the-best-workaround-for-the-wcf-client-using-block-issue)


####Summary

Use it this way ...

```c#
string data = Service<ServiceClient>.Use(client => client.DoSomething());
```

Rather than ...

```c#
string data;
bool success = false;
ServiceClient client = null;
try
{
    client = new ServiceClient().GetProxy();
    data = client.DoSomething();
    client.Close();
    success = true;
}
finally
{
    if (!success && client != null)
    {
        client.Abort();
    }
}
```


####Explanation of Service.Use() method

As per MSDN guidelines, WCF client should not be used in a `using` block.

```c#
// wrong
using(var client = new WcfClient())
{
	var result = client.DoSomething();
}
```

Instead, it should be closed or aborted. 

```c#
// right
WcfClient client = null;
bool success = false;
try
{
    client = new WcfClient();
    var result = client.DoSomething();
    client.Close();
    success = true;
	return result;
}
finally
{
    if (!success && client != null)
	{
        client.Abort();
	}
}
```

Since this is ugly at every call site, the boiler plate code can be wrapped into a helper method. 

```c#
// helper method with just TClient
public static class Service<TClient>
    where TClient : class, ICommunicationObject, new()
{
    public static TReturn Use<TReturn>(Func<TClient, TReturn> work)
    {
		TClient client = default(TClient);
		bool success = false;
        try
		{
            client = new TClient();
            TReturn result = work(client);
            client.Close();
            success = true;
            return result;
        }
		finally
		{
			if (!success && client != null)
			{
				client.Abort();
			}
		}
    }
}

// used as
var result = Service<ServiceClient>.Use(client => client.DoSomething());
```

Sometimes, there is a need to set something in WcfClient before using it (client credentials, etc.).

```c#
interface IWcfClientProxyFactory<TClient>
{
	TClient GetProxy();
}
public class ServiceProxy : IWcfClientProxyFactory<ServiceClient>
{
	public ServiceClient GetProxy()
	{
		var client = new ServiceClient();
		client.ClientCredentials = ... ;
		return client;
	}
}
```

We change the helper class to include `TProxy`. 

```c#
// helper method with TClient and TProxy
public static class Service<TClient, TProxy>
    where TClient : class, ICommunicationObject
    where TProxy : IWcfClientProxyFactory<TClient>, new()
{
    public static TReturn Use<TReturn>(Func<TClient, TReturn> codeBlock)
    {
        TClient client = default(TClient);
        bool success = false;
        try
        {
            client = new TProxy().GetProxy();
            TReturn result = codeBlock(client);
            client.Close();
            success = true;
            return result;
        }
        finally
        {
            if (!success && client != null)
            {
                client.Abort();
            }
        }
    }
}

// used as
var result = Service<ServiceClient, ServiceProxy>.Use(client => client.DoSomething());
```

But type `TProxy` can be removed.

```c#
// same
interface IWcfClientProxyFactory<TClient>
{
	TClient GetProxy();
}

// same client class with partial
public partial class ServiceClient : IWcfClientProxyFactory<ServiceClient>
{
	public ServiceClient GetProxy()
	{
		ServiceClient client = this;
		client.ClientCredentials = ... ;
		return client;
	}
}

// helper method with TClient and TProxy removed
public static class Service<TClient>
	where TClient : class, ICommunicationObject, IWcfClientProxyFactory<TClient>, new()
{
    public static TReturn Use(Func<TClient, TReturn> work)
    {
		TClient client = default(TClient);
		bool success = false;
        try
		{
            client = new TClient().GetProxy();
            TReturn result = work(client);
            client.Close();
            success = true;
            return result;
        }
		finally
		{
			if (!success && client != null)
			{
				client.Abort();
			}
		}
    }
}

// used as
var result = Service<ServiceClient>.Use(client => client.DoSomething());
```
