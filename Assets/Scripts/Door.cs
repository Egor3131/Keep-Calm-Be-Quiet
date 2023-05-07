using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Pathfinding;

public class Door : MonoBehaviour, IInteracrable
{
    [SerializeField] Slider interactSlider;
    [SerializeField] float buttonFillTime = 2f;
    [SerializeField] KeyType requiredKey;
    [SerializeField] AstarPath astarPath;
    private Animator animator => GetComponent<Animator>();
    private Player player => FindObjectOfType<Player>();
    private Inventory inventory => FindObjectOfType<Inventory>();
    private GraphUpdateScene graphUpdate => GetComponent<GraphUpdateScene>();
    private bool isButtonPressed = false;
    public static event Action<Vector3> OnDoorOpened = delegate { };
    public static event Action<KeyType> OnDoorNoKey = delegate { };





    public void Interact()
    {
        if (Input.GetButton("Use") && isButtonPressed == false)
        {
            player.isInteracting = true;
            interactSlider.value += 1 / buttonFillTime * Time.deltaTime;
        }

        if (!Input.GetButton("Use"))
        {
            isButtonPressed = false;
            player.isInteracting = false;
            interactSlider.value -= 1 / buttonFillTime * Time.deltaTime;
        }

        if (interactSlider.value >= 0.99)
        {
            isButtonPressed = true;
            if (inventory.HasKey(requiredKey))
            {
                animator.Play("DoorOpen", 0, 0f);
                OnDoorOpened?.Invoke(transform.position);
                interactSlider.gameObject.SetActive(false);
                UpdateDoorGrid();

            }
            else
            {
                OnDoorNoKey?.Invoke(requiredKey);
            }
            interactSlider.value = 0;
        }

        if (interactSlider.value == 0)
        {
            player.isInteracting = false;
        }
    }

    private void UpdateDoorGrid()
    {
        graphUpdate.setWalkability = true;
    }

}
