namespace batzen
{
    using ZenLib;
    public class Forward
    {
        delegate bool Process(Zen<Packet> pkt,Device device);   
    }
}