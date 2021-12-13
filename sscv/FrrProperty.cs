namespace batzen
{
    using System;
    using System.IO;
    using batzenLib;

    class FrrProperty
    {
		public const string ifPath = "./dc_conf/{0}/if.json";
        
        public const string v4RoutePath = "./dc_conf/{0}/v4route.txt";

        public const string v6RoutePath = "./dc_conf/{0}/v6route.txt";

        public void setProperty(DeviceProperty[] deviceProperty,string directoryPath)
        {
            FrrInterfaceProperty fip = new FrrInterfaceProperty();
            FrrRouteProperty frp = new FrrRouteProperty();

            int idx=0;
            string[] dirs = Directory.GetDirectories(directoryPath);
            foreach(string dir in dirs){
                string[] sp = dir.Split('/');
                int length = sp.Length;
                string deviceName = sp[length-1];

                if(deviceName != "topology"){
                    //var ifnp = string.Format(ifNumPath,deviceName);
                    var ifp = string.Format(ifPath,deviceName);
                    var v4rp = string.Format(v4RoutePath,deviceName);
                    var v6rp = string.Format(v6RoutePath,deviceName);

                    deviceProperty[idx] = new DeviceProperty();
                    deviceProperty[idx].Name = deviceName;
                    
                    fip.getInterfaceProperties(ifp,deviceProperty[idx]);
                    frp.getRouteProperties(v4rp,v6rp,deviceProperty[idx]);

                    idx++;
                }
            }
        }
    }
}
