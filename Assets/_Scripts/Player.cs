using UnityEngine;
using System.Collections;

public class Player
{
    public int gold = 10000;
    public int bagSize;
    public int[] itemCount = new int[5];

    public Player()
    {
        bagSize = 20 + itemCount[4] * 20;
    }

    void Load()
    {
        if (PlayerPrefs.HasKey("gameData"))
        {
            string gameData = PlayerPrefs.GetString("gameData");
            JSONObject j = new JSONObject(gameData);
            j.GetField(ref gold, "gold");
            for (int i = 0; i < 5; i++)
            {
                j.GetField(ref itemCount[i], "item" + i + "Count");
            }
        }
    }
}
