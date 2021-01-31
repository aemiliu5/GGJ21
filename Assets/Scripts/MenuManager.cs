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

    public Text stampsText;

    public Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Slider VolumeSlider;
    public AudioSource myFX;
    public AudioClip click;

    public GameObject loading;
    public Slider loadSlider;
    public Text loadText;

    public void Play()
    {
        ClickSound();
        StartCoroutine(LoadAsyncLevel("Level"));
    }

    IEnumerator LoadAsyncLevel(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != "WinScene")
        {
            MainMenu.SetActive(false);
            loading.SetActive(true); 
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (SceneManager.GetActiveScene().name != "WinScene")
            {
                loadSlider.value = progress;
                loadText.text = progress * 100 + "%";
                Debug.Log("Loading Progress: " + progress);
            }

            yield return null;
        }
    }

    public void Settings()
    {
        ClickSound();
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Credits()
    {
        ClickSound();
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        ClickSound();
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        MainMenu.SetActive(true);
    }

    public void GoToMainMenuFromWin()
    {
        ClickSound();
        SceneManager.LoadScene("Menu");
    }
    public void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            MainMenu.SetActive(true);
            loading.SetActive(false);
            SettingsMenu.SetActive(false);
            GetAvailableResolutions();
            AddUIListeners();
        }
        else if (SceneManager.GetActiveScene().name == "WinScene")
        {
            stampsText.text = "";
            stampsText.text += "Stamps\n";
            stampsText.text += PlayerPrefs.HasKey("Stamps") ? PlayerPrefs.GetInt("Stamps").ToString() : "0";
        }
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
        ClickSound();
        Application.Quit();
    }

    public void ClickSound()
    {
        myFX.PlayOneShot(click);
    }
}