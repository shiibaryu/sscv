namespace batzen
{
    using System;
    using System.IO;
    using Deedle;
    public class Layer3Edges
    {        
        public void getLayer3Edges(string path)
        {
            var network = NetworkBuilder.network;

            var file = Frame.ReadCsv(path);

            var confLen = File.ReadAllLines(path).Length - 1;

            var Inf = file.GetColumn<string>("Interface");
            var Ips = file.GetColumn<string>("IPs");
            var rInf = file.GetColumn<string>("Remote_Interface");
            var rIps = file.GetColumn<string>("Remote_IPs");

            for(int i=0;i<confLen;i++){

                var cInf = Inf.GetAt(i);
                var crInf = rInf.GetAt(i);

                string devname = null;
                string rdevname = null;

                int tail = 0;
                char[] ch = cInf.ToCharArray();
                while(ch[tail] != '['){
                    tail++;
                }
                devname = cInf.Substring(0,tail);

                tail = 0;
                ch = crInf.ToCharArray();
                while(ch[tail] != '['){
                    tail++;
                }
                rdevname = crInf.Substring(0,tail);

                network.Device[devname].Interface[cInf].Neighbor = network.Device[rdevname].Interface[crInf];
                network.Device[rdevname].Interface[crInf].Neighbor = network.Device[devname].Interface[cInf];
            }            
        }
    }
}