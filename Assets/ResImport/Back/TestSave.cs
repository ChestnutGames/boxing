using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
 

public class TestSave : MonoBehaviour
{
     
    void Start()
    {
        db();
       //DataHelper.SaveData<RoleData>(new RoleData(), "savedata.sf");
       //Debug.Log("1");
       //RoleData d =  DataHelper.LoadData<RoleData>("savedata.sf");
       //Debug.Log(d.ToString());
    }



    void db()
    {
        //创建数据库名称为xuanyusong.db
        DbAccess db = new DbAccess("data source=xuanyusong.db");
        //请注意 插入字符串是 已经要加上'宣雨松' 不然会报错
        db.CreateTable("momo", new string[] { "name", "qq", "email", "blog" }, new string[] { "text", "text", "text", "text" });
        //我在数据库中连续插入三条数据
        db.InsertInto("momo", new string[] { "'宣雨松'", "'289187120'", "'xuanyusong@gmail.com'", "'www.xuanyusong.com'" });
        db.InsertInto("momo", new string[] { "'雨松MOMO'", "'289187120'", "'000@gmail.com'", "'www.xuanyusong.com'" });
        db.InsertInto("momo", new string[] { "'哇咔咔'", "'289187120'", "'111@gmail.com'", "'www.xuanyusong.com'" });

        //然后在删掉两条数据
        db.Delete("momo", new string[] { "email", "email" }, new string[] { "'xuanyusong@gmail.com'", "'000@gmail.com'" });

        //注解1
        SqliteDataReader sqReader = db.SelectWhere("momo", new string[] { "name", "email" }, new string[] { "qq" }, new string[] { "=" }, new string[] { "289187120" });


        while (sqReader.Read())
        {
            Debug.Log(sqReader.GetString(sqReader.GetOrdinal("name")) + sqReader.GetString(sqReader.GetOrdinal("email")));
        }


        db.CloseSqlConnection();
    }

 
        //Process[] ps = Process.GetProcesses();
        //foreach (Process item in ps)
        //{
        //if (item.ProcessName == "加速程序的进程名字" || )
        //{
        ///// kill自己的游戏或者弹出提示什么的~
        //}
        //} 

    //private SQLiteHelper sql;
    ///// <summary>
    ///// 定义一个测试类
    ///// </summary>
    //public class TestClass
    //{
    //    public string Name = "张三";
    //    public float Age = 23.0f;
    //    public int Sex = 1;

    //    public List<int> Ints = new List<int>()
    //    {
    //        1,
    //        2,
    //        3
    //    };
    //}

    //void Start()
    //{
    //    //定义存档路径
    //    string dirpath = Application.persistentDataPath + "/Save";
    //    //创建存档文件夹
    //    JsonHelper.CreateDirectory(dirpath);
    //    //定义存档文件路径
    //    string filename = dirpath + "/GameData.sav";
    //    TestClass t = new TestClass();
    //    //保存数据
    //    JsonHelper.SetData(filename, t);
    //    //读取数据
    //    TestClass t1 = (TestClass)JsonHelper.GetData(filename, typeof(TestClass));

    //    Debug.Log(t1.Name);
    //    Debug.Log(t1.Age);
    //    Debug.Log(t1.Ints);
    //}


    //void Startsql()
    //{
    //    //创建名为sqlite4unity的数据库
    //    sql = new SQLiteHelper("data source=sqlite4unity.db");

    //    //创建名为table1的数据表
    //    sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });

    //    //插入两条数据
    //    sql.InsertValues("table1", new string[] { "'1'", "'张三'", "'22'", "'Zhang3@163.com'" });
    //    sql.InsertValues("table1", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });

    //    //更新数据，将Name="张三"的记录中的Name改为"Zhang3"
    //    sql.UpdateValues("table1", new string[] { "Name" }, new string[] { "'Zhang3'" }, "Name", "=", "'张三'");

    //    //插入3条数据
    //    sql.InsertValues("table1", new string[] { "3", "'王五'", "25", "'Wang5@163.com'" });
    //    sql.InsertValues("table1", new string[] { "4", "'王五'", "26", "'Wang5@163.com'" });
    //    sql.InsertValues("table1", new string[] { "5", "'王五'", "27", "'Wang5@163.com'" });

    //    //删除Name="王五"且Age=26的记录,DeleteValuesOR方法类似
    //    sql.DeleteValuesAND("table1", new string[] { "Name", "Age" }, new string[] { "=", "=" }, new string[] { "'王五'", "'26'" });

    //    //读取整张表
    //    SqliteDataReader reader = sql.ReadFullTable("table1");
    //    while (reader.Read())
    //    {
    //        //读取ID
    //        Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
    //        //读取Name
    //        Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
    //        //读取Age
    //        Debug.Log(reader.GetInt32(reader.GetOrdinal("Age")));
    //        //读取Email
    //        Debug.Log(reader.GetString(reader.GetOrdinal("Email")));
    //    }

    //    //读取数据表中Age>=25的所有记录的ID和Name
    //    reader = sql.ReadTable("table1", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
    //    while (reader.Read())
    //    {
    //        //读取ID
    //        Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
    //        //读取Name
    //        Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
    //    }

    //    //自定义SQL,删除数据表中所有Name="王五"的记录
    //    sql.ExecuteQuery("DELETE FROM table1 WHERE NAME='王五'");

    //    //关闭数据库连接
    //    sql.CloseConnection();
    //}


}