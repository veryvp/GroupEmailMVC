using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
/// <summary>
///check 的摘要说明
/// </summary>
/// 
namespace Common.Base
{
    public class Check
    {
        public Check()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>邮箱验证，传入字符串，返回true表示是正确的</summary>
        public bool IsEmail(string alt_email)
        {
            int i=alt_email.Split('@').Length-1;
            if (i != 1) { return false; }

         
            // 返回true表示是邮箱
            //return Regex.IsMatch(alt_email, @"/^[^@]+@[^@]+\.[^@]+$/.test(alt_email) && alt_email.length < 128");
            try
            {


                if (Regex.IsMatch(alt_email.Substring(0,alt_email.IndexOf("@")), @"[^A-Za-z0-9_\.\-\+]"))
                    return false;

                return Regex.IsMatch(alt_email, @"^[A-Za-z0-9](([_\.\-\+]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");


                          

            }
            catch (Exception ce)
            {
                return false;
            }

            return false;
            //return Regex.IsMatch(alt_email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        }

        /// <summary>手机格式验证，传入字符串，返回true表示是正确的</summary>
        public bool IsMobile(string alt_email)
        {
            return Regex.IsMatch(alt_email, @"^0{0,1}(130|131|132|133|134|135|136|137|138|139|145|150|151|152|153|154|155|156|157|158|159|180|181|182|183|185|186|187|188|189|147)-{0,1}[0-9]{8}$");
        }

        /// <summary>固定电话格式验证，传入字符串，返回true表示是正确的</summary>
        public bool IsTel(string alt_email)
        {

            return Regex.IsMatch(alt_email, @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^[0-9]{10,12}$)");

        }



        /// <summary>固定电话或者手机格式验证，传入字符串，返回true表示是符合两者中的一个</summary>
        public bool IsTelorMobile(string alt_tel)
        {
            bool a = IsTel(alt_tel);
            if (a == false)
            {
                a = IsMobile(alt_tel);
            }
            return a;
        }

        /// <summary>QQ验证，传入字符串，返回true表示符合</summary>
        public bool IsQQ(string alt_email)
        {
            return Regex.IsMatch(alt_email, @"(^[1-9]{1}\d{4,11}$)");
        }

        /// <summary>MSN验证，传入字符串，返回true表示符合</summary>
        public bool IsMsn(string alt_email)
        {
            return Regex.IsMatch(alt_email, @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(^[1-9]{1}\d{4,11}$)|(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)");
        }

        /// <summary>身份证验证，传入字符串，返回true表示符合</summary>
        public bool IsIDCard(string alt_email)
        {
            return Regex.IsMatch(alt_email, @"^(([0-9]{14}[x0-9]{1})|([0-9]{17}[xX0-9]{1}))$");
        }


        /// <summary>邮编验证，传入字符串，返回true表示是正确的</summary>
        public bool IsPostCard(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
            //return (/^\d{6}$/.test(alt_post));
        }


        /// <summary>日期（2003-12-05）验证，传入字符串，返回true表示符合</summary>
        public bool IsDate(string alt_email)
        {
            return Regex.IsMatch(alt_email, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        /// <summary>时间(15:00:00)验证，传入字符串，返回true表示符合</summary>
        public bool IsTime(string alt_email)
        {
            return Regex.IsMatch(alt_email, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>时间(2011-8-17 15:28:24)验证，传入字符串，返回true表示符合</summary>
        public bool IsDateTime(string StrSource)
        {
            //return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
            bool b = false;
            try
            {
                DateTime t = Convert.ToDateTime(StrSource);
                b = true;

            }
            catch (Exception)
            {

            }
            return b;

        }





        /// <summary>用户名验证，输入只能是数字，小数点，大小写中英文，“_”。返回true表示是正确的</summary>
        public bool IsName(string str_postalcode)
        {
            return Regex.IsMatch(str_postalcode, @"(^[A-Za-z0-9._]+$)");
        }

        ///  <summary>不能输入中文。返回true表示是正确的</summary>
        public bool IsNotChinese(string str_postalcode)
        {
            return Regex.IsMatch(str_postalcode, @"(^(([a-zA-Z]+[\d]*)|([\d]*[a-zA-Z]+))[\w.,#%& ]*$)");
        }

        /// <summary>只能输入中文名或英文名。返回true表示是正确的</summary>
        public bool IsChineseOrEnglIsh(string str_postalcode)
        {
            return Regex.IsMatch(str_postalcode, @"(^[A-Za-z\u4e00-\u9fa5]+$)");
        }


        /// <summary>只能输入中文名或英文名或数字。返回true表示是正确的</summary>
        public bool IsChineseOrEnglIshOrNum(string str_postalcode)
        {
            return Regex.IsMatch(str_postalcode, @"(^[0-9A-Za-z\u4e00-\u9fa5]+$)");

        }

        /// <summary>任意类型数字</summary>
        public bool IsMatch(string str_postalcode)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str_postalcode);
        }


        /// <summary>
        /// 判断一个字符串是否为合法数字，并且有指定位数的小数
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="precIsion">整数位数</param>
        /// <param name="scale">小数位数</param>
        /// <returns></returns>
        public bool IsMatch(string s, int num)
        {
            if (!IsMatch(s))
            {
                return false;
            }
            int n1 = s.IndexOf(".");
            if (n1 < 0) { return false; }
            if (n1 > 0)
            {
                string s1 = s.Substring(n1);
                if (s1.Length != num)
                {
                    return false;
                }
            }
            return true;
        }



        /// <summary>整数</summary>
        public bool IsInt(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^-?\d+$");
        }
        /// <summary>匹配正整数</summary>
        public bool IsIntZheng(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^[0-9]*[1-9][0-9]*$");
        }

        /// <summary>匹配非负整数（正整数 + 0） </summary>
        public bool IsIntFeiFu(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d+$");
        }


        /// <summary>非正整数（负整数 + 0） </summary>
        public bool IsIntFeiZheng(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^((-\d+)|(0+))$");
        }

        /// <summary>负整数</summary>
        public bool IsIntFu(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^-[0-9]*[1-9][0-9]*$");
        }

        /// <summary>浮点数</summary>
        public bool IsNum(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^(-?\d+)(\.\d+)?$");
        }

        /// <summary>正浮点数 </summary>
        public bool IsNumZheng(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
        }

        /// <summary>非负浮点数（正浮点数 + 0）</summary>
        public bool IsNumFeiFu(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d+(\.\d+)?$");
        }

        /// <summary>负浮点数</summary>
        public bool IsNumFu(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$");
        }

        /// <summary>非正浮点数（负浮点数 + 0） </summary>
        public bool IsNumFeiZheng(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$");
        }




        public bool IsSFZ(string str_postalcode)
        {

            if (str_postalcode.Length == 18)
            {

                bool check = CheckIDCard18(str_postalcode);

                return check;

            }

            else if (str_postalcode.Length == 15)
            {

                bool check = CheckIDCard15(str_postalcode);

                return check;

            }

            else
            {

                return false;

            }

        }



        private bool CheckIDCard18(string Id)
        {

            long n = 0;

            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {

                return false;//数字验证

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {

                return false;//省份验证

            }

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {

                return false;//生日验证

            }

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');

            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

            char[] Ai = Id.Remove(17).ToCharArray();

            int sum = 0;

            for (int i = 0; i < 17; i++)
            {

                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            }

            int y = -1;

            Math.DivRem(sum, 11, out y);

            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {

                return false;//校验码验证

            }

            return true;//符合GB11643-1999标准

        }



        private bool CheckIDCard15(string Id)
        {

            long n = 0;

            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {

                return false;//数字验证

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {

                return false;//省份验证

            }

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {

                return false;//生日验证

            }

            return true;//符合15位身份证标准

        }
    }
}

