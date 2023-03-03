using System.Collections;

namespace ChatServer.util;

public class BitArrayUtil
{
    public static BitArray CascadeBitArrayList(IList<BitArray> bits)
    {
        BitArray e = new BitArray(bits[0].Length);
        foreach (var b in bits)
        {
            e = CascadeBitArray(e, b);
        }

        return e;
    }
    
    public static BitArray CascadeBitArray(BitArray o1, BitArray o2)
    {
        if (o1.Count != o2.Count) throw new ArgumentException("Count of BitArrays must be the same");

        BitArray o3 = new BitArray(o1.Length);
        
        for (int i = 0; i < o1.Count; i++)
        {
            if (o1[i] || o2[i])
            {
                o3[i] = true;
            }
            else o3[i] = false;
        }

        return o3;
    }
}