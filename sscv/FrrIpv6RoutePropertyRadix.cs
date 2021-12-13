namespace batzen
{
    using System;
    using batzenLib;

    public class FrrIpv6RoutePropertyRadix
    {
        public void getv6RouteProperties(string path,DeviceProperty device)
        {
            string line;
            System.IO.StreamReader sr = new System.IO.StreamReader(path);

            if(sr == null){
                Console.WriteLine("No file at the path.");
            }

            Ipv6FIB v6Fib = new Ipv6FIB();
            Ipv6Radix v6Rad = new Ipv6Radix();

            Ipv6ForwardingInfo v6FwInfo;

            v6Fib.Radix = new v6RadixTree();
            v6Fib.Radix.Root = new v6RadixTreeNode();

            line = sr.ReadLine();

            Console.WriteLine(device.Name);
            
            while(line != null){
                string[] str = line.Split(' ');

                v6FwInfo = new Ipv6ForwardingInfo();

                if(str[0] == "local"){
                    line = sr.ReadLine();
                    continue;   
                }
                if(str[1] == "dev"){
                    v6FwInfo.Prefix = str[0];
                    v6FwInfo.Interface = str[2];
                    v6FwInfo.NextHop = null;

                    string[] net = str[0].Split("/");
                    int subnet = Int32.Parse(net[1]);

                    v6Rad.RAdd(v6Fib.Radix.Root,net[0],subnet,v6FwInfo,0);
                
                    line = sr.ReadLine();
                }
                else if(str[1] == "via"){
                    if(str[0] == "default"){
                        line = sr.ReadLine();
                    }
                    else{
                        v6FwInfo.Prefix = str[0];
                        v6FwInfo.NextHop = str[2];
                        v6FwInfo.Interface = str[4];

                        string[] net = str[0].Split("/");
                        int subnet = Int32.Parse(net[1]);

                        v6Rad.RAdd(v6Fib.Radix.Root,net[0],subnet,v6FwInfo,0);
                    
                        line = sr.ReadLine();
                    }
                }
                else if(str[1] == "proto"){
                                      
                    v6FwInfo.Prefix = str[0];
                    line = sr.ReadLine();

                    while(line != null && line.Split(" ")[0] == "	nexthop"){
                        string[] yaStr = line.Split(" ");

                        v6FwInfo.Interface = yaStr[4];
                        v6FwInfo.NextHop = yaStr[2];

                        string[] net = v6FwInfo.Prefix.Split("/");
                        int subnet = Int32.Parse(net[1]);

                        v6Rad.RAdd(v6Fib.Radix.Root,net[0],subnet,v6FwInfo,0);

                        line = sr.ReadLine();

                        if(line != null && line.Split(" ")[0] == "	nexthop"){
                            v6FwInfo = new Ipv6ForwardingInfo();
                            v6FwInfo.Prefix = str[0];
                        }
                    }

                    line = sr.ReadLine();
                }
                else if(str[2] == "encap"){
                    if(str[3] == "seg6"){
                        /*
                        device.Rtab[idx] = new Route_Table();
                        device.Rtab[idx].Network = str[0];
                        device.Rtab[idx].SrcInterface = "net2";

                        device.Rtab[idx].NextHopIf = device.Interface["net2"].Neighbor.Name;
                        device.Rtab[idx].NextHopHost = device.Interface["net2"].Neighbor.Owner.Name;

                        NetworkBuilder.st[idx-2] = new STable();
                        NetworkBuilder.st[idx-2].NextInterface = str[16];
                        */

                        Console.WriteLine("----------------");
                        Console.WriteLine("seg6 idex is");
                        //Console.WriteLine("seg6 idex is {0}",device.Rtab[idx].NextHopIf);
                        //Console.WriteLine("seg6 idex is {0}",device.Rtab[idx].NextHopHost);
                        Console.WriteLine("----------------");
                    }
                    else if(str[3] == "seg6local"){
                        /*
                        string encapAddr = str[9];

                        device.Rtab[idx] = new Route_Table();
                        device.Rtab[idx].Network = str[0];

                        */
                    }
                    else{
                        Console.WriteLine("else  !!!!!!!!!!!!!!");
                        Console.WriteLine("elsse  !!!!!!!!!!!!!!");
                        Console.WriteLine("plse  !!!!!!!!!!!!!!");
                        Console.WriteLine("else  !!!!!!!!!!!!!!");
                    }

                    line = sr.ReadLine();
                }
            }
            device.v6Fib[device.Name] = v6Fib;
        }
    }
}