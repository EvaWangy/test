using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;

namespace CsvToJsonProcessor
{
    public class CsvToJson
    {
        public void CvsToJson()
        {
            try
            {

                string InputFilePath = Utility.getFilePath("InputFilePath");
                string OutputFilePath = Utility.getFilePath("OutputFilePath");
                string ProcessedFilePath = Utility.getFilePath("ProcessedFilePath");

                string[] fileNames = Directory.GetFiles(InputFilePath);
                if (fileNames.Length > 0)
                {
                    foreach (string file in fileNames)
                    {
                        string[] FileExt = file.Split('.');
                        string FileEx = FileExt[FileExt.Length - 1];
                        if (FileEx.ToLower() == "csv")
                        {
                            string strOutputFilePath = OutputFilePath + FileExt[0].Replace(InputFilePath, "") + ".json";
                            string strProcessedFilePath = ProcessedFilePath + FileExt[0].Replace(InputFilePath, "") + "_" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + ".csv";

                            JsonResult resultSet = new JsonResult();
                            string[] csvLines = System.IO.File.ReadAllLines(file);
                            var headers = csvLines[0].Split(',').ToList<string>();
                            foreach (var line in csvLines.Skip(1))
                            {
                                var lineObject = new JObject();
                                var lineAttr = line.Split(',');

                                if (IsValidCsv(csvLines[0].Split(','), lineAttr))
                                {
                                    for (int x = 0; x < headers.Count; x++)
                                    {
                                        lineObject[headers[x]] = lineAttr[x];
                                    }

                                    if (IsValidJson(lineObject.ToString()))
                                    {
                                        resultSet.rows.Add(lineObject);
                                    }
                                    else
                                    {
                                        Logging.SaveMessageToLog(lineObject.ToString(), file, "CvsToJson()");
                                        Logging.SaveMessageToLog("Invalid Json format", file, "CvsToJson()");
                                        Console.WriteLine(lineObject.ToString());
                                        Console.WriteLine("Invalid Json format for file {0}", file);
                                    }
                                }
                                else
                                {
                                    Logging.SaveMessageToLog(line.ToString(), file, "CvsToJson()");
                                    Logging.SaveMessageToLog("Invalid csv line", file, "CvsToJson()");
                                    Console.WriteLine(line.ToString());
                                    Console.WriteLine("Invalid csv line in file {0}", file);
                                }

                            }

                            //open file stream
                            using (StreamWriter sw = File.CreateText(strOutputFilePath))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                //serialize object directly into file stream
                                serializer.Serialize(sw, resultSet);
                            }

                            Logging.SaveMessageToLog("File is converted to json.", file, "CvsToJson()");
                            Console.WriteLine("File - {0} is converted to json.", file);

                            System.IO.File.Move(file, strProcessedFilePath);

                            Logging.SaveMessageToLog("File moved to processed folder.", file, "CvsToJson()");
                            Console.WriteLine("File moved to processed folder.");

                        }
                        else
                        {
                            Logging.SaveMessageToLog("Invalid File.", file, "CvsToJson()");
                            Console.WriteLine("Invalid File");
                        }
                    }

                }
                else
                {
                    Logging.SaveMessageToLog("File Not Found.", "", "CvsToJson()");
                    Console.WriteLine("File Not Found.");
                }
            }
            catch (Exception ex)
            {
                Logging.SaveMessageToLog(ex.Message, "", "CvsToJson()");
                Console.WriteLine(ex.Message);
            }
        }

        private static bool IsValidCsv(string[] headers, string[] lines)
        {
            try
            {
                if (headers.Length == lines.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex) //some other exception
            {
                Logging.SaveMessageToLog(ex.Message, "", "IsValidCsv()");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        //validate Json format
        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Logging.SaveMessageToLog(jex.Message, "", "IsValidJson()");
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Logging.SaveMessageToLog(ex.Message, "", "IsValidJson()");
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
