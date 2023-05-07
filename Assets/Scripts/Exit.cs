using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Exit : MonoBehaviour
{
    CircleCollider2D circleCollider => GetComponent<CircleCollider2D>();
    public static event Action OnPlayerExited = delegate { };

    private void Start()
    {
        circleCollider.enabled = false;
    }


    private void OnEnable()
    {
        Bookshelf.OnBookCollected += AcitvateExit;
    }


    private void OnDisable()
    {
        Bookshelf.OnBookCollected -= AcitvateExit;
    }


    private void AcitvateExit(Loot loot)
    {
        circleCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.checkBox1.SetBool("isTaskCompleted", true);
            OnPlayerExited?.Invoke();
            SceneLoader.instance.StartLoadinScene(4);
        }
    }
}
