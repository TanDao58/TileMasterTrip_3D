using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject HomePanel;
    public GameObject MainPanel, WinPanel, LosePanel, EndPanel;

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

        HomeButton.onClick.AddListener(() =>
        {
            HomePanel.SetActive(true);
            MainPanel.SetActive(false);
        });
        PlayButton.onClick.AddListener(() =>
        {
            HomePanel.SetActive(false);
            auto_spawn_level.SpawnLevel(level_database.GetLevelAtPos(PlayerData.Instance.GetCurrentLevel()));
            game_manager.ResetData();
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
    }
    public void EndClick(int level_add)
    {
        if (level_add > 0 && (PlayerData.Instance.GetCurrentLevel() + level_add) < level_database.GetLengthLevels())
            PlayerData.Instance.SetCurrentLevel(PlayerData.Instance.GetCurrentLevel() + level_add);
        auto_spawn_level.SpawnLevel(level_database.GetLevelAtPos(PlayerData.Instance.GetCurrentLevel()));
        ScoreManager.Instance.ResetLevel();
        game_manager.ResetData();
    }
}
