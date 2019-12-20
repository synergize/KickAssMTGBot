using Couchbase.Helpers;
using System;

namespace Couchbase
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupCouchbaseConnections Testing = new SetupCouchbaseConnections();
            Testing.ReadDocument(Testing.CreateDocument());
            
        }
    }
}
