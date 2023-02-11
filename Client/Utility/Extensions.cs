using System.Linq;

namespace Client.Utility; 

public static class Extensions {
    public static string Repeat(this string s, int n) {
        return string.Concat(Enumerable.Repeat(s, n));
    }
}