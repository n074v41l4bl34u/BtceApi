using System.Diagnostics.Contracts;
using System;
namespace BtcE
{
    public class ClientFactory : IClientFactory
    {
        private readonly string _exchangeHost;

        public ClientFactory()
        {
            _exchangeHost = null;
        }

        public ClientFactory(string exchangeHost)
        {
            _exchangeHost = exchangeHost;
        }

        public IBtceApiClient CreateClient(string key, string secret)
        {
          Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(key));
          Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(secret));

            return new BtceApiClientV3(key, secret, _exchangeHost);
        }

        public IBtceApiPublicClient CreatePublicClient()
        {
            return new BtceApiPublicClientV3();
        }
    }
}