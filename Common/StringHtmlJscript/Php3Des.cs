using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class Php3Des
 {
    private static byte[] btev2 = ASCIIEncoding.UTF8.GetBytes("c89dkh3e");

 public static string Encrypt3DES(string a_strString, string a_strKey)
 {
 TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
 byte[] bte = Convert.FromBase64String(a_strKey);
 DES.Key = bte;
 DES.IV = btev2;
 DES.Mode = CipherMode.CBC;
 DES.Padding = System.Security.Cryptography.PaddingMode.Zeros;

 ICryptoTransform DESEncrypt = DES.CreateEncryptor();

 byte[] Buffer = ASCIIEncoding.UTF8.GetBytes(a_strString);
 string x64 = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));


 return x64;
 }



 public static string Decrypt3DES(string a_strString, string a_strKey)
 {
 TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();


 DES.Key = Convert.FromBase64String(a_strKey);
 DES.IV = btev2;
 DES.Mode = CipherMode.CBC;
 DES.Padding = System.Security.Cryptography.PaddingMode.Zeros;



 ICryptoTransform DESDecrypt = DES.CreateDecryptor();

 string result = "";
 try
 {
 byte[] Buffer = Convert.FromBase64String(a_strString);
 result = ASCIIEncoding.UTF7.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0,
Buffer.Length));
 }
 catch (Exception e)
 {

 }
 result = result.Replace("\0", "");

 return result;
 }
}
