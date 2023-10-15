using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataInGame
{
    public int current_Level = 0;
    public int player_star = 0;
}

public class PlayerData : Singleton<PlayerData>
{
    PlayerDataInGame player_data;
    private void Start()
    {
        LoadData();
    }
    public void SetCurrentLevel(int level)
    {
        player_data.current_Level = level;
        SaveData();
    }
    public void SetStar(int star)
    {
        player_data.player_star = star;
        SaveData();
    }
    public void SaveData()
    {
        BinarySerializer.Save<PlayerDataInGame>(player_data, "datagame.txt");
    }
    public void LoadData()
    {
        player_data = BinarySerializer.Load<PlayerDataInGame>("datagame.txt");
    }
}
