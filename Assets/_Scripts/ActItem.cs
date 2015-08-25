using UnityEngine;
public class ActItem
{
    private int id;
    private GameObject gameObject;
    private int actorType;
    private int hp;
    private int attack;
    private int defence;
    private int crit;
    private int dodge;
    /// <summary>
    /// 行动项
    /// </summary>
    /// <param name="id">行动项ID</param>
    /// <param name="gameObject">行动项的GameObject</param>
    /// <param name="actorType">1为友军，-1为敌军</param>
    public ActItem(int id, GameObject gameObject, int actorType)
    {
        this.id = id;
        this.gameObject = gameObject;
        this.actorType = actorType;
        this.hp = 0;
        this.attack = 0;
        this.defence = 0;
        this.crit = 0;
        this.dodge = 0;
    }
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

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
    }

    public int Defence
    {
        get
        {
            return defence;
        }

        set
        {
            defence = value;
        }
    }

    public int Crit
    {
        get
        {
            return crit;
        }

        set
        {
            crit = value;
        }
    }

    public int Dodge
    {
        get
        {
            return dodge;
        }

        set
        {
            dodge = value;
        }
    }
}

public class Enemy
{
    public string name;
    public string png;
    public int HP;
    public int str;
    public int def;
    public int exp;
    public int gold;
}

public class Hero
{
    public string name;
    public string png;
    int level;
    int exp;
    int baseHP;
    int baseMP;
    int baseStr;
    int baseMag;
    int baseDef;
    float coefficientHP;
    float coefficientMP;
    float coefficientStr;
    float coefficientMag;
    float coefficientDef;
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
}

