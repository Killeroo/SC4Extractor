using System;
using System.Collections.Generic;

using SC4Parser;
using SC4Parser.Subfiles;
using SC4Parser.Files;
using SC4Parser.DataStructures;

using LitJson;

namespace SC4Extractor // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        enum Operation
        {
            List,
            Extract
        }

        enum Output
        {
            Console,
            File,
            Json
        }

        private static Dictionary<string , string> output = new Dictionary<string, string> ()
        {
            {"--extract", "Extracts an Index Entry from the file"},
            {"--list", "Lists all contents of the file"},
            {"--list-records", "Lists all compressed records in the save game"},
            {"--list-entries", "Lists all index entries (data) in the save game"},

            {"--logging", "Enables logging from the parsing library"},
            {"--json", "Outputs any results in a JSON encoded file"},
            {"--do-not-decompress", "Does not decompress any extracted data"}
        };

        static void Main(string[] args)
        {



            ListAllIndexEntries(@"C:\Users\Kelpie\Documents\SimCity 4\Regions\London\City - Interpol.sc4");
            ListAllDirectoryRecord(@"C:\Users\Kelpie\Documents\SimCity 4\Regions\London\City - Interpol.sc4");
            return;
            using (SC4SaveFile file = new(@"C:\Users\Shadowfax\Downloads\City_-_Leptonm.sc4"))
            {
                IndexEntry networkIndex = file.FindIndexEntryWithType(Constants.NETWORK_INDEX_SUBFILE_TYPE); // TODO Add load with type to parser
                byte[] indexFileData = file.LoadIndexEntry(networkIndex);

                NetworkIndex networkFile = new();
                networkFile.Parse(indexFileData);
                networkFile.Dump();
                
            }
        }

        static void ListAllIndexEntries(string path)
        {
            using (SC4SaveFile file = new(path))
            {
                var orderedEntries = file.IndexEntries.OrderBy(entry => entry.FileLocation).ToList();
                for (int i = 0; i < orderedEntries.Count; i++)
                {
                    IndexEntry entry = orderedEntries[i];

                    bool compressed = file.IsIndexEntryCompressed(entry);

                    Console.WriteLine($"[{i+1}]".PadRight(6, ' ') + " tgi={0} offset=0x{1} compressed={4} bytes={2} ",
                        entry.TGI.ToString(),
                        entry.FileLocation.ToString("X8").PadRight(10, ' '),
                        entry.FileSize.ToString().PadRight(10, ' '),
                        i + 1,
                        compressed);
                }

                //Console.WriteLine(JsonMapper.ToJson(file.IndexEntries));
            }
        }

        static void ListAllDirectoryRecord(string path)
        {
            using (SC4SaveFile file = new(path))
            {
                int count = 1;
                foreach (var entry in file.DBDFFile.Resources)
                {

                    Console.WriteLine("entry={2} tgi={0} bytes={1}",
                        entry.TGI.ToString(),
                        entry.DecompressedFileSize.ToString().PadRight(10, ' '),
                        count.ToString().PadRight(5, ' '));


                    count++;
                }

                Console.WriteLine(JsonMapper.ToJson(file.IndexEntries));
            }
        }
    }
}