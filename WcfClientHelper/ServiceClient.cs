// note: namespace has to be same as generated ServiceClient's namespace.
namespace WcfClientHelper.WcfServiceReference
{
    public partial class ServiceClient : IWcfClientProxyFactory<ServiceClient>
    {
        public ServiceClient GetProxy()
        {
            var client = this;
            // do somthing here as set client credentials, etc.
            //client.ClientCredentials = ... ;
            return client;
        }
    }
}
