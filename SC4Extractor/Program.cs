using System;

using SC4Parser;
using SC4Parser.Subfiles;
using SC4Parser.Files;
using SC4Parser.DataStructures;

namespace SC4Extractor // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (SC4SaveFile file = new(@"C:\Users\Shadowfax\Downloads\City_-_Leptonm.sc4"))
            {
                IndexEntry networkIndex = file.FindIndexEntryWithType(Constants.NETWORK_INDEX_SUBFILE_TYPE); // TODO Add load with type to parser
                byte[] indexFileData = file.LoadIndexEntry(networkIndex);

                NetworkIndex networkFile = new();
                networkFile.Parse(indexFileData);
                networkFile.Dump();
                
            }
        }
    }
}