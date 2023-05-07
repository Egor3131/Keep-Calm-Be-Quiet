using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject SettingsTab;
    [SerializeField] GameObject AuthorsTab;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenTogle;
    [SerializeField] TMP_Dropdown resDropdown;
    private bool savedFullscreenBool = true;
    private float savedVolumeValue;
    private int savedResIndex;
    private Resolution[] resolutions;
    public static event Action<float> OnSliderSaved = delegate { };
    public static event Action OnButtonPressed = delegate { };


    private void Start()
    {
        savedVolumeValue = volumeSlider.value;
        OnSliderSaved?.Invoke(volumeSlider.value);
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResolution;
        resDropdown.RefreshShownValue();
    }


    public void LoadNextScene()
    {
        SceneLoader.instance.StartLoadinScene(1);
        OnButtonPressed?.Invoke();
    }


    public void QuitGame()
    {
        Debug.Log("You are quitting");
        OnButtonPressed?.Invoke();
        Application.Quit();
    }


    public void EnableAuthors()
    {
        OnButtonPressed?.Invoke();
        AuthorsTab.SetActive(true);
    }


    public void EnableSettings()
    {
        OnButtonPressed?.Invoke();
        SettingsTab.SetActive(true);
        fullscreenTogle.isOn = savedFullscreenBool;
        volumeSlider.value = savedVolumeValue;
    }


    public void BackButton()
    {
        OnButtonPressed?.Invoke();
        AuthorsTab.SetActive(false);
        SettingsTab.SetActive(false);
    }


    public void IncreaseVolumeButton()
    {
        OnButtonPressed?.Invoke();
        volumeSlider.value += 1;
    }


    public void DecreaseVolumeButton()
    {
        OnButtonPressed?.Invoke();
        volumeSlider.value -= 1;
    }


    public void SaveButton()
    {
        OnButtonPressed?.Invoke();
        savedFullscreenBool = fullscreenTogle.isOn;
        Screen.fullScreen = fullscreenTogle.isOn;
        Resolution resolution = resolutions[savedResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        savedVolumeValue = volumeSlider.value;
        OnSliderSaved?.Invoke(savedVolumeValue);
    }

    public void SetResolution(int indexValue)
    {
        OnButtonPressed?.Invoke();
        savedResIndex = indexValue;
    }

}
