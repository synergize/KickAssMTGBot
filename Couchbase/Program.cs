using Couchbase.Helpers;
using System;
using VTFileSystemManagement;

namespace Couchbase
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SetupCouchbaseConnections Testing = new SetupCouchbaseConnections();
                Testing.ReadDocument(Testing.CreateDocument());
            }
            catch (Exception e)
            {
                FileSystemManager fileSystem = new FileSystemManager("Logs");
                fileSystem.LogException(e);
                throw;
            }

            
        }
    }
}
