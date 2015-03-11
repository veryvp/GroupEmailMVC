using System;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using System.Web.Security;
using System.Net;
using System.Collections;

public class StrHelper
{
    private static string passWord;	//�����ַ���

    /// <summary>
    /// �ж������Ƿ�����
    /// </summary>
    /// <param name="num">Ҫ�жϵ��ַ���</param>
    /// <returns></returns>
    static public bool VldInt(string num)
    {
        #region
        int ResultNum;
        return int.TryParse(num, out ResultNum);
        #endregion
    }
    public static string FilterDisplayMsgChar(string str)
    {
        // This item is obfuscated and can not be translated.
        
        str = str.Replace(@"\", @"\\");
        str = str.Replace("\"", "\\\"");
        return str;
    }


    public static double FixToDouble(object value)
    {
        // This item is obfuscated and can not be translated.
        double dbvalue;
        dbvalue = Convert.ToDouble(value);
        return dbvalue;
             
    }

 

 



    public static DateTime FixToDateTime(object value)
    {
        // This item is obfuscated and can not be translated.
        return Convert.ToDateTime(value.ToString());
    }


    public static decimal FixToDec(object value)
    {
        // This item is obfuscated and can not be translated.
        return Convert.ToDecimal(value.ToString());
    }

 

 

 


    public static int FixToInt(string value)
    {
        // This item is obfuscated and can not be translated.
        int result = 0;
        int.TryParse(value, out result);
        return result;
    }

    //public static int FixToInt(string value)
    //{
    //    // This item is obfuscated and can not be translated.
        
    //    int result = 0;
    //    int.TryParse(value, out result);
    //    return result;
    //}


    public static int FixToInt(object value)
    {
        // This item is obfuscated and can not be translated.
        int result = 0;
        //int.TryParse(value, out result);
        return result;
    }

 

 

 


 


    /// <summary>
    /// �����ı��༭���滻����ַ���
    /// </summary>
    /// <param name="str">Ҫ�滻���ַ���</param>
    /// <returns></returns>
    static public string GetHtmlEditReplace(string str)
    {
        #region
        return str.Replace("'", "��").Replace("&nbsp;", " ").Replace(",", "��").Replace("%", "��").
            Replace("script", "").Replace(".js", "");
        #endregion
    }

    /// <summary>
    /// ��ȡ�ַ�������
    /// </summary>
    /// <param name="str">��Ҫ��ȡ���ַ���</param>
    /// <param name="num">��ȡ�ַ����ĳ���</param>
    /// <returns></returns>
    static public string GetSubString(string str, int num)
    {
        #region
        return (str.Length > num) ? str.Substring(0, num) + "..." : str;
        #endregion
    }

    /// <summary>
    /// ��ȡ�ַ����Ż���
    /// </summary>
    /// <param name="stringToSub">��Ҫ��ȡ���ַ���</param>
    /// <param name="length">��ȡ�ַ����ĳ���</param>
    /// <returns></returns>
    public static string GetFirstString(string stringToSub, int length)
    {
        #region
        Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
        char[] stringChar = stringToSub.ToCharArray();
        StringBuilder sb = new StringBuilder();
        int nLength = 0;
        bool isCut = false;
        for (int i = 0; i < stringChar.Length; i++)
        {
            if (regex.IsMatch((stringChar[i]).ToString()))
            {
                sb.Append(stringChar[i]);
                nLength += 2;
            }
            else
            {
                sb.Append(stringChar[i]);
                nLength = nLength + 1;
            }

            if (nLength > length)
            {
                isCut = true;
                break;
            }
        }
        if (isCut)
            return sb.ToString() + "..";
        else
            return sb.ToString();
        #endregion
    }

    /// <summary>
    /// ����������Ϣ
    /// </summary>
    /// <param name="text">����</param>
    /// <param name="maxLength">��󳤶�</param>
    /// <returns></returns>
    public static string InputText(string text, int maxLength)
    {
        #region
        text = text.Trim();
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        if (text.Length > maxLength)
            text = text.Substring(0, maxLength);
        text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
        text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
        text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
        text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
        text = text.Replace("'", "''");
        return text;
        #endregion
    }

    /// <summary>
    /// ���������
    /// </summary>
    /// <returns></returns>
    private string GenerateCheckCode()
    {
        #region
        int number;
        char code;
        string checkCode = String.Empty;

        System.Random random = new Random();

        for (int i = 0; i < 5; i++)
        {
            number = random.Next();

            if (number % 2 == 0)
                code = (char)('0' + (char)(number % 10));
            else
                code = (char)('A' + (char)(number % 26));

            checkCode += code.ToString();
        }

        HttpContext.Current.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));

