using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Common.StringHtmlJscript
{

	public class Denery
	{
		/// <summary>
		/// 解密方法
		/// </summary>
		/// <param name="需要解密的字符串数组，如：">"090100410109A991".toCharArray() </param>
		public static string[] denery(char[] buf)
		{
			int i, j, tempFlag, tempCount, tempFlag1;

			char[] buf1 = new char[100];
			// 解密
			for (i = 0; i < 16; i++)
			{
				buf[i] = (char)(buf[i] - 0x30);
				if (buf[i] > 9)
				{
					buf[i] = (char)(buf[i] - 7);
				}
			}

			// 第一次解密 左移+异或 取第0个，注意第0个不参与运算

			tempCount = buf[0];
			for (j = 0; j < tempCount; j++)
			{
				tempFlag = 0;
				for (i = 1; i < 16; i++)
				{
					tempFlag1 = buf[i] & 0x01;
					buf[i] = (char)(buf[i] >> 1);
					buf[i] = (char)(buf[i] + (tempFlag << 3));
					tempFlag = tempFlag1;
				}
				buf[1] = (char)(buf[1] + (tempFlag << 3));
			}
			tempFlag = buf[0];
			for (i = 1; i < 16; i++)
			{
				buf[i] = (char)(buf[i] ^ tempFlag);
			}

			// 第二次解密 左移+异或 取第12个

			tempCount = buf[0];
			for (j = 0; j < tempCount; j++)
			{
				tempFlag = 0;
				for (i = 1; i < 16; i++)
				{
					tempFlag1 = buf[i] & 0x01;
					buf[i] = (char)(buf[i] >> 1);
					buf[i] = (char)(buf[i] + (tempFlag << 3));
					tempFlag = tempFlag1;
				}
				buf[1] = (char)(buf[1] + (tempFlag << 3));
			}
			tempFlag = buf[12];
			for (i = 0; i < 16; i++)
			{
				if (i == 12)
				{
					continue;
				}
				buf[i] = (char)(buf[i] ^ tempFlag);
			}

			// 第三次解密 左移+异或 取第8个

			tempCount = buf[0];
			for (j = 0; j < tempCount; j++)
			{
				tempFlag = 0;
				for (i = 1; i < 16; i++)
				{
					tempFlag1 = buf[i] & 0x01;
					buf[i] = (char)(buf[i] >> 1);
					buf[i] = (char)(buf[i] + (tempFlag << 3));
					tempFlag = tempFlag1;
				}
				buf[1] = (char)(buf[1] + (tempFlag << 3));
			}
			tempFlag = buf[8];
			for (i = 0; i < 16; i++)
			{
				if (i == 8)
				{
					continue;
				}
				buf[i] = (char)(buf[i] ^ tempFlag);
			}

			// 第四次解密 左移+异或 取第4个

			tempCount = buf[0];
			for (j = 0; j < tempCount; j++)
			{
				tempFlag = 0;
				for (i = 1; i < 16; i++)
				{
					tempFlag1 = buf[i] & 0x01;
					buf[i] = (char)(buf[i] >> 1);
					buf[i] = (char)(buf[i] + (tempFlag << 3));
					tempFlag = tempFlag1;
				}
				buf[1] = (char)(buf[1] + (tempFlag << 3));
			}
			tempFlag = buf[4];
			for (i = 0; i < 16; i++)
			{
				if (i == 4)
				{
					continue;
				}
				buf[i] = (char)(buf[i] ^ tempFlag);
			}

			for (i = 0; i < 16; i++)
			{
				buf[i] = (char)(buf[i] + 0x30);
				if (buf[i] > 0x39)
				{
					buf[i] = (char)(buf[i] + 7);
				}
			}
			// System.out.println("userCode:"+new String(buf));
			buf1[0] = buf[3];
			buf1[1] = buf[7];
			buf1[2] = buf[11];
			buf1[3] = buf[15];
			buf1[4] = (char)0;
			string userCode = new string(buf1);
			// fileRepro.WriteString("userCode:");
			// fileRepro.WriteString(buf1);
			// System.out.println("userCode:"+userCode);
			// fileRepro.WriteString(" userName:");
			buf1[0] = buf[1];
			buf1[1] = buf[5];
			buf1[2] = buf[9];
			buf1[3] = buf[13];
			// fileRepro.WriteString(buf1);
			string userName = new string(buf1);
			buf1[0] = buf[0];
			buf1[1] = buf[4];
			buf1[2] = buf[8];
			buf1[3] = buf[12];
			// fileRepro.WriteString(buf1);
			string random = new string(buf1);
			// System.out.println("userName:"+userName);
			// fileRepro.WriteString(" plugCount:");
			buf1[0] = buf[2];
			buf1[1] = buf[6];
			buf1[2] = buf[10];
			buf1[3] = buf[14];
			buf1[4] = (char)0x0d;
			buf1[5] = (char)0x0a;
			buf1[6] = (char)0;
			// fileRepro.WriteString(buf1);
			string plugCount = new string(buf1);
			// System.out.println("plugCount:"+plugCount);
			return new string[] {userName.Substring(0, 4), userCode.Substring(0, 4), plugCount.Substring(0, 4), random.Substring(0, 4)};
		}
	}
}