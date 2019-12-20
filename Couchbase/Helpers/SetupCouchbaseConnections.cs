using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase.Helpers
{
    class SetupCouchbaseConnections
    {
        private const string username = "sysalis";
        private const string password = "QM#fy^9Hh$FH";
        private Cluster CreateCluster()
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://192.168.1.172") }
            });

            var authenticator = new PasswordAuthenticator(username, password);
            cluster.Authenticate(authenticator);
            cluster.OpenBucket("MoversShakersServerStorage", password);

            return cluster;
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

        public void ReadDocument(Document<dynamic> document)
        {
            var cluster = CreateCluster();
            var bucket = cluster.OpenBucket();
            var upsert = bucket.Upsert(document);
            if (upsert.Success)
            {
                var get = bucket.GetDocument<dynamic>(document.Id);
                document = get.Document;
                var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
                Console.WriteLine(msg);
            }
        }

    }
}
