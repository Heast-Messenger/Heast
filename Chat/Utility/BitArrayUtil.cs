using System.Collections;

namespace Chat.Utility;

public static class BitArrayUtil
{
    public static BitArray CascadeBitArrayList(IList<BitArray> bits)
    {
        var e = new BitArray(bits[0].Length);

        return bits.Aggregate(e, CascadeBitArray);
    }
    
    public static BitArray CascadeBitArray(BitArray o1, BitArray o2)
    {
        if (o1.Count != o2.Count)
            throw new ArgumentException("Count of BitArrays must be the same");

        var o3 = new BitArray(o1.Length);
        
        for (var i = 0; i < o1.Count; i++)
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