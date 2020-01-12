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
                Testing.WriteDocument(Testing.CreateDocument(), "MoversShakersServerStorage");
            }
            catch (Exception ex)
            {
                FileSystemManager fileSystem = new FileSystemManager("Logs");
                fileSystem.LogException(ex);
            }

            
        }
    }
}
