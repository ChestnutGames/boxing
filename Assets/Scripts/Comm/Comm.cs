using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public class Comm  {

    public static string NowDateDiff(string tar)
    { 
        DateTime dt = DateTime.ParseExact(tar, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);  
        return DateDiff(dt,DateTime.Now);
    }

    public DateTime getDate(string str)
    {
        return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
    }

    public static int GetSeconds(DateTime DateTime1, DateTime DateTime2)
    {
        if (DateTime.Compare(DateTime1, DateTime2) > 0)
            return 0;
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration(); 
        return ts.Seconds; 
    }

    public static string DateDiffHour(DateTime DateTime1, DateTime DateTime2)
    {
        if (DateTime.Compare(DateTime1, DateTime2) > 0)
            return "";
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        string dateDiff = "";
        int day = 0;
        if (ts.Days > 0)
        {
            day = ts.Days;
        }  
         
        int a = (day * 24) + ts.Hours;
        if (a < 10)
            dateDiff += "0";
        dateDiff += a.ToString() + " : "; 
       
        int b = ts.Minutes;
        if (b < 10)
            dateDiff += "0";
            dateDiff += b.ToString() + " : ";

        int c = ts.Seconds;
        if (c < 10)
            dateDiff += "0";
        dateDiff += c.ToString();
        return dateDiff;
    } 

    /// <summary> 时间间隔 </summary>
    /// <param name="DateTime1">第一个日期和时间当前</param>
    /// <param name="DateTime2">第二个日期和时间目标</param>
    /// <returns>同一天的相隔的分钟的整数部分</returns>
    /// 
    public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        if (DateTime.Compare(DateTime1, DateTime2) > 0)
            return "";
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        string dateDiff = "";
        if (ts.Days > 365)
        {
            dateDiff += ts.Days/365 + "年";
        }
        else if (ts.Days > 0)
        {
           dateDiff+= ts.Days.ToString() + "天";
        }
        else if (ts.Hours > 0)
        {
            dateDiff +=  ts.Hours.ToString() + "小时";
        }
        else if (ts.Minutes > 0)
        {
            dateDiff += ts.Minutes.ToString() + "分钟";
        }             
        else if (ts.Seconds > 0)
        {
            dateDiff +=  ts.Seconds.ToString() + "秒";
        } 
        return dateDiff;
    }

    public int GetDay()
    {
        return DateTime.Now.Day;
    }
     
    /// <summary> 
    /// 名称：IsNumberic 
    /// 功能：判断输入的是否是数字 
    /// 参数：string oText：源文本 
    /// 返回值：　bool true:是　false:否 
    /// </summary> 
    public static bool IsNumberic(string oText)
    {
        try
        {
            int var1 = Convert.ToInt32(oText);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetAttrStr(Def.AttrId type)
    {
        string str = "";
        switch (type)
        {
            case Def.AttrId.FightPower:
                str = "战力";
                break;
            case Def.AttrId.Defense:
                str = "防御";
                break;
            case Def.AttrId.Pray:
                str = "王者";
                break;
            case Def.AttrId.Crit:
                str = "暴击";
                break;
        }
        return str;
    }
}
