using UnityEngine; 
using System.Collections.Generic;
using System;

public class ChapterData
{
    public int csv_id;
    public int level;
    public string bg;
	public string name;
    public int power; 


    public int[] typeMax = new int[Enum.GetNames(typeof(Def.levelType)).Length];
    public int[] curLevel = new int[Enum.GetNames(typeof(Def.levelType)).Length]; 
}
