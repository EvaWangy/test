using System;
using System.Linq;
using System.Timers;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CsvToJsonProcessor;

namespace TimerService
{
    public class Timer1
    {
        private static System.Timers.Timer aTimer;

        public static void Main()
        {
            
            aTimer = new System.Timers.Timer();
            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 60000;
            aTimer.Enabled = true;

            Console.WriteLine("Press the Enter key to exit the program.");
            Console.ReadLine();

        }

        // Specify what you want to happen when the Elapsed event is 
        // raised.
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var timer = source as System.Timers.Timer;

            timer.Stop();
            CsvToJson csvToJson = new CsvToJson();

            csvToJson.CvsToJson();
            timer.Start();
           
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }



    }
}



