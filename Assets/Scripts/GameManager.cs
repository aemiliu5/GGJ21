using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool paused;
    public float timerEnd;
    public int stamps;
    public Image clock;

    private void Start()
    {
        paused = false;
        Time.timeScale = 1;
        PlayerPrefs.SetInt("Stamps", 0);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        clock.fillAmount = 1 - (Time.timeSinceLevelLoad / timerEnd);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Pause();
        }

        if (Time.timeSinceLevelLoad > timerEnd)
        {
            PlayerPrefs.SetInt("Stamps", stamps);
            PlayerPrefs.Save();
            SceneManager.LoadScene("WinScene");
        }
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);

            FindObjectOfType<MouseLook>().enabled = false;

            foreach (AudioSource aud in FindObjectsOfType<AudioSource>())
            {
                if(aud.isPlaying)
                    aud.Pause();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            PauseMenu.SetActive(false);

            FindObjectOfType<MouseLook>().enabled = true;
            
            foreach (AudioSource aud in FindObjectsOfType<AudioSource>())
            {
                if(aud.time > 0)
                    aud.Play();
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
