using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] Text coin_txt;
    [SerializeField] Text star_txt;

    [Header("Panel")]
    public GameObject HomePanel;
    public GameObject MainPanel, WinPanel, LosePanel, EndPanel, LevelPanel;

    [Header("Button")]
    public Button SoundButton;
    public Button VibraButton, HomeButton, PlayButton, WinButton, LoseButton;

    [Header("Image")]
    [SerializeField] Sprite img_sound_on;
    [SerializeField] Sprite img_sound_off, img_vibra_on, img_vibra_off;

    [Header("Data")]
    [SerializeField] GameManager game_manager;
    [SerializeField] AutoSpawnLevel auto_spawn_level;
    [SerializeField] LevelDatabase level_database;

    [Header("Level")]
    [SerializeField] GameObject button_level;
    [SerializeField] Transform level_button_parent;
    bool is_sound_on, is_vibra_on;
    private void Start()
    {
        is_sound_on = PlayerData.Instance.GetSoundStatus();
        is_vibra_on = PlayerData.Instance.GetVibrationStatus();

        // Sound
        Image img = SoundButton.GetComponent<Image>();
        if (is_sound_on)
        {
            img.sprite = img_sound_on;
        }
        else
        {
            img.sprite = img_sound_off;
            SoundManager.Instance.DisableSound();
        }

        // Vibration
        img = VibraButton.GetComponent<Image>();

        if (is_vibra_on)
            img.sprite = img_vibra_on;
        else
            img.sprite = img_vibra_off;

        for (int i = 0; i < level_database.GetLengthLevels(); i++)
        {
            int temp = i;
            GameObject level = Instantiate(button_level, level_button_parent);
            level.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerData.Instance.SetCurrentLevel(temp); 
                LevelPanel.SetActive(false);
                HomePanel.SetActive(false);
                MainPanel.SetActive(true);
                ResetAndSpawn();
            });
            level.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }
        HomeButton.onClick.AddListener(() =>
        {
            HomePanel.SetActive(true);
            MainPanel.SetActive(false);
            coin_txt.text = PlayerData.Instance.GetPlayerCoin().ToString();
            star_txt.text = PlayerData.Instance.GetScore().ToString();
            auto_spawn_level.ResetLevel();
            game_manager.ResetData();
        });
        PlayButton.onClick.AddListener(() =>
        {
            HomePanel.SetActive(false);
            auto_spawn_level.SpawnLevel(level_database.GetLevelAtPos(PlayerData.Instance.GetCurrentLevel()));
            ScoreManager.Instance.ResetLevel();
            MainPanel.SetActive(true);
        });
        SoundButton.onClick.AddListener(() =>
        {
            is_sound_on = PlayerData.Instance.GetSoundStatus();
            Image img = SoundButton.GetComponent<Image>();
            PlayerData.Instance.SetSoundStatus(!is_sound_on);
            SoundManager.Instance.gameObject.SetActive(!is_sound_on);
            if (is_sound_on)
            {
                img.sprite = img_sound_off;
                SoundManager.Instance.DisableSound();
            }
            else
            {
                img.sprite = img_sound_on;
                SoundManager.Instance.PlaySound("background_music", true);
            }
        });
        VibraButton.onClick.AddListener(() =>
        {
            is_vibra_on = PlayerData.Instance.GetVibrationStatus();
            Image img = VibraButton.GetComponent<Image>();
            PlayerData.Instance.SetVibrationStatus(!is_vibra_on);
            if (is_vibra_on)
                img.sprite = img_vibra_off;
            else
                img.sprite = img_vibra_on;
        });
        coin_txt.text = PlayerData.Instance.GetPlayerCoin().ToString();
        star_txt.text = PlayerData.Instance.GetScore().ToString();
    }
    public void ChangeLevel(int level_add)
    {
        if (level_add > 0 && (PlayerData.Instance.GetCurrentLevel() + level_add) < level_database.GetLengthLevels())
            PlayerData.Instance.SetCurrentLevel(PlayerData.Instance.GetCurrentLevel() + level_add);
        ResetAndSpawn();
    }
    public void ResetAndSpawn()
    {
        auto_spawn_level.ResetLevel();
        auto_spawn_level.SpawnLevel(level_database.GetLevelAtPos(PlayerData.Instance.GetCurrentLevel()));
        ScoreManager.Instance.ResetLevel();
        game_manager.ResetData();
    }
}
