using CoreFire;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BitGifter.Core.Tests
{
    class FireBaseUsageTests
    {
        [Fact]
        public void ShouldCreateRecord()
        {
            var uri = new Uri("https://helloworld-42239.firebaseio.com/");
            var client = Client.Builder()
                .WithUri(uri)
                .Build();

            client.PushSync("/now", DateTime.UtcNow);
        }
    }
}
