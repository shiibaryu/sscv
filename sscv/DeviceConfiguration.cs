namespace batzen
{
    using System;

    public class DeviceConfiguration
    {
        public void setConfigurations(Property prop)
        {
            Network network = NetworkBuilder.network;
            DeviceProperty[] deviceProperty = prop.deviceProperty;
            int deviceNumber = deviceProperty.Length;

            for(int i=0;i<deviceNumber;i++){
                string devName = deviceProperty[i].Name;
                Device device = network.Device[devName];

                device.setConfiguration(device,deviceProperty[i]);
            }
        }
    }
}