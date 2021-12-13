namespace batzen
{
    using System;
    using System.Collections.Generic;

    public class FrrIpv4RouteProperty
    {
        public void getv4RouteProperties(string path,DeviceProperty device)
        {
            string line;

            System.IO.StreamReader sr = new System.IO.StreamReader(path);

            if(sr == null){
                Console.WriteLine("No file at the path.");
            }

            List<RouteProperty> routep = new List<RouteProperty>();
            
            line = sr.ReadLine();
            
            while(line != null){
                string[] str = line.Split(' ');
                
                if(str[1] == "dev"){
                    routep.Add(new RouteProperty(){IpAddress = str[0], Interface = str[2],NextHop = null});
                    
                    //string[] net = v4FwInfo.Prefix.Split("/");

                    line = sr.ReadLine();
                }
                else if(str[1] == "via"){
                    if(str[0] == "default"){
                        routep.Add(new RouteProperty(){IpAddress = "0.0.0.0/0", Interface = str[4],NextHop = null});

                        line = sr.ReadLine();
                    }
                    else{
                        routep.Add(new RouteProperty(){IpAddress = str[0], Interface = str[4],NextHop = str[2]});

                        line = sr.ReadLine();
                    }
                }
                else if(str[1] == "proto"){
                    
                    Console.WriteLine("proto  !!!!!!!!!!!!!!");
                
                    //v4FwInfo.Prefix = str[0];
                    line = sr.ReadLine();

                    while(line != null && line.Split(" ")[0] == "	nexthop"){
                        string[] yaStr = line.Split(" ");

                        //v4FwInfo.Interface = yaStr[4];
                        //v4FwInfo.NextHop = yaStr[2];

                        //string[] net = v4FwInfo.Prefix.Split("/");
                        //int subnet = Int32.Parse(net[1]);

                        //v4Rad.RAdd(v4Fib.Radix.Root,net[0],subnet,v4FwInfo,0);5

                        routep.Add(new RouteProperty(){IpAddress = str[0], Interface = yaStr[4],NextHop = yaStr[2]});


                        line = sr.ReadLine();
                        /*
                        if(line != null && line.Split(" ")[0] == "	nexthop"){
                            v4FwInfo = new Ipv4ForwardingInfo();
                            v4FwInfo.Prefix = str[0];
                        }   
                        */
                    }
                }

            }
            device.routep = routep;
        }
    }
}