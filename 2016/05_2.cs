using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Advent052 {
  public static void Main () {
    string doorId = Console.ReadLine();
    Regex leadingZeroes = new Regex(@"^00-00-0");
    char[] password = new char[8];
    int i = 0;
    int found = 0;

    using (MD5 md5 = MD5.Create()) {
      while (found < 8) {
        byte[] hashInput = Encoding.ASCII.GetBytes(doorId + i);
        string hash = BitConverter.ToString(md5.ComputeHash(hashInput));

        if (leadingZeroes.Match(hash).Success) {
          int position = (int)hash[7] - 48;

          if (position < 0 || position >= 8) {
            i++;
            continue;
          }

          if (password[position] == 0) {
            password[position] = hash[9];
            found++;
            Console.WriteLine("Password: " + new string(password));
          }
        }

        i++;
      }
    }
  }
}
