namespace batzen
{
    using System.Collections.Generic;
    
    public class DeviceProperty
    {
        public string Name;

        /*<device name,InterfaceProperty>*/
        public Dictionary<string,InterfaceProperty> ifp;

        /*<device name,RouteProperty>*/
        public Dictionary<string,Ipv4FIB> v4Fib =  new Dictionary<string, Ipv4FIB>();

        public Dictionary<string,Ipv6FIB> v6Fib = new Dictionary<string, Ipv6FIB>();

        public List<RouteProperty> routep = new List<RouteProperty>();

        public List<RouteProperty> routepv6 = new List<RouteProperty>();

        public List<Srv6Rule> Srv6Rule = new List<Srv6Rule>();
    }
}