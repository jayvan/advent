using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Advent05 {
  public static void Main () {
    string doorId = Console.ReadLine();
    Console.Write("Password: ");
    Regex leadingZeroes = new Regex(@"^0{5}");
    int i = 0;
    int found = 0;

    using (MD5 md5 = MD5.Create()) {
      while (found < 8) {
        byte[] hashInput = Encoding.ASCII.GetBytes(doorId + i);
        string hash = BitConverter.ToString(md5.ComputeHash(hashInput)).Replace("-","");

        if (leadingZeroes.Match(hash).Success) {
          Console.Write(hash[5]);
          found++;
        }

        i++;
      }
    }

    Console.Write("\n");
  }
}
