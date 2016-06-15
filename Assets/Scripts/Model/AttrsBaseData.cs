using System;
using System.Collections;

public class AttrsBase {
    //buffer用 
    public float[] attrArr = new float[Enum.GetNames(typeof(Def.AttrType)).Length];
    public float[] additionArr = new float[Enum.GetNames(typeof(Def.AttrType)).Length];//增益

}