        return checkCode;
        #endregion
    }


    /// <summary>
    /// ������֤��ͼƬ
    /// </summary>
    public void CreateCheckCodeImage()
    {
        #region
        string checkCode = GenerateCheckCode();
        if (checkCode == null || checkCode.Trim() == String.Empty)
            return;

        System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
        Graphics g = Graphics.FromImage(image);

        try
        {
            //�������������
            Random random = new Random();

            //���ͼƬ����ɫ
            g.Clear(Color.White);

            //��ͼƬ�ı���������
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(checkCode, font, brush, 2, 2);

            //��ͼƬ��ǰ��������
            for (int i = 0; i < 150; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            //��ͼƬ�ı߿���
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
        #endregion
    }

    #region ����ָ��λ�������
    private static char[] constant =   
          {   
            '0','1','2','3','4','5','6','7','8','9',   
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
          };

    public static string GenerateRandom(int Length)
    {
        System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
        Random rd = new Random();
        for (int i = 0; i < Length; i++)
        {
            newRandom.Append(constant[rd.Next(62)]);
        }
        return newRandom.ToString();
    }

    public static string GetNumRandom(int Length)
    {
        System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
        char[] NumStr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        Random rd = new Random();
        for (int i = 0; i < Length; i++)
        {
            newRandom.Append(constant[rd.Next(10)]);
        }
        return newRandom.ToString();
    }
    #endregion

    /// <summary>
    /// ��ȡ���ֵ�һ��ƴ��
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    static public string getSpells(string input)
    {
        #region
        int len = input.Length;
        string reVal = "";
        for (int i = 0; i < len; i++)
        {
            reVal += getSpell(input.Substring(i, 1));
        }
        return reVal;
        #endregion
    }

    static public string getSpell(string cn)
    {
        #region
        byte[] arrCN = Encoding.Default.GetBytes(cn);
        if (arrCN.Length > 1)
        {
            int area = (short)arrCN[0];
            int pos = (short)arrCN[1];
            int code = (area << 8) + pos;
            int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
            for (int i = 0; i < 26; i++)
            {
                int max = 55290;
                if (i != 25) max = areacode[i + 1];
                if (areacode[i] <= code && code < max)
                {
                    return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                }
            }
            return "?";
        }
        else return cn;
        #endregion
    }


    /// <summary>
    /// ���תȫ��
    /// </summary>
    /// <param name="BJstr"></param>
    /// <returns></returns>
    static public string GetQuanJiao(string BJstr)
    {
        #region
        char[] c = BJstr.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
            if (b.Length == 2)
            {
                if (b[1] == 0)
                {
                    b[0] = (byte)(b[0] - 32);
                    b[1] = 255;
                    c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                }
            }
        }

        string strNew = new string(c);
        return strNew;

        #endregion
    }

    /// <summary>
    /// ȫ��ת���
    /// </summary>
    /// <param name="QJstr"></param>
    /// <returns></returns>
    static public string GetBanJiao(string QJstr)
    {
        #region
        char[] c = QJstr.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
            if (b.Length == 2)
            {
                if (b[1] == 255)
                {
                    b[0] = (byte)(b[0] + 32);
                    b[1] = 0;
                    c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                }
            }
        }
        string strNew = new string(c);
        return strNew;
        #endregion
    }

    #region ���ܵ�����
    /// <summary>
    /// ���ܵ�����
    /// </summary>
    public enum PasswordType
    {
        SHA1,
        MD5
    }
    #endregion


    /// <summary>
    /// �ַ�������
    /// </summary>
    /// <param name="PasswordString">Ҫ���ܵ��ַ���</param>
    /// <param name="PasswordFormat">Ҫ���ܵ����</param>
    /// <returns></returns>
    static public string EncryptPassword(string PasswordString, PasswordType PasswordFormat)
    {
        #region
        switch (PasswordFormat)
        {
            case PasswordType.SHA1:
                {
                    passWord = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "SHA1");
                    break;
                }
            case PasswordType.MD5:
                {
                    passWord = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "MD5").Substring(8, 16).ToLower();
                    break;
                }
            default:
                {
                    passWord = string.Empty;
                    break;
                }
        }
        return passWord;
        #endregion
    }

    /// <summary>
    /// �ַ���ת��Ϊ html
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string StringToHtml(string str)
    {
        #region
        str = str.Replace("&", "&amp;");
        str = str.Replace(" ", "&nbsp;");
        str = str.Replace("'", "''");
        str = str.Replace("\"", "&quot;");
        str = str.Replace(" ", "&nbsp;");
        str = str.Replace("<", "&lt;");
        str = str.Replace(">", "&gt;");
        str = str.Replace("\r\n", "<br>");

        return str;
        #endregion
    }

    /// <summary>
    /// htmlת�����ַ���
    /// </summary>
    /// <param name="strHtml"></param>
    /// <returns></returns>
    public static string HtmlToString(string strHtml)
    {
        #region
        strHtml = strHtml.Replace("<br>", "\r\n");
        strHtml = strHtml.Replace(@"<br />", "\r\n");
        strHtml = strHtml.Replace(@"<br/>", "\r\n");
        strHtml = strHtml.Replace("&gt;", ">");
        strHtml = strHtml.Replace("&lt;", "<");
        strHtml = strHtml.Replace("&nbsp;", " ");
        strHtml = strHtml.Replace("&quot;", "\"");

        strHtml = Regex.Replace(strHtml, @"<\/?[^>]+>", "", RegexOptions.IgnoreCase);

        return strHtml;
        #endregion
    }

    /// <summary>
    /// ����������ڱ�ʾ��ʽ
    /// </summary>
    /// <returns></returns>
    public static string GetChineseWeek(DateTime t)
    {
        #region
        string week = "";

        switch (t.DayOfWeek)
        {
            case DayOfWeek.Monday:
                week = "һ";
                break;
            case DayOfWeek.Tuesday:
                week = "��";
                break;
            case DayOfWeek.Wednesday:
                week = "��";
                break;
            case DayOfWeek.Thursday:
                week = "��";
                break;
            case DayOfWeek.Friday:
                week = "��";
                break;
            case DayOfWeek.Saturday:
                week = "��";
                break;
            case DayOfWeek.Sunday:
                week = "��";
                break;
        }

        return "����" + week;
        #endregion
    }

    /// <summary>
    /// �������������
    /// </summary>
    /// <returns></returns>
    public static string GetRamCode()
    {
        #region
        return DateTime.Now.ToString("yyyyMMddHHmmssff");
        #endregion
    }

    /// <summary>
    /// ����ָ�����ȵ��ַ���,������strLong��str�ַ���
    /// </summary>
    /// <param name="strLong">���ɵĳ���</param>
    /// <param name="str">��str�����ַ���</param>
    /// <returns></returns>
    public static string StringOfChar(int strLong, string str)
    {
        #region
        string ReturnStr = string.Empty;
        for (int i = 0; i < strLong; i++)
        {
            ReturnStr += str;
        }
        return ReturnStr;
        #endregion
    }

    public static string FilterStr(string str)
    {

        return str.Replace("'", " ").Replace(".", "").Replace("\r\n", "��");
    }

    /// <summary>
    /// ���������룺�磺bar_code("20070520122334", 20, 1, 1);
    /// </summary>
    /// <param name="str"></param>
    /// <param name="ch">�ȶ�</param>
    /// <param name="cw">�������</param>
    /// <param name="type_code">�Ƿ��������1Ϊ���</param>
    /// <returns></returns>
    public static string BarCode(object str, int ch, int cw, int type_code)
    {
        #region
        string strTmp = str.ToString();
        string code = strTmp;
        strTmp = strTmp.ToLower();
        int height = ch;
        int width = cw;

        strTmp = strTmp.Replace("0", "_|_|__||_||_|"); ;
        strTmp = strTmp.Replace("1", "_||_|__|_|_||");
        strTmp = strTmp.Replace("2", "_|_||__|_|_||");
        strTmp = strTmp.Replace("3", "_||_||__|_|_|");
        strTmp = strTmp.Replace("4", "_|_|__||_|_||");
        strTmp = strTmp.Replace("5", "_||_|__||_|_|");
        strTmp = strTmp.Replace("7", "_|_|__|_||_||");
        strTmp = strTmp.Replace("6", "_|_||__||_|_|");
        strTmp = strTmp.Replace("8", "_||_|__|_||_|");
        strTmp = strTmp.Replace("9", "_|_||__|_||_|");
        strTmp = strTmp.Replace("a", "_||_|_|__|_||");
        strTmp = strTmp.Replace("b", "_|_||_|__|_||");
        strTmp = strTmp.Replace("c", "_||_||_|__|_|");
        strTmp = strTmp.Replace("d", "_|_|_||__|_||");
        strTmp = strTmp.Replace("e", "_||_|_||__|_|");
        strTmp = strTmp.Replace("f", "_|_||_||__|_|");
        strTmp = strTmp.Replace("g", "_|_|_|__||_||");
        strTmp = strTmp.Replace("h", "_||_|_|__||_|");
        strTmp = strTmp.Replace("i", "_|_||_|__||_|");
        strTmp = strTmp.Replace("j", "_|_|_||__||_|");
        strTmp = strTmp.Replace("k", "_||_|_|_|__||");
        strTmp = strTmp.Replace("l", "_|_||_|_|__||");
        strTmp = strTmp.Replace("m", "_||_||_|_|__|");
        strTmp = strTmp.Replace("n", "_|_|_||_|__||");
        strTmp = strTmp.Replace("o", "_||_|_||_|__|");
        strTmp = strTmp.Replace("p", "_|_||_||_|__|");
        strTmp = strTmp.Replace("r", "_||_|_|_||__|");
        strTmp = strTmp.Replace("q", "_|_|_|_||__||");
        strTmp = strTmp.Replace("s", "_|_||_|_||__|");
        strTmp = strTmp.Replace("t", "_|_|_||_||__|");
        strTmp = strTmp.Replace("u", "_||__|_|_|_||");
        strTmp = strTmp.Replace("v", "_|__||_|_|_||");
        strTmp = strTmp.Replace("w", "_||__||_|_|_|");
        strTmp = strTmp.Replace("x", "_|__|_||_|_||");
        strTmp = strTmp.Replace("y", "_||__|_||_|_|");
        strTmp = strTmp.Replace("z", "_|__||_||_|_|");
        strTmp = strTmp.Replace("-", "_|__|_|_||_||");
        strTmp = strTmp.Replace("*", "_|__|_||_||_|");
        strTmp = strTmp.Replace("/", "_|__|__|_|__|");
        strTmp = strTmp.Replace("%", "_|_|__|__|__|");
        strTmp = strTmp.Replace("+", "_|__|_|__|__|");
        strTmp = strTmp.Replace(".", "_||__|_|_||_|");
        strTmp = strTmp.Replace("_", "<span   style='height:" + height + ";width:" + width + ";background:#FFFFFF;'></span>");
        strTmp = strTmp.Replace("|", "<span   style='height:" + height + ";width:" + width + ";background:#000000;'></span>");

        if (type_code == 1)
        {
            return strTmp + "<BR>" + code;
        }
        else
        {
            return strTmp;
        }
        #endregion
    }


    public static string ClearHtml(string HtmlString)
    {
        string pn = "(</?.*?/?>)";
        HtmlString = Regex.Replace(HtmlString, pn, "");
        return HtmlString;
    }

    public static string ClearFormat(string HtmlString)
    {
        HtmlString = HtmlString.Replace("\r\n", string.Empty).Replace(" ", string.Empty);
        return HtmlString.Trim();
    }

    #region ��ȡN������
    /// <summary>
    /// ȡָ����������
    /// </summary>
    /// <param name="str">����Ĵ�ȡ�ַ���</param>
    /// <param name="rowsnum">ָ��������</param>
    /// <param name="strnum">ÿ�е�Ӣ���ַ������ֽ���</param>
    /// <returns></returns>
    public static string GetContent(string str, int rowsnum, int strnum)
    {
        //1�������ݿ�           
        string content = str.Replace("\r\n", "��");
        string[] strContent = content.Split(Convert.ToChar("��"));

        int strCount = rowsnum * strnum;
        int cutrow = rowsnum - strContent.Length;
        cutrow = rowsnum > 10 ? rowsnum : 10;
        int pStrCount;
        string setOkStr = "";


        //2�����ݿ����
        for (int i = 0; i < strContent.Length; i++)
        {
            pStrCount = System.Text.Encoding.Default.GetBytes(strContent[i]).Length;
            if (pStrCount < strCount)
            {
                setOkStr += strContent[i] + "<br>";
                rowsnum -= Convert.ToInt32(Math.Ceiling((double)pStrCount / (double)strnum));
                strCount = rowsnum * strnum;
            }
            else
            {
                if (rowsnum > 0)
                {
                    setOkStr += CutStr(strContent[i], rowsnum * strnum, cutrow);
                }
                else
                {
                    //��ȥrowsnum��Ϊ�˱�����Щ�г���Ϊ90,�е�Ϊ89������
                    setOkStr = setOkStr.Substring(0, setOkStr.Length - cutrow / 2) + "...";
                }
                break;
            }
        }

        setOkStr = setOkStr.Replace("  ", "��"); //����ǣ��ո�תӲ��ȫ��)�ո�
        return setOkStr;

    }

    //�ַ�����ȡ����
    public static string CutStr(string str, int length, int rowsnum)
    {
        if (System.Text.Encoding.Default.GetBytes(str).Length < length)
            return str;

        length = length - rowsnum;
        int i = 0, j = 0;
        foreach (char chr in str)
        {
            if ((int)chr > 127)
                i += 2;
            else
                i++;
            if (i > length)
            {
                str = str.Substring(0, j) + "...";
                break;
            }
            j++;
        }
        return str;

    }
    #endregion

    #region �õ�����
    public static int ConvertToInt(string Str)
    {
        return Str.Trim() == string.Empty ? 0 : int.Parse(Str);
    }

    public static int GetInt(object o)
    {
        #region
        if (o == DBNull.Value || o == null)
            return 0;
        else
            return Convert.ToInt32(o);
        #endregion
    }
    #endregion

    /// <summary>
    /// ת����ʱ������
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static object ConvertDate(object o)
    {
        DateTime dt;
        if (DateTime.TryParse(o.ToString(), out dt))
            return dt;
        else
            return DBNull.Value;
    }

    #region �պ�HTML����
    public static string CloseHTML(string str)
    {
        string[] HtmlTag = new string[] { "p", "div", "span", "table", "ul", "font", "b", "u", "i", "a", "h1", "h2", "h3", "h4", "h5", "h6" };

        for (int i = 0; i < HtmlTag.Length; i++)
        {
            int OpenNum = 0, CloseNum = 0;
            Regex re = new Regex("<" + HtmlTag + "[^>]*" + ">", RegexOptions.IgnoreCase);
            MatchCollection m = re.Matches(str);
            OpenNum = m.Count;
            re = new Regex("</" + HtmlTag + ">", RegexOptions.IgnoreCase);
            m = re.Matches(str);
            CloseNum = m.Count;

            for (int j = 0; j < OpenNum - CloseNum; j++)
            {
                str += "</" + HtmlTag + ">";
            }
        }

        return str;
    }
    #endregion

    /// <summary>
    /// �õ�192.248.23.*��IP
    /// </summary>
    /// <param name="Str">IP��ַ</param>
    /// <returns></returns>
    public static string GetSortIp(string Str)
    {
        int x = Str.LastIndexOf('.') - 1;
        return Str.Substring(0, x) + "*.*";
    }

    /// <summary>
    /// ���������������ȡ��ʵIP
    /// </summary>
    /// <returns></returns>
    public string AcceptTrueIP()
    {
        string user_IP = null;
        if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
        {
            user_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else
        {
            user_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        return user_IP;
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns></returns>
    public static string GetYearMonth()
    {
        return DateTime.Now.ToString("yyyyMM");
    }

    #region ��ȡԶ��ҳ������
    public static string GetHttpData(string Url)
    {
        //string sException = null;
        string sRslt = null;
        WebResponse oWebRps = null;
        WebRequest oWebRqst = WebRequest.Create(Url);
        oWebRqst.Timeout = 50000;
        try
        {
            oWebRps = oWebRqst.GetResponse();
        }
        catch (WebException e)
        {
            //sException = e.Message.ToString();
            //Response.Write(sException);
        }
        catch (Exception e)
        {
            //sException = e.ToString();
            //Response.Write(sException);
        }
        finally
        {
            if (oWebRps != null)
            {
                StreamReader oStreamRd = new StreamReader(oWebRps.GetResponseStream(), Encoding.GetEncoding("UTF-8"));//GB2312|UTF-8"
                sRslt = oStreamRd.ReadToEnd();
                oStreamRd.Close();
                oWebRps.Close();
            }
        }
        return sRslt;
    }

    public string[] GetData(string Html)
    {
        String[] rS = new String[2];
        string s = Html;
        s = Regex.Replace(s, "\\s{3,}", "");
        s = s.Replace("\r", "");
        s = s.Replace("\n", "");
        string Pat = "<td align=\"center\" class=\"24p\"><B>(.*)</B></td></tr><tr>.*(<table width=\"95%\" border=\"0\" cellspacing=\"0\" cellpadding=\"10\">.*</table>)<table width=\"98%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">(.*)<td align=center class=l6h>";
        Regex Re = new Regex(Pat);
        Match Ma = Re.Match(s);
        if (Ma.Success)
        {
            rS[0] = Ma.Groups[1].ToString();
            rS[1] = Ma.Groups[2].ToString();
            //pgStr = Ma.Groups[3].ToString();
        }
        return rS;
    }
    #endregion

    /// <summary>
    /// �ж�ҳ���Ƿ����
    /// </summary>
    /// <param name="sURL"></param>
    /// <returns></returns>
    public static bool UrlExist(string sURL)
    {
        #region
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            //WebProxy   proxy   =   new   WebProxy("your   proxy   server",   8080);   
            //request.Proxy   =   proxy;   
            request.Method = "HEAD";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            bool result = false;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result = true;
                    break;
                case HttpStatusCode.Moved:
                    break;
                case HttpStatusCode.NotFound:
                    break;
            }
            response.Close();
            return result;
        }
        catch
        {
            return false;
        }
        #endregion
    }

    #region ��ȡ�ִ��е�����
    /// <summary>
    /// ��ȡ�ִ��е�����
    /// </summary>
    /// <param name="HtmlCode"></param>
    /// <returns></returns>
    public static ArrayList GetPageUrl(string HtmlCode)
    {
        ArrayList my_list = new ArrayList();
        string p = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        Regex re = new Regex(p, RegexOptions.IgnoreCase);
        MatchCollection mc = re.Matches(HtmlCode);

        for (int i = 0; i <= mc.Count - 1; i++)
        {
            string name = mc[i].ToString();
            if (!my_list.Contains(name))//�ų��ظ�URL
            {
                my_list.Add(name);
            }
        }
        return my_list;
    }
    #endregion


    /// <summary>
    /// �� Stream ת���� string
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ConvertStreamToString(Stream s)
    {
        #region
        string strResult = "";
        StreamReader sr = new StreamReader(s, Encoding.UTF8);

        Char[] read = new Char[256];

        // Read 256 charcters at a time.    
        int count = sr.Read(read, 0, 256);

        while (count > 0)
        {
            // Dump the 256 characters on a string and display the string onto the console.
            string str = new String(read, 0, count);
            strResult += str;
            count = sr.Read(read, 0, 256);
        }


        // �ͷ���Դ
        sr.Close();

        return strResult;
        #endregion
    }

    /// <summary>
    /// �Դ��ݵĲ����ַ������д�����ֹע��ʽ����
    /// </summary>
    /// <param name="str">���ݵĲ����ַ���</param>
    /// <returns>String</returns>
    public static string ConvertSql(string str)
    {
        #region
        str = str.Trim();
        str = str.Replace("'", "''");
        str = str.Replace(";--", "");
        str = str.Replace("=", "");
        str = str.Replace(" or ", "");
        str = str.Replace(" and ", "");

        return str;
        #endregion
    }



    /// <summary>
    /// �Դ��ݵĲ����ַ������д�����ֹע��ʽ����
    /// </summary>
    /// <param name="str">���ݵĲ����ַ���</param>
    /// <returns>String</returns>
    public static string ConvertSql2(string str)
    {
        #region
        str = str.Trim();
        // str = str.Replace("'", "''");
        str = str.Replace(";--", "");
        str = str.Replace("=", "");
        str = str.Replace(" or ", "");
        str = str.Replace(" and ", "");

        return str;
        #endregion
    }


    /// <summary>
    /// ��ʽ��ռ�ÿռ��С�����
    /// </summary>
    /// <param name="size">��С</param>
    /// <returns>���� String</returns>
    public static string FormatNUM(long size)
    {
        #region
        decimal NUM;
        string strResult;

        if (size > 1073741824)
        {
            NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1073741824));
            strResult = NUM.ToString("N") + " M";
        }
        else if (size > 1048576)
        {
            NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1048576));
            strResult = NUM.ToString("N") + " M";
        }
        else if (size > 1024)
        {
            NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1024));
            strResult = NUM.ToString("N") + " KB";
        }
        else
        {
            strResult = size + " �ֽ�";
        }

        return strResult;
        #endregion
    }

    /// <summary>
    /// ��ȡ����Ԫ�صĺϲ��ַ���
    /// </summary>
    /// <param name="stringArray"></param>
    /// <returns></returns>
    public static string GetArrayString(string[] stringArray)
    {
        #region
        string totalString = null;
        for (int i = 0; i < stringArray.Length; i++)
        {
            totalString = totalString + stringArray[i];
        }
        return totalString;
        #endregion
    }

    /// <summary>
    /// ��ָ���ַ����еĺ���ת��Ϊƴ������ĸ����д�����зǺ��ֱ���Ϊԭ�ַ�
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ConvertSpellFirst(string text)
    {
        #region
        char pinyin;
        byte[] array;
        StringBuilder sb = new StringBuilder(text.Length);
        foreach (char c in text)
        {
            pinyin = c;
            array = Encoding.Default.GetBytes(new char[] { c });

            if (array.Length == 2)
            {
                int i = array[0] * 0x100 + array[1];

                #region ����ƥ��
                if (i < 0xB0A1) pinyin = c;
                else
                    if (i < 0xB0C5) pinyin = 'a';
                    else
                        if (i < 0xB2C1) pinyin = 'b';
                        else
                            if (i < 0xB4EE) pinyin = 'c';
                            else
                                if (i < 0xB6EA) pinyin = 'd';
                                else
                                    if (i < 0xB7A2) pinyin = 'e';
                                    else
                                        if (i < 0xB8C1) pinyin = 'f';
                                        else
                                            if (i < 0xB9FE) pinyin = 'g';
                                            else
                                                if (i < 0xBBF7) pinyin = 'h';
                                                else
                                                    if (i < 0xBFA6) pinyin = 'g';
                                                    else
                                                        if (i < 0xC0AC) pinyin = 'k';
                                                        else
                                                            if (i < 0xC2E8) pinyin = 'l';
                                                            else
                                                                if (i < 0xC4C3) pinyin = 'm';
                                                                else
                                                                    if (i < 0xC5B6) pinyin = 'n';
                                                                    else
                                                                        if (i < 0xC5BE) pinyin = 'o';
                                                                        else
                                                                            if (i < 0xC6DA) pinyin = 'p';
                                                                            else
                                                                                if (i < 0xC8BB) pinyin = 'q';
                                                                                else
                                                                                    if (i < 0xC8F6) pinyin = 'r';
                                                                                    else
                                                                                        if (i < 0xCBFA) pinyin = 's';
                                                                                        else
                                                                                            if (i < 0xCDDA) pinyin = 't';
                                                                                            else
                                                                                                if (i < 0xCEF4) pinyin = 'w';
                                                                                                else
                                                                                                    if (i < 0xD1B9) pinyin = 'x';
                                                                                                    else
                                                                                                        if (i < 0xD4D1) pinyin = 'y';
                                                                                                        else
                                                                                                            if (i < 0xD7FA) pinyin = 'z';
                #endregion
            }

            sb.Append(pinyin);
        }

        return sb.ToString();
        #endregion
    }

    //#region Request
    ///// </summary>
    ///// <param name="filedName"></param>
    ///// <param name="defaultValue"></param>
    ///// <returns></returns>
    //public static int Request(string filedName, int defaultValue)
    //{
    //    return Request(filedName, defaultValue, false);
    //}

    ///// <summary>
    ///// ��ȡURLָ��������������������ڻ���Ϊ�վͷ���Ĭ��ֵ
    ///// </summary>
    ///// <param name="filedName">����KEY</param>
    ///// <param name="defaultValue">Ĭ��ֵ</param>
    ///// <param name="isDecrypt">�Ƿ���Ҫ����</param>
    ///// <returns></returns>
    //public static int Request(string filedName, int defaultValue, bool isDecrypt)
    //{
    //    string str = HttpContext.Current.Request[filedName];
    //    if (str != null && str.Trim() != "")
    //    {
    //        if (isDecrypt)
    //        {
    //            str = new PblogDes().Decrypt(str);
    //        }
    //        return int.Parse(str);
    //    }

    //    return defaultValue;
    //}

    /// <summary>
    /// ��ȡURLָ��������������������ڻ���Ϊ�վ��׳�������Ϣ
    /// </summary>
    /// <param name="filedName"></param>
    /// <returns></returns>
    //public static int Request(string filedName)
    //{
    //    return Request(filedName, false);
    //}

    /// <summary>
    /// ��ȡURLָ��������������������ڻ���Ϊ�վ��׳�������Ϣ
    /// </summary>
    /// <param name="filedName"></param>
    /// <param name="isDecrypt">�Ƿ���Ҫ����</param>
    /// <returns></returns>
    //public static int Request(string filedName, bool isDecrypt)
    //{
    //    int i = Request(filedName, -1, isDecrypt);
    //    if (i == -1)
    //    {
    //        throw new Exception("�Ҳ���ָ��������" + filedName);
    //    }

    //    return i;
    //}
    //#endregion




    public static bool IsNumberic(string str)
    {

        bool ret=false;
        if (str == null || str.Length == 0)
            return ret;

        System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
        byte[] bytestr = ascii.GetBytes(str);
        foreach (byte c in bytestr)
        {
            if (c < 48 || c > 57)
            {
                ret= false;
            }
            ret= true;
        }

        return ret;
    }




}