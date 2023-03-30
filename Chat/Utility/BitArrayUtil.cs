using System.Collections;
using Chat.Permissionengine;

namespace Chat.Utility;

public static class BitArrayUtil
{
    public static BitArray CascadeBitArrayList(IList<BitArray> bits)
    {
        if (bits.Count == 1) return bits[0];
        var e = new BitArray(PermissionsEngine.RolePermissionMaxSize);

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

    public static bool[] ConvertToBoolArray(BitArray b)
    {
        var o3 = new bool[b.Length];
        
        for (var i = 0; i < b.Count; i++)
        {
            if (b[i])
            {
                o3[i] = true;
            }
            else o3[i] = false;
        }

        return o3;
    }
}