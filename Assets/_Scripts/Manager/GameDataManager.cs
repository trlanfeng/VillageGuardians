using UnityEngine;
using System.Collections;

public class GameDataManager : MonoBehaviour
{

    private int itemCount = 5;

    GameManager GM;

    void Start()
    {
        GM = transform.GetComponent<GameManager>();
    }
    //根据文档名保存存档，自动存档为auto，其他为数字编号
    public void SaveData(string dataName)
    {
        JSONObject gameData = new JSONObject(JSONObject.Type.OBJECT);
        gameData.AddField("level", GM.level);
        gameData.AddField("gold", GM.player.gold);
        for (int i = 0; i < itemCount; i++)
        {
            gameData.AddField("item" + i + "Count", GM.player.itemCount[i]);
        }
        PlayerPrefs.SetString("gameData_" + dataName, gameData.ToString());
    }
    //根据文档名读取存档，自动存档为auto，其他为数字编号
    public void LoadData(string dataName)
    {
        if (PlayerPrefs.HasKey("gameData_" + dataName))
        {
            string gameData = PlayerPrefs.GetString("gameData_" + dataName);
            JSONObject j = new JSONObject(gameData);
            j.GetField(ref GM.level, "level");
            j.GetField(ref GM.player.gold, "gold");
            for (int i = 0; i < 5; i++)
            {
                j.GetField(ref GM.player.itemCount[i], "item" + i + "Count");
            }
        }
    }
    //读取自动存档
    public void LoadAutoData()
    {
        LoadData("auto");
    }
    //保存自动存档
    public void SaveAutoData()
    {
        SaveData("auto");
    }

}
