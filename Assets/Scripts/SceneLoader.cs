using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator loadingAnimator;
    [SerializeField] AudioClip schoolSounds;
    [SerializeField] AudioClip startAmbience;
    [SerializeField] AudioClip endMenuAmbience;

    public static SceneLoader instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    public void StartLoadinScene(int sceneIdex)
    {
        StartCoroutine(LoadScene(sceneIdex));
    }


    IEnumerator LoadScene(int sceneIdex)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            SoundManager.instance.StartMusicTransition(schoolSounds);
        }
        else if (currentSceneIndex == 1)
        {
            SoundManager.instance.StartMusicTransition(null);
        }
        else if (currentSceneIndex == 2)
        {
            SoundManager.instance.StartMusicTransition(startAmbience);
        }
        else if (currentSceneIndex == 3)
        {
            SoundManager.instance.StartMusicTransition(endMenuAmbience);
        }
        loadingAnimator.Play("LoadingFadeOut", 0, 0f);
        yield return new WaitForSeconds(2);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIdex);
    }
}
