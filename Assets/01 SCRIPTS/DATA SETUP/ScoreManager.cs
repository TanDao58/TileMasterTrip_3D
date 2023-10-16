using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] Text Score_txt, Coin_txt, Time_txt, Combo_txt;
    private int _player_score = 0;
    private int _current_combo = 0;

    private float _time_max_level = 480;
    private float _time_end_combo = 30;

    public int count_img_in_texture = 0;
    public int count_pawn_in_game = 0;

    public bool IsLoseGame = false;
    public bool IsWinGame = false;
    public bool IsStopGame = false;
    private void Start()
    {
        Coin_txt.text = PlayerData.Instance.GetPlayerCoin().ToString();
    }

    private void Update()
    {
        if (!IsStopGame)
        {
            if (IsWinGame)
            {
                PlayerData.Instance.SetPlayerCoin(1000);
                PlayerData.Instance.SetScore(_player_score);
                Coin_txt.text = PlayerData.Instance.GetPlayerCoin().ToString();
                IsStopGame = true;
                return;

            }
            if (_time_max_level > 0)
            {
                Time_txt.text = (_time_max_level / 60 >= 10 ? null : "0") + ((int)_time_max_level / 60).ToString() + " : " + (_time_max_level % 60 >= 10 ? null : "0") + ((int)_time_max_level % 60).ToString();
                _time_max_level -= Time.deltaTime;
            }
            else
            {
                //Thua
                IsStopGame = true;
                return;
            }
        }

    }
    public void SetScorePlayer(int score)
    {
        _player_score += score;
        Score_txt.text = _player_score.ToString();
    }
    public void SetComboPlayer(int combo)
    {
        _current_combo = combo;
        Combo_txt.text = combo.ToString();
    }
    public int GetComboPlayer()
    {
        return _current_combo;
    }
}
