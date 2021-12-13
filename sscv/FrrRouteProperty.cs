namespace batzen
{
    using System;
    using batzenLib;

    class FrrRouteProperty
    {
        public void getRouteProperties(string v4Path,string v6Path,DeviceProperty device)
        {
            /*   
            FrrIpv4RouteProperty v4r = new FrrIpv4RouteProperty();
            v4r.getv4RouteProperties(v4Path,device);
            */
            
            FrrIpv6RouteProperty v6r = new FrrIpv6RouteProperty();
            v6r.getv6RouteProperties(v6Path,device);
        }
    }
}