using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject CreditsMenu;
    public GameObject PauseMenu;
    public Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Slider VolumeSlider;

    public void Play()
    {
        SceneManager.LoadScene("Level");
    }

    


    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }



    public void Settings()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Credits()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        
        MainMenu.SetActive(true);
   
    }


    public void Awake()
    {
        GetAvailableResolutions();
        AddUIListeners();
    }

    public void AddUIListeners()
    {
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        VolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
    }

    public void GetAvailableResolutions()
    {
        resolutions = Screen.resolutions;

        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        PlayerPrefs.SetInt("SettingsResolutionIndex", resolutionDropdown.value);
    }

    public void OnMusicVolumeChange()
    {
        AudioListener.volume = VolumeSlider.value; // affects menu music only
        PlayerPrefs.SetFloat("SettingsVolumeMusic", VolumeSlider.value);
    }

    public void Exit()
    {
        Application.Quit();
    }

}