namespace batzen
{
    using System;

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    class Program
    {
        const string directoryPath = "./dc_conf";

        static void Main(string[] args)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            int deviceNum = System.IO.Directory.GetDirectories(directoryPath).Length - 1;
            // add number of switch
            int switchNum = 0;
            deviceNum += switchNum;

            Property prop = new Property();
            prop.setProperty(directoryPath,deviceNum-switchNum);

	    	NetworkBuilder nw = new NetworkBuilder();
            nw.Build(prop,deviceNum);
	    	nw.Run();

	    	sw.Stop();
            TimeSpan ts = sw.Elapsed;
            //Console.WriteLine($"　{ts}");
            Console.WriteLine($"{ts.Seconds}.{ts.Milliseconds}");
            //Console.WriteLine($" {ts.Minutes} minute {ts.Seconds} second {ts.Milliseconds} milisecond");
            //Console.WriteLine($"　{sw.ElapsedMilliseconds} milisecond");        
        }
    }
}
