using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase.Helpers
{
    class SetupCouchbaseConnections
    {
        private const string username = "sysalis";
        private const string password = "QM#fy^9Hh$FH";
        private IBucket GetBucket(string bucketName)
        {
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
                        Password = password,
                        Username = username,
                        BucketName = bucketName
                    }}},
                UseSsl = false,
            };
            var cluster = new Cluster(config);
            var authenticator = new PasswordAuthenticator(username, password);
            cluster.Authenticate(authenticator);
            return cluster.OpenBucket(bucketName, password);
        }

        public Document<dynamic> CreateDocument()
        {
            var document = new Document<dynamic>
            {
                Id = "Hello",
                Content = new
                {
                    name = "Couchbase"
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
