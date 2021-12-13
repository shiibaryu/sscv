namespace batzen
{
    using System;
    using System.Collections.Generic;

    public class FrrIpv6RouteProperty
    {
        public void getv6RouteProperties(string path,DeviceProperty device)
        {
            string line;
            System.IO.StreamReader sr = new System.IO.StreamReader(path);

            if(sr == null){
                Console.WriteLine("No file at the path.");
            }

            List<RouteProperty> routep = new List<RouteProperty>();

            line = sr.ReadLine();

            while(line != null){
                //Srv6Rule srv6Rule = new Srv6 Rule();
                
                string[] str = line.Split(' ');

                if(str[0] == "local" || str[0] == "anycast" || str[0] == "multicast"){
                    line = sr.ReadLine();
                    continue;
                }

                if(str[1] == "dev"){
                    if(str[2] != "lo"){
                        routep.Add(new RouteProperty(){IpAddress = str[0], Interface = str[2],NextHop = null});   
                    }

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
                    //table.NextHop = device.Interface[str[2]].Neighbor.Owner.Name;
                    //table.NextHopIf = device.Interface[str[2]].Neighbor.Name;  
                }
                else if(str[1] == "proto"){
                    
                    //Console.WriteLine("proto  !!!!!!!!!!!!!!");

                    line = sr.ReadLine();

                    while(line != null && line.Split(" ")[0] == "	nexthop"){
                        string[] yaStr = line.Split(" ");
                        routep.Add(new RouteProperty(){IpAddress = str[0], Interface = yaStr[4],NextHop = yaStr[2]});
                        line = sr.ReadLine();
                    }
                    if(str[1] != "proto"){
                        line = sr.ReadLine();
                    }
                }
                else if(str[2] == "encap"){
                    Srv6Rule srv6Rule = new Srv6Rule();

                    if(str[3] == "seg6"){
                        if(str[5] == "encap"){
                            Ipv6 targetAddr = Ipv6.Parse(str[0]+"/128");

                            string mode = str[5];
                            int size = Int16.Parse(str[7]);
                            string[] v6Rule = new string[size];
 
                            //int prefix = Int32.Parse(str[0].Split("/")[1]);
							int prefix = 128;
                            
                            
                            int i = 0;
                            int sidx = 9;

                            while(str[sidx] != "]"){
                                v6Rule[i] = str[sidx]+"/128";
                                sidx++;
                                i++;                                
                            }

                            srv6Rule.mode = mode;
                            srv6Rule.targetAddress = targetAddr;
                            srv6Rule.encapAddress = v6Rule;
                            srv6Rule.index = 0;
                            srv6Rule.prefix = prefix;
    
                            device.Srv6Rule.Add(srv6Rule);
                        } 
                        else if(str[5] == "inline"){
                            //target prefix str[0], mode str[5], encap addr str[9~]
                            int i = 0;
                            Ipv6 targetAddr = Ipv6.Parse(str[0]+"/128");
                            string mode = str[5];
                            int size = Int16.Parse(str[7]) - 1;
                            string[] v6Rule = new string[size];

                            int idx = 9;

                            while(str[idx] != "::"){
                                v6Rule[i] = str[idx]+"/128";
                                idx++;
                                i++;
                            }

                            srv6Rule.mode = mode;
                            srv6Rule.targetAddress = targetAddr;
                            srv6Rule.encapAddress = v6Rule;
                            srv6Rule.index = 0;
    
                            device.Srv6Rule.Add(srv6Rule);
    
                        }                   
                    }
                    else if(str[3] == "seg6local"){
                        if(str[5] == "End"){
                            Ipv6 targetAddr = Ipv6.Parse(str[0]+"/128");  
                            string mode = str[5];   

                            srv6Rule.mode = mode;
                            srv6Rule.targetAddress = targetAddr;
                            srv6Rule.index = 0;

                            device.Srv6Rule.Add(srv6Rule);
                        }
                        else if(str[5] == "End.DX6"){
                            Ipv6 targetAddr = Ipv6.Parse(str[0]+"/128");    
                            string mode = str[5];   

                            srv6Rule.mode = mode;
                            srv6Rule.targetAddress = targetAddr;
                            srv6Rule.index = 0;

                            device.Srv6Rule.Add(srv6Rule);
                        }
                    }
                    else{
                        Console.WriteLine("else  !!!!!!!!!!!!!!");
                    }

                    line = sr.ReadLine();
                }
            }
            device.routepv6 = routep;
            //device.segmentRoutingv6List = seg6AddrList;
        }
    }
}