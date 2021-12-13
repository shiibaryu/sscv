namespace batzen
{
    using System;
    using System.Collections.Generic;
    using ZenLib;
    using static ZenLib.Language;

    class Reachability
    {
        public static int p = 0;

        Stack<Device> stack = null;

        public Device destDevice = null;

        public static HashSet<string> pathList;

        //public static HashSet<List<string>> pathList;
        //public static List<List<string>> pathList;


        //public string prevDevice = null;

        public void setDstDevice(string dev)
        {
            this.destDevice = NetworkBuilder.network.Device[dev];

            stack = new Stack<Device>();

            pathList = new HashSet<string>();
            //pathList = new HashSet<List<string>>();
            //pathList = new List<List<string>>();
        }

        public void reach(string curDevice,string prevDevice)
        {
            Device cDev = NetworkBuilder.network.Device[curDevice];

            //push current device name to stack
            //Console.WriteLine(cDev.Name);
            this.stack.Push(cDev);

            //paste true label
            cDev.isVisited = true;


            if(this.destDevice == cDev){
                /*do verification*/
                int size = this.stack.Count;

                string path = null;
                //List<string> path = new List<string>();

                foreach(var s in stack){
                    /*
                    Console.Write(s.Name);
                    Console.Write("--");
                    */

                    path += s.Name;
                    path += "-";
                    //path.Add(s.Name);
                }
            
                //Console.WriteLine();
                
                pathList.Add(path);
                //pathList.Add(path);
                p++;
            }
            else{
                foreach(var inf in cDev.Interface){

                    if(inf.Value.Name.Contains("lo")){
                        continue;
                    }

                    if(inf.Value.Neighbor == null){
                        continue;
                    }

                    Device neiborDev = inf.Value.Neighbor.Owner;

                    /*
                    Console.WriteLine(curDevice);
                    Console.WriteLine(neiborDev.Name);
                    */

                    if(neiborDev.isVisited == true){
                        continue;
                    }

                    if(cDev.Name.Contains("S") && cDev.Name.Contains("SP") == false && prevDevice != null){
                        if(prevDevice.Contains("T")){
                            cDev.isVisited = false;
                            break;
                        }
                        else{
                            Console.WriteLine("Else error (return) at Server ");
                        }
                    }
                    else if(cDev.Name.Contains("T")){
                        if(!(prevDevice.Contains("S") && prevDevice.Contains("SP") == false && neiborDev.Name.Contains("L") || 
                             prevDevice.Contains("L") && neiborDev.Name.Contains("S") && neiborDev.Name.Contains("SP") == false) || 
                             prevDevice.Contains("S") && prevDevice.Contains("SP") == false && neiborDev.Name.Contains("S") && neiborDev.Name.Contains("SP") == false && prevDevice != neiborDev.Name){
                                continue;
                        }
                    }
                    else if(cDev.Name.Contains("L")){
                        if(!(prevDevice.Contains("T") && neiborDev.Name.Contains("SP") || 
                             prevDevice.Contains("SP") && neiborDev.Name.Contains("T") ||
                             prevDevice.Contains("T") && neiborDev.Name.Contains("T") && prevDevice != neiborDev.Name)
                             ){
                            continue;
                        }
                    }
                    else if(cDev.Name.Contains("SP")){
                        if(!(prevDevice.Contains("L") && neiborDev.Name.Contains("L") && prevDevice != neiborDev.Name)){
                            continue;
                        }
                    }
                    else{
                        if(prevDevice != null){
                            //Console.WriteLine("else!");   
                            //Console.WriteLine(cDev.Name);
                            //Console.WriteLine(neiborDev.Name);
                            break;
                        }
                    }

                    reach(neiborDev.Name,curDevice);                    
                }
            }
            Device dev = stack.Pop();
            //Console.WriteLine("pop at " + dev.Name);
            dev.isVisited = false;
        }
    }
}