namespace batzenLib
{
    using System;
    using System.Net;
    using batzen;

    public class v6RadixTreeNode
    {
        public int valid;

        public Ipv6ForwardingInfo data;

        public v6RadixTreeNode left;

        public v6RadixTreeNode right;
    }

    public class v6RadixTree
    {
        public int Num;

        public v6RadixTreeNode　Root;
    }

    public class Ipv6Radix
    {
         public v6RadixTree Init(v6RadixTree rt)
        {
            rt = new v6RadixTree();
            rt.Root = new v6RadixTreeNode();

            rt.Root.left = null;
            rt.Root.right = null;

            rt.Num = 1;
            rt.Root.valid = 0;

            return rt;
        }

        static int BitTest(byte[] key, int d)
        {
            return ((key[(d) >> 3]) & (0x80 >> (d & 0x7)));
        }

        public byte[] getRAddr(string addr)
        {
            IPAddress v6Addr = IPAddress.Parse(addr);
            var addrByte = v6Addr.GetAddressBytes();

            return addrByte;
        }

        public int RAdd(v6RadixTreeNode cur,string addr, int prefix, Ipv6ForwardingInfo data, int depth)
        {
            byte[] b = getRAddr(addr);
            return Add(cur,b,prefix,data,depth);
        }

        /*
            return:
                    0  if success
                    1  if a node of the prefix exists
                    -1 if fail
        */
        private int Add(v6RadixTreeNode cur,byte[] key, int prefix, Ipv6ForwardingInfo data, int depth)
        {
            if(prefix == depth){
                if(cur.valid == 1){
                    Console.WriteLine("already");
                    return 1;
                }
                cur.valid = 1;
                cur.data = data;
                //Console.WriteLine(data.Prefix);
                Console.WriteLine(depth);
            }
            else{
                if(BitTest(key,depth) != 0){
                    if(cur.right == null){
                        cur.right = new v6RadixTreeNode();
                        cur.right.valid = 0;
                    }
                    return Add(cur.right,key,prefix,data,depth+1);
                }
                else{
                    if(cur.left == null){
                        cur.left = new v6RadixTreeNode();
                        cur.left.valid = 0;
                    }
                    return Add(cur.left,key,prefix,data,depth+1);
                }
            }
            
            return 0;
        }

        public Ipv6ForwardingInfo RLookUp(v6RadixTreeNode cur, v6RadixTreeNode cand, string addr, int depth)
        {
            byte[] b = getRAddr(addr);

            return LookUp(cur, cand, b, depth);
        }

        private Ipv6ForwardingInfo LookUp(v6RadixTreeNode cur, v6RadixTreeNode cand, byte[] key, int depth)
        {
            if(cur != null && (depth == 128)){
                if(cur.valid == 1){
                    return cur.data;
                }
            }

            if(cur == null){
                return cand != null ? cand.data : null;
            }

            if(cur.valid == 1){
                cand = cur;
            }

            if(BitTest(key,depth) != 0){
                return LookUp(cur.right,cand,key,depth+1);
            }
            else{
                return LookUp(cur.left,cand,key,depth+1);
            }
        }

        public void Delete(v6RadixTreeNode cur, byte[] key, int prefix, int depth)
        {
            if(cur == null){
                Console.WriteLine("No prefix");
                return;
            }

            if(prefix == depth){
                if(BitTest(key,depth) != 0){
                    if(cur.right != null){
                        Console.WriteLine("right delete");
                        cur.right = null;
                        Shrink(cur);
                        return;
                    }
                }
                else{
                    if(cur.left != null){
                        Console.WriteLine("left delete");
                        cur.left = null;
                        Shrink(cur);
                        return;
                    }
                }
            }
            else{
                if(BitTest(key,depth) != 0){
                    Delete(cur.right,key,prefix,depth+1);
                    return;
                }
                else{   
                    Delete(cur.left,key,prefix,depth+1);
                    return;
                }
            }
        }

        public void Shrink(v6RadixTreeNode cur)
        {
               
        }
    }
}