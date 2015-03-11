using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Common.FileStreamEncode
{
    /// <summary>
    /// 编码转换
    /// </summary>
    public class CodeTrans
    {
        /// <summary>
        /// 解密base64图片
        /// </summary>
        /// <param name="base64string"></param>
        /// <returns></returns>
        public Bitmap GetImageFromBase64(string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        /// <summary>
        /// base64加密图片
        /// </summary>
        /// <param name="imagefile">图片路径</param>
        /// <param name="FileExt">图片尾缀</param>
        /// <returns></returns>
        public string GetBase64FromImage(string Imagefile, string FileExt = "jpg")
        {
            string strbaser64 = "";
            try
            {
                Bitmap bmp = new Bitmap(Imagefile);
                MemoryStream ms = new MemoryStream();
                if (FileExt.Equals("png"))
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                else if (FileExt.Equals("gif"))
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                }
                else
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                //throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }

    }
}
