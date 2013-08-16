using System;
using System.ServiceModel;

namespace WcfClientHelper
{
    /// <summary>
    /// Helper class that wraps the codeblock in try-catch and closing the TClient on success and aborting the client on error.
    /// </summary>
    /// <typeparam name="TClient">WcfClient</typeparam>
    /// <remarks>http://stackoverflow.com/questions/573872/what-is-the-best-workaround-for-the-wcf-client-using-block-issue</remarks>
    /// <example>
    /// string data = Service<ServiceClient>.Use(client => client.GetData(7));
    /// </example>
    public static class Service<TClient>
        where TClient : class, ICommunicationObject, IWcfClientProxyFactory<TClient>, new()
    {
        public static TReturn Use<TReturn>(Func<TClient, TReturn> codeBlock)
        {
            TClient client = default(TClient);
            bool success = false;
            try
            {
                client = new TClient().GetProxy();
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
}
