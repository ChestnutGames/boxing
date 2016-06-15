

struct Empl
{
    public int x, y;
    public Empl(int a, int p)
    {
        x = a;
        y = p;
 
    } 
}

public class BuffData : AttrsBase
{ 
    public int buffid;
    public Def.AttrId id1;
    public float v1;
    public Def.AttrId id2;
    public float v2;
    public Def.AttrId id3;
    public float v3;
    public Def.AttrId id4;
    public float v4;
    public Def.AttrId id5;
    public float v5;
    public Def.AttrId id6;
    public float v6;
    public Def.AttrId id7;
    public float v7;
    public Def.AttrId id8;
    public float v8;

    public void GetBuffAttr()
    {
        switch (this.id1)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v1;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v1;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v1;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v1;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v1;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v1;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v1;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v1;
                break;
        }
        switch (this.id2)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v2;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v2;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v2;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v2;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v2;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v2;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v2;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v2;
                break;
        }
        switch (this.id3)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v3;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v3;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v3;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v3;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v3;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v3;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v3;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v3;
                break;
        }
        switch (this.id4)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v4;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v4;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v4;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v4;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v4;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v4;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v4;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v4;
                break;
        }
        switch (this.id5)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v5;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v5;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v5;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v5;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v5;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v5;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v5;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v5;
                break;

        }
        switch (this.id6)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v6;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v6;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v6;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v6;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v6;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v6;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v6;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v6;
                break;
        }
        switch (this.id7)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v7;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v7;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v7;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v7;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v7;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v7;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v7;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v7;
                break;
        }
        switch (this.id8)
        {
            case Def.AttrId.FightPowerAdd:
                this.additionArr[(int)Def.AttrType.FightPower] = this.v8;
                break;
            case Def.AttrId.DefenseAdd:
                this.additionArr[(int)Def.AttrType.Defense] = this.v8;
                break;
            case Def.AttrId.CritAdd:
                this.additionArr[(int)Def.AttrType.Crit] = this.v8;
                break;
            case Def.AttrId.PrayAdd:
                this.additionArr[(int)Def.AttrType.Pray] = this.v8;
                break;
            case Def.AttrId.FightPower:
                this.attrArr[(int)Def.AttrType.FightPower] = this.v8;
                break;
            case Def.AttrId.Defense:
                this.attrArr[(int)Def.AttrType.Defense] = this.v8;
                break;
            case Def.AttrId.Crit:
                this.attrArr[(int)Def.AttrType.Crit] = this.v8;
                break;
            case Def.AttrId.Pray:
                this.attrArr[(int)Def.AttrType.Pray] = this.v8;
                break;
        }
    }
}
