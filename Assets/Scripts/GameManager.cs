using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    GameObject jumpScare;
    GameObject jumpScareFace;
    public Animator checkBox;
    public Animator checkBox1;
    public List<GameObject> interactableObj;
    public bool hasChoosenButton;
    [HideInInspector] public List<Canvas> buttons;
    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    private void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeValues;

    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= InitializeValues;
    }

    private void InitializeValues(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3)
        {
            checkBox = GameObject.FindGameObjectWithTag("CheckBox").GetComponent<Animator>();
            checkBox1 = GameObject.FindGameObjectWithTag("CheckBox1").GetComponent<Animator>();
            jumpScare = GameObject.FindGameObjectWithTag("JumpScare");
            jumpScareFace = GameObject.FindGameObjectWithTag("JumpScareFace");

            jumpScare.gameObject.SetActive(false);

            buttons = new List<Canvas>();
            interactableObj = new List<GameObject>();
            interactableObj = GameObject.FindGameObjectsWithTag("Interactable").ToList();
            Enemy.OnPlayerDied += StartLoadingToMenu;
            TurnOffAllCanvases();
        }
    }



    private void TurnOffAllCanvases()
    {
        foreach (GameObject go in interactableObj)
        {
            var canvas = go.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                buttons.Add(canvas);
                canvas.gameObject.SetActive(false);
            }
        }
    }




    public Transform GetClosestInteractable(Transform playerPos)
    {
        var sortedList = interactableObj.OrderBy(obj => (playerPos.position - obj.transform.position).sqrMagnitude).ToList();
        return sortedList[0].transform;
    }


    public Canvas GetClosestButton(Transform playerPos)
    {
        var sortedList = buttons.OrderBy(obj => (playerPos.position - obj.transform.position).sqrMagnitude).ToList();
        return sortedList[0];
    }


    public void MakeSoundWave(Sound sound)
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(sound.position, sound.range);
        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].TryGetComponent<IHear>(out IHear hear))
            {
                hear.ReactToSound(sound);
            }
        }
    }


    private void StartLoadingToMenu(GameObject enemy)
    {
        StartCoroutine(KillPlayer());
    }


    IEnumerator KillPlayer()
    {
        jumpScare.SetActive(true);
        Destroy(Player.instance.gameObject);
        jumpScareFace.transform.DOShakePosition(2, 2, 20);
        jumpScareFace.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        yield return new WaitForSeconds(15f);
        if (!hasChoosenButton)
        {
            SceneLoader.instance.StartLoadinScene(0);
        }
    }



}
