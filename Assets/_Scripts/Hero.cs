using UnityEngine;
using System.Collections;
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
        str = 10;
        mag = 100;
        def = 2;
    }

    public void setJsonToHero(JSONObject jo)
    {

    }
}

