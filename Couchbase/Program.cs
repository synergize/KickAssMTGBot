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
                var test = Testing.ReadDocument("Hello", "MoversShakersServerStorage");
                int i = 0;
            }
            catch (Exception ex)
            {
                FileSystemManager fileSystem = new FileSystemManager("Logs");
                fileSystem.LogException(ex);
            }

            
        }
    }
}
