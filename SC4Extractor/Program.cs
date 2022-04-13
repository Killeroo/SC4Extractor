using System;

using SC4Parser.Files;

namespace SC4Extractor // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (SC4SaveFile file = new SC4SaveFile(@"C:\Users\Shadowfax\Downloads\City_-_Leptonm.sc4"))
            {
                //file.Dump();
                file.GetNetworkSubfile1().Dump();
            }
        }
    }
}