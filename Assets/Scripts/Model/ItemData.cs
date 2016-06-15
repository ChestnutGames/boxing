using UnityEngine;
using System.Collections;
using System;
 
    public class ItemData : AttrsBase
    {
        public enum QualityType
        {
            None = 0,
            White = 1,
            Green = 2,
            Blue = 3,
            Purple = 4,
            Glod = 5,
            Red = 6
        }

        public int id;
        public string name;

        public string desc;
        public string path;

        public int subType;
        public int useType;//服务器用
        public int bagType;
        public string useArg1;
        public string useArg2;
        public int sellPrice;
        public QualityType quality;
        public int maxCount;
        public int trace;
        public int isShow;
        public string typeName; 
    } 