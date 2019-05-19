using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Common
{
    public class Logging
    {
        public static void SaveMessageToLog(string strMsg, string fileName, string function)
        {
            var line = Environment.NewLine + Environment.NewLine;

            try
            {
                string filepath = Utility.getFilePath("LogFilePath");  //Text File Path

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + "Log" + DateTime.Today.ToString("dd-MM-yyyy-hh-mm-ss") + ".log";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Message :" + strMsg + line;
                    string info = String.Format("In file {0}, function {1}", fileName, function);
                    sw.WriteLine(error);
                    sw.WriteLine(info);
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
