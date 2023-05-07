using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class Table : MonoBehaviour, IInteracrable
{
    [SerializeField] Slider interactSlider;
    [SerializeField] float buttonFillTime = 2f;
    private Player player => FindObjectOfType<Player>();
    private Animator animator => GetComponent<Animator>();
    private bool isButtonPressed = false;
    public static event Action OnPlayerHidden = delegate { };
    public static event Action OnPlayerNotHidden = delegate { };


    public void Interact()
    {
        if (Input.GetButton("Use") && isButtonPressed == false)
        {
            player.isInteracting = true;
            interactSlider.value += 1 / buttonFillTime * Time.deltaTime;
            if (!player.isHiding)
            {
                animator.Play("HidingAnimation", -1, interactSlider.normalizedValue);
                player.spriteRenderer.enabled = false;
            }
        }

        if (!Input.GetButton("Use"))
        {
            isButtonPressed = false;
            interactSlider.value -= 1 / buttonFillTime * Time.deltaTime;
            if (!player.isHiding)
            {
                animator.Play("HidingAnimation", -1, interactSlider.normalizedValue);
            }
        }

        if (interactSlider.value >= 0.99)
        {
            isButtonPressed = true;
            player.isHiding = !player.isHiding;
            if (player.isHiding == true)
            {
                OnPlayerHidden?.Invoke();
            }
            else
            {
                OnPlayerNotHidden?.Invoke();
            }
            animator.SetBool("isHiding", player.isHiding);
            interactSlider.value = 0;
            player.transform.position = gameObject.transform.position + new Vector3(0, -2);
            player.spriteRenderer.enabled = !player.isHiding;
        }

        if (interactSlider.value == 0 && player.isHiding == false)
        {
            player.isInteracting = false;
            player.spriteRenderer.enabled = true;
        }
    }


}
