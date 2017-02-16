using System.Collections.Generic;
using System.Linq;

namespace Graphs {
    public class Permutation {

        public static List<int[]> getPermutations(int def) {
            List<int> a = new List<int>();
            for (int i = 0; i < def; i++) {
                a.Add(i);
            }
            return getPermutations(a.ToArray());
        }

        public static List<int[]> getPermutations(int[] def) {
            if (def.Length == 1) {
                return new List<int[]>() { def };
            }

            List<int[]> permutations = new List<int[]>();

            int toInsert = def[0];

            List<int> temp = def.ToList();
            temp.RemoveAt(0);
            int[] newdef = temp.ToArray();

            foreach (int[] item in getPermutations(newdef)) {
                for (int i = 0; i <= item.Length; i++) {
                    temp = item.ToList();
                    temp.Insert(i, toInsert);
                    int[] newPerm = temp.ToArray();
                    permutations.Add(newPerm);
                }
            }

            return permutations;
        }

    }
}
