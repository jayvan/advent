using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Advent142 {
  private static string salt;
  private static Dictionary<int, string> hashCache = new Dictionary<int, string>();
  private static MD5 md5;

  private static string GetHash(int index) {
    if (hashCache.ContainsKey(index)) {
      return hashCache[index];
    }

    string hash = salt + index;

    for (int i = 0; i < 2017; i++) {
      byte[] hashInput = Encoding.ASCII.GetBytes(hash);
      hash = BitConverter.ToString(md5.ComputeHash(hashInput)).Replace("-","");
      hash = hash.ToLower();
    }

    hashCache.Add(index, hash);
    return hash;
  }

  public static void Main () {
    var triplet = new Regex(@"(.)\1\1");
    md5 = MD5.Create();

    salt = Console.ReadLine();

    int keysFound = 0;;
    int index = -1;

    while (keysFound < 64) {
      index++;
      string hash = GetHash(index);
      Match match = triplet.Match(hash);

      if (match.Success) {
        char letter = match.Groups[0].Value[0];
        var fiveOfThem = new Regex(new String(letter, 5));

        for (int j = index + 1; j <= index + 1000; j++) {
          if (fiveOfThem.IsMatch(GetHash(j))) {
            Console.Write('.');
            keysFound++;
            break;
          }
        }
      }

      hashCache.Remove(index);
    }

    Console.WriteLine("\n" + index);
  }
}
