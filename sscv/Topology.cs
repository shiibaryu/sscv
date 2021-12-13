namespace batzen
{
    using System.IO;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Topology
    {
        public void setTopology(string TopoPath,int devNum)
        {
            Network network = NetworkBuilder.network;
            Device[] devices = new Device[devNum];

            var file = File.ReadAllText(TopoPath);
            dynamic list = JsonConvert.DeserializeObject<dynamic>(file);

            int DevNum = 0;

            foreach(var i in list){
                string Dev1Name = i.node1.hostname;
                string Dev1If = i.node1.interfaceName;

                string Dev2Name = i.node2.hostname;
                string Dev2If = i.node2.interfaceName;

                Device temp = null;

                if(network.Device.TryGetValue(Dev1Name,out temp) == false){
                    devices[DevNum] = new Device {Interface = new Dictionary<string,Interface>()};
                    devices[DevNum].Name = Dev1Name;

                    Interface ifa = new Interface();
                    ifa.Name = Dev1If;
                    ifa.Owner = devices[DevNum];
                    devices[DevNum].Interface[ifa.Name] = ifa;

                    network.Device[Dev1Name] = devices[DevNum];
                    DevNum += 1;
                }
                else{
                    Device dev = network.Device[Dev1Name];

                    Interface ifa = new Interface();
                    ifa.Name = Dev1If;
                    ifa.Owner = dev;
                    dev.Interface[ifa.Name] = ifa;
                }

                if(network.Device.TryGetValue(Dev2Name,out temp) == false){
                    devices[DevNum] = new Device {Interface = new Dictionary<string,Interface>()};
                    devices[DevNum].Name = Dev2Name;

                    Interface ifa = new Interface();
                    ifa.Name = Dev2If;
                    ifa.Owner = devices[DevNum];
                    devices[DevNum].Interface[ifa.Name] = ifa;
                    
                    network.Device[Dev2Name] = devices[DevNum];
                    DevNum += 1; 
                }
                else{
                    Device dev = network.Device[Dev2Name];

                    Interface ifa = new Interface();
                    ifa.Name = Dev2If;
                    ifa.Owner = dev;
                    dev.Interface[ifa.Name] = ifa;
                }
                
                network.Device[Dev1Name].Interface[Dev1If].Neighbor = network.Device[Dev2Name].Interface[Dev2If];
                network.Device[Dev2Name].Interface[Dev2If].Neighbor = network.Device[Dev1Name].Interface[Dev1If];
            }
            
        }   
    }
}