namespace WcfClientHelper
{
    public interface IWcfClientProxyFactory<TClient>
    {
        /// <summary>
        /// Do something with client before using it.
        /// </summary>
        TClient GetProxy();
    }
}
