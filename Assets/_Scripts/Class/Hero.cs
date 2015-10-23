using UnityEngine;
using System.Collections;
public class Hero : ActItem
{
    public string name;
    public string png;
    public int level;
    public int exp;
    public int HPMax;
    public int MPMax;
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
    public int weaponID;
    public int armorID;

    public Hero()
    {
        base.ActorType = 1;
        level = 1;
        exp = 0;

        HPMax = 12;
        MPMax = 0;
        HP = HPMax;
        MP = MPMax;
        str = 5;
        mag = 0;
        def = 2;

        baseHP = 1;
        baseMP = 1;
        baseStr = 1;
        baseMag = 1;
        baseDef = 1;

        LoadData();

        checkLevelUp();
    }

    public void setJsonToHero(JSONObject jo)
    {
        jo.GetField(ref baseHP, "baseHP");
        jo.GetField(ref baseMP, "baseMP");
        jo.GetField(ref baseStr, "baseStr");
        jo.GetField(ref baseMag, "baseMag");
        jo.GetField(ref baseDef, "baseDef");
        checkLevelUp();
    }

    public void checkLevelUp()
    {
        int _constExp = Mathf.RoundToInt(Mathf.Pow((level), 0.4f) * Mathf.Pow(level, 2) * 5);
        while (exp >= _constExp)
        {
            level += 1;
            levelUp();
            exp -= _constExp;
            _constExp = Mathf.RoundToInt(Mathf.Pow((level), 0.4f) * Mathf.Pow(level, 2) * 5);
        }
    }

    public void levelUp()
    {
        HPMax = Mathf.RoundToInt(Mathf.Pow(baseHP, 0.4f) * HPMax);
        MPMax = Mathf.RoundToInt(Mathf.Pow(baseMP, 0.4f) * MPMax);
        HP = HPMax;
        MP = MPMax;
        str = str + baseStr;
        mag = mag + baseMag;
        def = def + baseDef;
    }

    public void SaveData()
    {
        JSONObject heroJsonData = new JSONObject(JSONObject.Type.OBJECT);
        heroJsonData.AddField("level", level);
        heroJsonData.AddField("exp", exp);
        heroJsonData.AddField("HPMax", HPMax);
        heroJsonData.AddField("MPMax", MPMax);
        heroJsonData.AddField("str", str);
        heroJsonData.AddField("mag", mag);
        heroJsonData.AddField("def", def);
        Debug.Log("heroData:" + heroJsonData.ToString());
        PlayerPrefs.SetString("heroJsonData", heroJsonData.ToString());
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("heroJsonData"))
        {
            string heroJsonData = PlayerPrefs.GetString("heroJsonData");
            JSONObject j = new JSONObject(heroJsonData);
            j.GetField(ref level, "level");
            j.GetField(ref exp, "exp");
            j.GetField(ref HPMax, "HPMax");
            j.GetField(ref MPMax, "MPMax");
            j.GetField(ref str, "str");
            j.GetField(ref mag, "mag");
            j.GetField(ref def, "def");
            HP = HPMax;
            MP = MPMax;
        }
    }
}

