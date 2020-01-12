using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using System;
using System.Collections.Generic;

namespace Couchbase.Helpers
{
    class SetupCouchbaseConnections
    {
        private IBucket GetBucket(string bucketName)
        {
            CouchbaseCredentialsModel loginInfo = new CouchbaseCredentialsModel("sysalis", "QM#fy^9Hh$FH");
            var config = new ClientConfiguration
            {
                // assign one or more Couchbase Server URIs available for bootstrap
                Servers = new List<Uri>
                {
                    new Uri("http://192.168.1.172:8091/")
                },
                BucketConfigs = new Dictionary<string, BucketConfiguration> {
                    {"Couchbase", new BucketConfiguration {
                        PoolConfiguration = new PoolConfiguration {
                            MaxSize = 6,
                            MinSize = 4,
                            SendTimeout = 12000
                        },
                        Port = 8091,
                        DefaultOperationLifespan = 123,
                        Password = loginInfo.Password,
                        Username = loginInfo.Username,
                        BucketName = bucketName
                    }}},
                UseSsl = false,
            };
            var cluster = new Cluster(config);
            var authenticator = new PasswordAuthenticator(loginInfo.Username, loginInfo.Password);
            cluster.Authenticate(authenticator);
            return cluster.OpenBucket(bucketName, loginInfo.Password);
        }

        public Document<dynamic> CreateDocument()
        {
            var document = new Document<dynamic>
            {
                Id = "Hello Part 2",
                Content = new
                {
                    name = "Couchbase"
                }
            };

            return document;
        }

        public Document<dynamic> CreateDiscordChannelDocument(ulong discordServerId, ulong discordchannelId)
        {
            var document = new Document<dynamic>
            {
                Id = "Movers And Shakers Server Information",
                Content = new
                {
                    ServerID = discordServerId,
                    ChannelID = discordchannelId
                }
            };

            return document;
        }

        public void WriteDocument(Document<dynamic> document, string bucketName)
        {
            var bucket = GetBucket(bucketName);
            var upsert = bucket.Upsert(document);
            if (upsert.Success)
            {
                var get = bucket.GetDocument<dynamic>(document.Id);
                document = get.Document;
            }
        }

        public Document<dynamic> ReadDocument(string docuumentId, string bucketName)
        {
            var bucket = GetBucket(bucketName);
            var get = bucket.GetDocument<dynamic>(docuumentId);

            return get.Document;
        }

    }
}
