namespace batzen
{
    using System;
    using System.Collections.Generic;

    public class Property
    {   
        public DeviceProperty[] deviceProperty;

        public void setProperty(string directoryPath,int deviceNum)
        {
            deviceProperty = new DeviceProperty[deviceNum];

            //for Frr
            FrrProperty frp = new FrrProperty();
            frp.setProperty(deviceProperty,directoryPath);
        }
    }
    
}