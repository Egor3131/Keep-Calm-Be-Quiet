using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float sprintDelta = 2f;
    [SerializeField] float interactableDistance = 5f;
    [SerializeField] Transform FlashLight;
    public Animator animator => GetComponent<Animator>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    private Vector2 rawInput;
    private Transform nearestObj;
    private Canvas nearestCanvas;
    private Sound sound;
    public bool isHiding = false;
    public bool isRunning = false;
    public bool isInteracting = false;
    private bool isScreaming = false;



    public static Player instance { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(UpdateClosestObj());
    }

    private void OnEnable()
    {
        StressSlider.OnStressFulled += StartScreaming;

    }

    private void OnDisable()
    {
        StressSlider.OnStressFulled -= StartScreaming;
    }


    private void FixedUpdate()
    {
        Movement();
        MakeSprintSound();
        InteractWithObject();
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneLoader.instance.StartLoadinScene(0);
        }
    }


    private void MakeStepSound()
    {
        if (IsPlayerWalking())
        {
            sound = new Sound(6, transform.position);
            GameManager.Instance.MakeSoundWave(sound);
        }
    }

    private void MakeSprintSound()
    {
        sound = new Sound(12, transform.position);
        GameManager.Instance.MakeSoundWave(sound);
    }

    public bool IsPlayerWalking()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            return true;
        }
        else return false;
    }



    private void InteractWithObject()
    {
        if (nearestObj != null)
        {
            float distanceToObj = (nearestObj.position - transform.position).sqrMagnitude;

            if (distanceToObj <= interactableDistance)
            {
                nearestCanvas.gameObject.SetActive(true);
                var interactableObj = nearestObj.GetComponent<IInteracrable>();
                if (interactableObj == null) return;
                interactableObj.Interact();
            }
            else nearestCanvas.gameObject.SetActive(false);
        }
    }


    private void StartScreaming()
    {
        StartCoroutine(Scream());
    }

    IEnumerator Scream()
    {
        isScreaming = true;
        Sound sound = new Sound(40, transform.position);
        animator.Play("Screaming", 0, 0);
        yield return new WaitForSeconds(2);
        GameManager.Instance.MakeSoundWave(sound);
    }


    private void StopScreaming()
    {
        isScreaming = false;
    }

    private void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }


    private IEnumerator UpdateClosestObj()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            nearestObj = GameManager.Instance.GetClosestInteractable(transform);
            nearestCanvas = GameManager.Instance.GetClosestButton(transform);
        }
    }


    private void Movement()
    {
        if (isInteracting != false)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (isScreaming == false)
        {
            if (Input.GetButton("Sprint"))
            {
                MakeSprintSound();
                rb.velocity = rawInput * moveSpeed * sprintDelta;
                isRunning = true;
                animator.SetBool("isRunning", isRunning);
            }
            else
            {
                MakeStepSound();
                rb.velocity = rawInput * moveSpeed;
                isRunning = false;
                animator.SetBool("isRunning", isRunning);
            }
            animator.SetBool("isStanding", !IsPlayerWalking());
            animator.SetFloat("Horizontal", Math.Abs(rawInput.x));
            animator.SetFloat("Vertical", rawInput.y);
            if (rawInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
            if (rawInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
            SetLightRotation();
        }
        else rb.velocity = Vector2.zero;
    }


    private void SetLightRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;

        Vector2 lookDirection = rawInput.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        FlashLight.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }





}
