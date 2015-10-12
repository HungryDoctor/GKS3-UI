using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS3_UI
{
    static class IO
    {
        public static Dictionary<int, string> ParseStrings(string path)
        {
            try
            {
                StreamReader reader = new StreamReader(path);
                string line;
                Dictionary<int, string> strings = new Dictionary<int, string>();

                int counter = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    strings.Add(counter,line);
                    counter++;
                }

                return strings;
            }
            catch
            {
                return null;
            }
        }
    }
}