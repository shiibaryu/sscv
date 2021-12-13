namespace batzenLib
{
    using System;
    using batzen;

    public class v4RadixTreeNode
    {
        public int valid;

        public Ipv4ForwardingInfo data;

        public v4RadixTreeNode left;

        public v4RadixTreeNode right;
    }

    public class v4RadixTree
    {
        public int Num;

        public v4RadixTreeNode　Root;
    }

    public class Ipv4Radix
    {   
        public v4RadixTree Init(v4RadixTree rt)
        {
            rt = new v4RadixTree();
            rt.Root = new v4RadixTreeNode();

            rt.Root.left = null;
            rt.Root.right = null;

            rt.Num = 1;
            rt.Root.valid = 0;

            return rt;
        }

        /*
            v4
            0x80 -> 10000000
            0x7 -> 111
        */

        /*
            v6
            0x80 * 2  <- 1000000000000000
            d & 0x14か15か16
            d >> 4
        */

        static int BitTest(byte[] key, int d)
        {
            return ((key[(d) >> 3]) & (0x80 >> (d & 0x7)));
        }

        public byte[] getRAddr(string addr)
        {
            int[] pr = new int[4];
            string[] str = addr.Split(".");

            pr[0] = Int32.Parse(str[0]);
            pr[1] = Int32.Parse(str[1]);
            pr[2] = Int32.Parse(str[2]);
            pr[3] = Int32.Parse(str[3]);

            UInt32 uAddr = ((UInt32)pr[0]) + ((UInt32)pr[1] << 8) + ((UInt32)pr[2] << 16) + ((UInt32)pr[3] << 24);
            byte[] d = BitConverter.GetBytes(uAddr);

            return d;
        }

        public int RAdd(v4RadixTreeNode cur,string addr, int prefix, Ipv4ForwardingInfo data, int depth)
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
        private int Add(v4RadixTreeNode cur,byte[] key, int prefix, Ipv4ForwardingInfo data, int depth)
        {
            if(prefix == depth){
                if(cur.valid == 1){
                    Console.WriteLine("already");
                    return 1;
                }
                cur.valid = 1;
                cur.data = data;
                Console.WriteLine(data.Prefix);
                Console.WriteLine(depth);
            }
            else{
                if(BitTest(key,depth) != 0){
                    if(cur.right == null){
                        cur.right = new v4RadixTreeNode();
                        cur.right.valid = 0;
                    }
                    return Add(cur.right,key,prefix,data,depth+1);
                }
                else{
                    if(cur.left == null){
                        cur.left = new v4RadixTreeNode();
                        cur.left.valid = 0;
                    }
                    return Add(cur.left,key,prefix,data,depth+1);
                }
            }
            
            return 0;
        }

        public Ipv4ForwardingInfo RLookUp(v4RadixTreeNode cur, v4RadixTreeNode cand, string addr, int depth)
        {
            byte[] b = getRAddr(addr);

            return LookUp(cur, cand, b, depth);
        }

        private Ipv4ForwardingInfo LookUp(v4RadixTreeNode cur, v4RadixTreeNode cand, byte[] key, int depth)
        {
            if(cur != null && (depth == 32)){
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

        public void Delete(v4RadixTreeNode cur, byte[] key, int prefix, int depth)
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
                    //Console.WriteLine("right");
                    Delete(cur.right,key,prefix,depth+1);
                    return;
                }
                else{   
                    //Console.WriteLine("left");
                    Delete(cur.left,key,prefix,depth+1);
                    return;
                }
            }
        }

        public void Shrink(v4RadixTreeNode cur)
        {
               
        }
        
    }

}