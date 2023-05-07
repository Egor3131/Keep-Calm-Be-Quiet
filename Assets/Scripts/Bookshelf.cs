using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Bookshelf : MonoBehaviour, IInteracrable
{
    [SerializeField] Slider interactSlider;
    [SerializeField] float buttonFillTime = 2f;
    private Player player => FindObjectOfType<Player>();
    private Animator animator => GetComponent<Animator>();
    private LootDrop lootDrop => GetComponent<LootDrop>();
    public static event Action<Loot> OnBookCollected = delegate { };



    public void Interact()
    {
        if (Input.GetButton("Use"))
        {
            player.isInteracting = true;
            interactSlider.value += 1 / buttonFillTime * Time.deltaTime;
        }

        if (!Input.GetButton("Use"))
        {
            interactSlider.value -= 1 / buttonFillTime * Time.deltaTime;
            player.isInteracting = false;
        }

        if (interactSlider.value >= 0.99)
        {
            Loot item = lootDrop.GetLoot();
            if (item.itemName == "Book")
            {
                player.animator.Play("PickUpBook");
                OnBookCollected.Invoke(item);
                GameManager.Instance.checkBox.SetBool("isTaskCompleted", true);

            }
            interactSlider.gameObject.SetActive(false);
            player.isInteracting = false;
            interactSlider.value = 0;
        }
    }
}
