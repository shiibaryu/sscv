namespace batzen
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class FrrInterfaceProperty
    {
        public string getIfname(string ifname)
        {
            char[] c = ifname.ToCharArray();

            int idx;
            for(idx=0;idx<c.Length;idx++){
                if(c[idx] == ':' || c[idx] == '@'){
                    break;
                }
            }

            string name = ifname.Substring(0,idx);
            //Console.WriteLine(name);

            return name;
        }

        public void getInterfaceProperties(string ifPath,DeviceProperty device)
        {   
            device.ifp = new System.Collections.Generic.Dictionary<string, InterfaceProperty>();
            
            using (StreamReader sr = File.OpenText(ifPath))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                JEnumerable<JToken> children = JToken.ReadFrom(reader).Children();
                foreach(var elm in children){
                    InterfaceProperty ifp = new InterfaceProperty();
                    ifp.Name = elm["ifname"].ToString();
                    ifp.Mtu = Convert.ToInt32(elm["mtu"].ToString());
                    if(elm["flags"][1].ToString() == "UP" || elm["flags"][1].ToString() == "DOWN"){
                        ifp.State = elm["flags"][1].ToString();
                    }
                    else{
                        ifp.State = elm["flags"][2].ToString();
                    }
                    ifp.Owner = device.Name;
                    ifp.PortNumber = Convert.ToByte(elm["ifindex"].ToString());
                    ifp.EtherAddr = elm["address"].ToString();

                    foreach(var ai in elm["addr_info"]){
                        if(ai["family"].ToString() == "inet"){
                            ifp.Ipv4Addr = ai["local"].ToString() + "/" + ai["prefixlen"].ToString();
                        }
                        else{
                            string addr;
                            addr =  ai["local"].ToString() + "/" + ai["prefixlen"].ToString();
                            if(ai["scope"].ToString() == "global"){
                                ifp.gIpv6Addr.Add(addr);
                            }
                            else if(ai["scope"].ToString() == "local"){
                                ifp.loIpv6Addr = addr;
                            }
                            else if(ai["scope"].ToString() == "link"){
                                ifp.liIpv6Addr = addr;
                            }
                            else if(ai["scope"].ToString() == "host"){
                                ifp.hIpv6Addr = addr;
                            }
                        }
                    }

                    device.ifp[ifp.Name] = ifp;
                }
            }
        }
    }
}