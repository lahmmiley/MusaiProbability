using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class FileTool
    {
        private const string RESULT_FOLDER = "../../result/";

        public static void Write(string fileName, string content)
        {
            if(Directory.Exists(RESULT_FOLDER))
            {
                Directory.CreateDirectory(RESULT_FOLDER);
            }
            string path = RESULT_FOLDER + fileName;
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}
