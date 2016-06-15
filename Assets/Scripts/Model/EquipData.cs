using UnityEngine;
using System.Collections;

public class EquipData {

    public enum EquipType
    {
        Boxing =1,
        Cloth = 2,
        Belt = 3,
        Shoe = 4
    }

    public int csv_id; 
    public string name;
    public EquipLevelData levelData;

}
