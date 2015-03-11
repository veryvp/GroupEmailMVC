using System.Data;
using System.IO;



namespace Common.Office
{
    public static class CsvHelper
    {
        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="columname">字段标题,逗号分隔</param>
        public static bool DtToCsv(DataTable dt, string strFilePath, string columname)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(columname);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > 0) {
                        sb.Append(",");
                    }
                    sb.Append(dt.Rows[i][j].ToString());
                }
            }
            return DtToCsv(sb.ToString(),strFilePath);
        }


        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="Content">内容</param>
        /// <param name="strFilePath">物理路径</param>
        public static bool DtToCsv(string Content, string strFilePath)
        {
            try
            {
                StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);
                strmWriterObj.WriteLine(Content);
               
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        public static DataTable CsvToDt(string filePath, int n, DataTable dt)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8, false);
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    string[] split = str.Split(',');

                    System.Data.DataRow dr = dt.NewRow();
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}


