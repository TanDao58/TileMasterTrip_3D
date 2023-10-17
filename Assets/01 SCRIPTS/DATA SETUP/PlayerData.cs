using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataInGame
{
    public int current_Level = 0;
    public int player_score = 0;
    public int player_coin = 0;
    public bool is_player_enable_sound = true;
    public bool is_player_enable_vibration = true;
}

public class PlayerData : Singleton<PlayerData>
{
    PlayerDataInGame player_data = new PlayerDataInGame();
    private void OnEnable()
    {
        LoadData();
    }
    public int GetCurrentLevel() => player_data.current_Level;
    public void SetSoundStatus(bool status)
    {
        player_data.is_player_enable_sound = status;
        SaveData();
    }
    public void SetVibrationStatus(bool status)
    {
        player_data.is_player_enable_vibration = status;
        SaveData();
    }
    public bool GetSoundStatus() => player_data.is_player_enable_sound;
    public bool GetVibrationStatus() => player_data.is_player_enable_vibration;
    public void SetPlayerCoin(int coin)
    {
        player_data.player_coin = coin;
        SaveData();
    }
    public int GetPlayerCoin()
    {
        return player_data.player_coin;
    }
    public void SetCurrentLevel(int level)
    {
        player_data.current_Level = level;
        SaveData();
    }
    public void SetScore(int score)
    {
        player_data.player_score = score;
        SaveData();
    }
    public void SaveData()
    {
        BinarySerializer.Save(player_data, "datagame.txt");
    }
    public void LoadData()
    {
        player_data = BinarySerializer.Load<PlayerDataInGame>("datagame.txt");
    }
}
