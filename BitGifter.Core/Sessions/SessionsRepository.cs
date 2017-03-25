using System;
using System.Collections.Generic;
using System.Text;

namespace BitGifter.Core.Sessions
{
    class SessionsRepository
    {
        public string GetSession()
        {
            return BuildClient()
                .GetSync<string>("/sessions/first_customer");
        }

        public void SetSession(string value)
        {
            BuildClient()
                .SetSync("/sessions/first_customer", value);
        }

        private CoreFire.Client BuildClient()
        {
            var uri = new Uri("https://helloworld-42239.firebaseio.com/");
            var client = CoreFire.Client.Builder()
                .WithUri(uri)
                .Build();
            return client;
        }
    }
}
