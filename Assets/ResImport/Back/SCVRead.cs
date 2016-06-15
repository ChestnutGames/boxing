using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SCVRead { 
   
    #region Scv格式文件读取和生成
    /// <summary>
    /// 对读取到的cvs单独一行内容进行处理，去掉Csv格式，返回常规字符串，每项之间用特殊字符“^”隔开
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static string DealCode(string str)
    {
        string s = "";
        int k = 1;
        if (str.Length == 0) return "";
        str = str.Replace("^", "");
        for (int i = 0; i < str.Length; i++)
        {
            switch (str.Substring(i, 1))
            {
                case "\"":
                    s += str.Substring(i, 1);
                    k++;
                    break;
                case ",":
                    if (k % 2 == 0)
                        s += str.Substring(i, 1);
                    else
                        s += "^";
                    break;
                default: s += str.Substring(i, 1); break;
            }
        }
        return s;
    }
    /// <summary>
    /// 对单引号和双引号处理
    /// </summary>
    /// <param name="tmp"></param>
    /// <returns></returns>
    private static string[] DealCode2(string[] tmp)
    {
        string[] tmps = new string[tmp.Length];
        for (int i = 0; i < tmp.Length; i++)
        {
            string temp = tmp[i].Replace("\"\"", "^");
            temp = temp.Replace("\"", "");
            temp = temp.Replace("^", "\"");
            temp = temp.Replace("''", "∵");
            temp = temp.Replace("∵", "'");
            tmps[i] = temp;
        }
        return tmps;
    }

    public static List<string[]> GetScvByFile(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        return GetScv(sr);
    }
    /// <summary>
    /// 获取Scv文件
    /// </summary>
    /// <param name="reader">System.IO.StreamReader流</param>
    /// <returns>返回List<string[]>数组</returns>
    public static List<string[]> GetScv(System.IO.StreamReader reader)
    {
        List<string[]> list = new List<string[]>();
        string strline = "";
        while ((strline = reader.ReadLine()) != null)//每次单独抽取Csv一行的内容来处理
        {
            strline = DealCode(strline).Replace("'", "''");//调用函数处理每一行内容
            string[] strs = strline.Split(new char[] { '^' });//对处理后的内容进行特殊字符“^”分隔就得到了常规的字符数组了，你就可以进行其他用途了。
            strs = DealCode2(strs);
            list.Add(strs);
        }
        return list;
    }
     
    #endregion
}
