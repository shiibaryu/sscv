using System;
using System.IO;
using Deedle;

namespace  batzen
{
    public class ProcessConfiguration
    {
        public string Node;

        public string VRF;

        public string Router_ID;

        public int Confederation_ID;

        public int Confederation_Members;

        public bool Multipath_EBGP;

        public bool Multipath_IBGP;

        public string Multipath_Match_Mode;

        public string Neighbors;

        public bool Route_Reflector;

        public string Tie_Breaker;
    }

    public class PeerConfiguration
    {
        public string Node;

        public string VRF;

        public int Local_AS;

        public string Local_IP;

        public string Local_Interface;

        public int Confederation;

        public string Remote_AS;

        public string Remote_IP;

        public string Description;

        public bool Route_Reflector_Client;

        public string Cluster_ID;

        public string Peer_Group;

        public string Import_Policy;

        public string Export_Policy;

        public bool Send_Community;

        public bool Is_Passive;
    }
    public class Bgp
    {

        public ProcessConfiguration[] pc;
        
        public PeerConfiguration[] pe;
        
        public void getBgpProperties(string prPath,string pePath)
        {
            getProcessConfiguration(prPath);
            getPeerConfiguration(pePath);
        }

        public void getProcessConfiguration(string path)
        { 
            var file = Frame.ReadCsv(path);

            var confLen = File.ReadAllLines(path).Length - 1;

            pc = new ProcessConfiguration[confLen];


        }

        public void getPeerConfiguration(string path)
        {
            var file = Frame.ReadCsv(path);

            var confLen = File.ReadAllLines(path).Length - 1;

            pe = new PeerConfiguration[confLen];

            
        }
    }
}