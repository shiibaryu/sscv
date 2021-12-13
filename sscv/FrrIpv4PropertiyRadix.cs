namespace batzen
{
    using System;
    using System.IO;
    using batzenLib;
    using System.Diagnostics.CodeAnalysis;

    public class FrrIpv4RoutePropertyRadix
    {
        public void getv4RouteProperties(string path,DeviceProperty device)
        {
            string line;

            System.IO.StreamReader sr = new System.IO.StreamReader(path);

            if(sr == null){
                Console.WriteLine("No file at the path.");
            }

            Ipv4FIB v4Fib = new Ipv4FIB();
            Ipv4Radix v4Rad = new Ipv4Radix();

            Ipv4ForwardingInfo v4FwInfo;
            
            v4Fib.Radix = new v4RadixTree();
            v4Fib.Radix.Root = new v4RadixTreeNode();
            
            line = sr.ReadLine();
            while(line != null){
                string[] str = line.Split(' ');

                v4FwInfo = new Ipv4ForwardingInfo();
                
                if(str[1] == "dev"){
                    v4FwInfo.Prefix = str[0];
                    v4FwInfo.Interface = str[2];
                    //dounikasusu !!!!
                    v4FwInfo.NextHop = null;

                    Console.WriteLine("fwInfo.Interface: {0}",v4FwInfo.Interface);
                    Console.WriteLine("fwInfo.Prefix: {0}",v4FwInfo.Prefix);
                    Console.WriteLine("fwInfo.Nexthop: {0}",v4FwInfo.NextHop);
                    
                    //string[] net = v4FwInfo.Prefix.Split("/");
                    string[] net = str[0].Split("/");
                    int subnet = Int32.Parse(net[1]);

                    v4Rad.RAdd(v4Fib.Radix.Root,net[0],subnet,v4FwInfo,0);

                    line = sr.ReadLine();
                }
                else if(str[1] == "via"){
                    if(str[0] == "default"){
                        //デフォげの設定？？？
                        v4FwInfo.Prefix = "0.0.0.0/0";
                        v4FwInfo.Interface = str[4];
                        v4FwInfo.NextHop = null;

                        Console.WriteLine("fwInfo.Interface: {0}",v4FwInfo.Interface);
                        Console.WriteLine("fwInfo.Prefix: {0}",v4FwInfo.Prefix);
                        Console.WriteLine("fwInfo.Nexthop: {0}",v4FwInfo.NextHop);

                        v4Rad.RAdd(v4Fib.Radix.Root,"0.0.0.0",0,v4FwInfo,0);

                        line = sr.ReadLine();
                    }
                    else{
                        v4FwInfo.Prefix = str[0];
                        v4FwInfo.Interface = str[4];
                        v4FwInfo.NextHop = str[2];

                        Console.WriteLine("fwInfo.Interface: {0}",v4FwInfo.Interface);
                        Console.WriteLine("fwInfo.Prefix: {0}",v4FwInfo.Prefix);
                        Console.WriteLine("fwInfo.Nexthop: {0}",v4FwInfo.NextHop);
                        //table.NextHop = device.Interface[str[4]].Neighbor.Owner.Name;
                        //table.NextHopIf = device.Interface[str[4]].Neighbor.Name;

                        //string[] net = v4FwInfo.Prefix.Split("/");
                        string[] net = str[0].Split("/");
                        int subnet = Int32.Parse(net[1]);

                        v4Rad.RAdd(v4Fib.Radix.Root,net[0],subnet,v4FwInfo,0);

                        line = sr.ReadLine();
                    }
                }
                else if(str[1] == "proto"){
                    
                    Console.WriteLine("proto  !!!!!!!!!!!!!!");
                
                    v4FwInfo.Prefix = str[0];
                    line = sr.ReadLine();
                    Console.WriteLine(line);
                    while(line != null && line.Split(" ")[0] == "	nexthop"){
                        string[] yaStr = line.Split(" ");

                        v4FwInfo.Interface = yaStr[4];
                        v4FwInfo.NextHop = yaStr[2];

                        string[] net = v4FwInfo.Prefix.Split("/");
                        int subnet = Int32.Parse(net[1]);

                        v4Rad.RAdd(v4Fib.Radix.Root,net[0],subnet,v4FwInfo,0);

                        Console.WriteLine(device.Name);
                        Console.WriteLine("!!!!!!!!!!!!!");
                        Console.WriteLine("fwInfo.Interface: {0}",v4FwInfo.Interface);
                        Console.WriteLine("fwInfo.Prefix: {0}",v4FwInfo.Prefix);
                        Console.WriteLine("fwInfo.Nexthop: {0}",v4FwInfo.NextHop);

                        line = sr.ReadLine();
                        
                        if(line != null && line.Split(" ")[0] == "	nexthop"){
                            v4FwInfo = new Ipv4ForwardingInfo();
                            v4FwInfo.Prefix = str[0];
                        }
                        
                    }
                    Console.WriteLine("break");
                }

            }
           
            device.v4Fib[device.Name] = v4Fib;
        }

    }
}