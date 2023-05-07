using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMenuController : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject backButton;
    public static event Action OnButtonClicked = delegate { };
    public static event Action<GameObject> OnGameOver = delegate { };


    private void OnEnable()
    {
        pauseMenu.SetActive(false);
        Enemy.OnPlayerDied += StartShowingMenu;
    }


    private void OnDisable()
    {
        Enemy.OnPlayerDied -= StartShowingMenu;
    }

    private void Update()
    {
        OpenPauseMenu();
    }


    public void OpenPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            OnButtonClicked?.Invoke();
            if (pauseMenu.activeSelf == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void MainMenu()
    {
        GameManager.Instance.hasChoosenButton = true;
        SceneLoader.instance.StartLoadinScene(0);
        OnButtonClicked?.Invoke();
        Time.timeScale = 1;
    }


    public void Back()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        OnButtonClicked?.Invoke();
    }

    public void RestartGame()
    {
        GameManager.Instance.hasChoosenButton = true;
        SceneLoader.instance.StartLoadinScene(3);
        OnButtonClicked?.Invoke();
        Time.timeScale = 1;
    }


    private void StartShowingMenu(GameObject obj)
    {
        StartCoroutine(ShowPauseMenu());
    }

    IEnumerator ShowPauseMenu()
    {
        GameManager.Instance.hasChoosenButton = false;
        yield return new WaitForSeconds(3.5f);
        pauseMenu.SetActive(true);
        backButton.gameObject.SetActive(false);
    }
}
