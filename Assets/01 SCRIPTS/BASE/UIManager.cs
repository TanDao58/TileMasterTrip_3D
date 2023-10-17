using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Button")]
    public Button SoundButton, VibraButton;

    [Header("Image")]
    [SerializeField] Sprite img_sound_on;
    [SerializeField] Sprite img_sound_off, img_vibra_on, img_vibra_off;

    bool is_sound_on, is_vibra_on;
    private void Start()
    {
        SoundButton.onClick.AddListener(() =>
        {
            is_sound_on = PlayerData.Instance.GetSoundStatus();
            Image img = SoundButton.GetComponent<Image>();
            PlayerData.Instance.SetSoundStatus(!is_sound_on);
            SoundManager.Instance.gameObject.SetActive(!is_sound_on);
            if (is_sound_on)
                img.sprite = img_sound_off;
            else
                img.sprite = img_sound_on;
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
    public void LoadNewScene(int index)
    {
        SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(index).name);
    }
}
