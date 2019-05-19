using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Common
{
    public static class Utility
    {
        public static string getFilePath(string file)
        {
            string filePath = String.Empty;
            switch(file)
            {
                case "InputFilePath":
                    filePath = System.Configuration.ConfigurationManager.AppSettings["InputFilePath"].ToString();
                    break;
                case "OutputFilePath":
                    filePath =   System.Configuration.ConfigurationManager.AppSettings["OutputFilePath"].ToString();
                    break;
                case "LogFilePath":
                    filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString();
                    break;
                case "ProcessedFilePath":
                    filePath =  System.Configuration.ConfigurationManager.AppSettings["ProcessedFilePath"].ToString();
                    break;
                   }
              return filePath;

        }

        
    }
}
