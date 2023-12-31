using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] GameObject EndPanel, WinPanel, LosePanel;
    [SerializeField] Text Star_txt, Coin_txt, Time_txt, Combo_txt;
    private int _player_score = 0;
    private int _current_combo = 0;

    private float _time_max_level = 480;


    [Header("Setup Time Combo")]
    [SerializeField] GameObject ProgressBar;
    [SerializeField] Slider _time_end_combo;
    [SerializeField] float _default_time_combo = 0;
    [SerializeField] float _time_combo_delta = 0;


    [Header("Count Image")]
    public int count_img_in_texture = 0;
    public int count_pawn_in_game = 0;

    [Header("Setup Game Value")]
    public bool IsLoseGame = false;
    public bool IsWinGame = false;
    public bool IsStopGame = false;
    private void Start()
    {
        Coin_txt.text = PlayerData.Instance.GetPlayerCoin().ToString();
        _time_end_combo.value = _time_end_combo.maxValue = _default_time_combo;
    }
    private void Update()
    {
        if (!IsStopGame)
        {
            EndPanel.SetActive(false);
            if (IsWinGame)
            {
                PlayerData.Instance.SetScore(_player_score);
                SetCoinsPlayer();
                WinPanel.SetActive(true);
                IsStopGame = true;
                return;
            }
            if (IsLoseGame)
            {
                ResetLevel();
                LosePanel.SetActive(true);
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
                IsLoseGame = true;
                IsStopGame = true;
                return;
            }
            if (_current_combo > 0)
            {
                ProgressBar.SetActive(true); ;
                if (_time_end_combo.value > 0)
                {
                    _time_end_combo.value -= Time.deltaTime;
                }
                else
                {
                    _current_combo = 0;
                }
            }
            else
            {
                ProgressBar.SetActive(false);
                _time_end_combo.value = _default_time_combo;
            }
        }
        else
            EndPanel.SetActive(true);

    }
    public void ResetLevel()
    {
        _time_max_level = 480;
        _current_combo = 0;
        SetScorePlayer(-_player_score);
        Star_txt.text = _player_score.ToString();
        AnimateCoins.Instance.Coins = 0;
        _time_end_combo.value = _time_end_combo.maxValue = _default_time_combo;
        IsWinGame = false;
        IsStopGame = false;
        IsLoseGame = false;
    }
    public void SetCoinsPlayer()
    {
        PlayerData.Instance.SetPlayerCoin(100);
        Coin_txt.text = PlayerData.Instance.GetPlayerCoin().ToString();
    }
    public void SetScorePlayer(int score)
    {
        _player_score += score; 
    }
    public void SetComboPlayer()
    {
        _current_combo++;
        _time_end_combo.value = _time_end_combo.maxValue -= _time_combo_delta / _current_combo;
        Combo_txt.text = "Combo : x " + _current_combo.ToString();
    }
    public int GetComboPlayer()
    {
        return _current_combo;
    }
}
