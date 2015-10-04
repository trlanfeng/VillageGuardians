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
        baseHP = 12;
        baseMP = 0;
        baseStr = 5;
        baseMag = 0;
        baseDef = 2;

        coefficientHP = 1;
        coefficientMP = 1;
        coefficientStr = 1;
        coefficientMag = 1;
        coefficientDef = 1;

        checkLevel();
    }

    public void setJsonToHero(JSONObject jo)
    {
        jo.GetField(ref coefficientHP, "baseHP");
        jo.GetField(ref coefficientMP, "baseMP");
        jo.GetField(ref coefficientStr, "baseStr");
        jo.GetField(ref coefficientMag, "baseMag");
        jo.GetField(ref coefficientDef, "baseDef");
        checkLevel();
    }

    public void checkLevel()
    {
        int _constExp = Mathf.RoundToInt(Mathf.Pow((level + 1), 0.4f) * Mathf.Pow(level, 2) * 5);
        while (exp >= _constExp)
        {
            level += 1;
            exp -= _constExp;
            _constExp = Mathf.RoundToInt(Mathf.Pow((level + 1), 0.4f) * Mathf.Pow(level, 2) * 5);
        }
        calcHeroAttribute();
    }

    public void calcHeroAttribute()
    {
        HP = Mathf.RoundToInt(Mathf.Pow((level + 1), coefficientHP) * Mathf.Pow(level, 2) * baseHP);
        MP = Mathf.RoundToInt(Mathf.Pow((level + 1), coefficientMP) * Mathf.Pow(level, 2) * baseMP);
        str = Mathf.RoundToInt(Mathf.Pow((level + 1), coefficientStr) * Mathf.Pow(level, 2) * baseStr);
        mag = Mathf.RoundToInt(Mathf.Pow((level + 1), coefficientMag) * Mathf.Pow(level, 2) * baseMag);
        def = Mathf.RoundToInt(Mathf.Pow((level + 1), coefficientDef) * Mathf.Pow(level, 2) * baseDef);
    }
}

