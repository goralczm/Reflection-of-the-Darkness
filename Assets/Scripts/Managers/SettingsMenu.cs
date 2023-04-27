using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Instances")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider[] musicSliders;
    public Slider[] sfxSliders;

    [SerializeField] private GameObject pcSettings;
    [SerializeField] private GameObject mobileSettings;
    [SerializeField] private GameObject confirmPopup;
    [SerializeField] private TextMeshProUGUI timerText;


    [HideInInspector] public int currentResolutionIndex;
    private Resolution[] resolutions;
    private AudioManager _audioManager;
    private int oldResolutionIndex;
    private bool mustConfirm;

    public float confirmationTimer;
    public float musicVolume;
    public float sfxVolume;

    private void Start()
    {
        _audioManager = AudioManager.instance;
        if (_audioManager == null)
            Debug.LogWarning("Audio Manager not found!");

        mobileSettings.SetActive(false);
        pcSettings.SetActive(true);

        if (Application.isMobilePlatform)
        {
            pcSettings.SetActive(false);
            mobileSettings.SetActive(true);
        }

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            /*if (options.Contains(option))
                continue;*/

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        options.Reverse();
        Array.Reverse(resolutions);

        resolutionDropdown.AddOptions(options);

        SettingsData data = SaveSystem.LoadData("settings") as SettingsData;

        if (data == null)
            SaveData();
        else
            LoadData();

        ConfirmSelection();
    }
    private void ChangeResolutionValue(int newResolutionIndex)
    {
        resolutionDropdown.value = newResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        oldResolutionIndex = currentResolutionIndex;
        currentResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        mustConfirm = true;
        StartCoroutine(Confimation());
    }

    public void ConfirmSelection()
    {
        mustConfirm = false;
        confirmPopup.SetActive(false);
    }

    private void SetOldResolution()
    {
        ConfirmSelection();
        ChangeResolutionValue(oldResolutionIndex);
        ConfirmSelection();
    }

    public void SetMusicVolume(float volume)
    {
        _audioManager.masterMixer.SetFloat("musicVolume", volume);
        musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        _audioManager.masterMixer.SetFloat("sfxVolume", volume);
        sfxVolume = volume;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SaveData()
    {
        SettingsData data = new SettingsData(this);
        SaveSystem.SaveData(data, "settings");
    }

    public void LoadData()
    {
        SettingsData data = SaveSystem.LoadData("settings") as SettingsData;

        if (data == null)
            return;

        if (resolutionDropdown.options.Count != 0 && data.resolutionIndex <= resolutionDropdown.options.Count - 1)
            ChangeResolutionValue(data.resolutionIndex);
        else
            ChangeResolutionValue(0);
        ConfirmSelection();
        fullscreenToggle.isOn = data.fullscreen;
        foreach (Slider slider in musicSliders)
        {
            slider.value = data.musicVolume;
        }
        foreach (Slider slider in sfxSliders)
        {
            slider.value = data.sfxVolume;
        }
    }

    IEnumerator Confimation()
    {
        confirmPopup.SetActive(true);
        for (int i = 15; i >= 0; i--)
        {
            if (!mustConfirm)
            {
                confirmPopup.SetActive(false);
                yield break;
            }
            timerText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        SetOldResolution();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
