using UnityEngine;
public class ActItem
{
    private int id;
    private GameObject gameObject;
    //1为友军，-1为敌军
    private int actorType;

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }
    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }

        set
        {
            gameObject = value;
        }
    }
    /// <summary>
    /// -1：敌人；0：中立；1：友军
    /// </summary>
    public int ActorType
    {
        get
        {
            return actorType;
        }

        set
        {
            actorType = value;
        }
    }
}

public class Player
{
    public int gold;
    public int bagSize;
    public int item1Count;
    public int item2Count;
    public int item3Count;
    public int item4Count;
    public int item5Count;
}

public class Enemy : ActItem
{
    public string name;
    public string png;
    public int HP;
    public int str;
    public int def;
    public int exp;
    public int gold;

    public Enemy()
    {
        base.ActorType = -1;
        HP = 50;
        str = 10;
        def = 10;
        exp = 10;
        gold = 10;
    }

    public void setJsonToEnemy(JSONObject jo)
    {
        jo.GetField(ref HP, "hp");
        jo.GetField(ref str, "str");
        jo.GetField(ref def, "def");
        jo.GetField(ref exp, "exp");
        jo.GetField(ref gold, "gold");
    }
}

public class Hero : ActItem
{
    public string name;
    public string png;
    public int level;
    public int exp;
    public int HP;
    public int MP;
    public int str;
    public int mag;
    public int def;
    public int baseHP;
    public int baseMP;
    public int baseStr;
    public int baseMag;
    public int baseDef;
    public float coefficientHP;
    public float coefficientMP;
    public float coefficientStr;
    public float coefficientMag;
    public float coefficientDef;
    public int weaponID;
    public int armorID;

    public Hero()
    {
        base.ActorType = 1;
        level = 1;
        exp = 0;
        HP = 100;
        MP = 100;
        str = 5;
        mag = 100;
        def = 2;
    }

    public void setJsonToHero(JSONObject jo)
    {

    }
}

public class Item
{
    public int id = 0;
    public string name = "";
    public string png = "";
    public string effect = "";
    public string comment = "";
    public int gold = 0;
    public int cure = 0;
    public int count = 0;
    public GameObject GameObject;
}

