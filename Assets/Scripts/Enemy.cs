using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using System.Linq;
using System;

public enum EnemyState
{
    Roaming,
    Following,
    HeardSound
}

public class Enemy : MonoBehaviour, IHear
{
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintSpeed = 4f;
    [SerializeField] float standartViewDistance = 50f;
    [SerializeField] float viewDistanceFlashLight = 70f;
    [SerializeField] Transform target;
    [SerializeField] Animator animator;
    private float viewDistance;
    private Seeker seeker => GetComponent<Seeker>();
    private AIPath aiPath => GetComponent<AIPath>();
    private AIBase aiBase => GetComponent<AIBase>();
    private AudioSource audioSource => GetComponentInChildren<AudioSource>();
    private bool isIdlingStarted = false;
    private float distanceToPlayer;
    public static event Action<Vector3> OnPlayerHeard = delegate { };
    public static event Action<GameObject> OnPlayerDied = delegate { };
    private float footStepTimer;
    private float footStepTimerMax = 0.38f;
    private float screamTimer = 10f;
    private bool isScreamed = false;
    private Vector2 randomPos;
    private EnemyState currentState;


    void Start()
    {
        viewDistance = standartViewDistance;
        currentState = EnemyState.Roaming;
    }


    private void OnEnable()
    {
        FlashLight.OnFlashLightOn += IncreaseViewDistance;
        FlashLight.OnFlashLightOff += SetStandartViewDistance;
        Table.OnPlayerHidden += LosePlayer;
        Table.OnPlayerNotHidden += SetStandartViewDistance;
    }


    private void OnDisable()
    {
        FlashLight.OnFlashLightOn -= IncreaseViewDistance;
        FlashLight.OnFlashLightOff -= SetStandartViewDistance;
        Table.OnPlayerHidden -= LosePlayer;
        Table.OnPlayerNotHidden -= SetStandartViewDistance;
    }

    private void Update()
    {
        ScreamTimer();
        ClearPath();
        PlayWalkingSounds();
        UpdateFollowingState();
        animator.SetFloat("Speed", aiPath.maxSpeed);
        ChangeSpriteRotation();

        switch (currentState)
        {
            case EnemyState.Roaming:
                StartFindingRandomPath();
                break;

            case EnemyState.Following:
                FollowTarget();
                break;


            case EnemyState.HeardSound:

                UpdateHeardSoundState();
                break;
        }
    }


    private void FollowTarget()
    {
        if (target != null)
        {
            seeker.StartPath(transform.position, target.position);
            aiPath.maxSpeed = sprintSpeed;
        }
    }


    private void UpdateFollowingState()
    {
        if (target != null)
        {
            if ((target.position - transform.position).sqrMagnitude < viewDistance)
            {
                currentState = EnemyState.Following;
                if (isScreamed == false)
                {
                    OnPlayerHeard?.Invoke(transform.position);
                    isScreamed = true;
                }
            }
            else currentState = EnemyState.Roaming;

        }
    }


    private void UpdateHeardSoundState()
    {

        if (aiPath.remainingDistance == Mathf.Infinity)
        {
            aiBase.SetPath(null);
            currentState = EnemyState.Roaming;
        }
    }


    public void ReactToSound(Sound sound)
    {
        if (currentState != EnemyState.Following)
        {
            if (isScreamed == false)
            {
                OnPlayerHeard?.Invoke(transform.position);
                isScreamed = true;
            }
            currentState = EnemyState.HeardSound;
            seeker.StartPath(transform.position, sound.position);
        }
    }


    private void StartFindingRandomPath()
    {
        if (!aiPath.hasPath && !isIdlingStarted)
        {
            StartCoroutine(FindRandomPath());
        }
    }


    IEnumerator FindRandomPath()
    {
        isIdlingStarted = true;

        aiPath.maxSpeed = walkSpeed;
        randomPos = GetRandomPosition(transform.position, 10, 4);
        seeker.StartPath(transform.position, randomPos);

        yield return new WaitForSeconds(2.5f);
        isIdlingStarted = false;
    }



    private void ChangeSpriteRotation()
    {
        if (aiPath.hasPath && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Horizontal", Mathf.Abs(direction.x));

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else transform.localScale = new Vector3(1, 1, 1);
        }
    }




    Vector2 GetRandomPosition(Vector2 center, float radius, float minDistance)
    {
        Vector2 randomPos = Vector2.zero;
        float distanceToCenter = 0;

        do
        {
            Vector2 circle = UnityEngine.Random.insideUnitCircle * radius;
            randomPos = center + circle;
            distanceToCenter = (randomPos - center).sqrMagnitude;
        }
        while (distanceToCenter < minDistance * minDistance);

        return randomPos;
    }

    private void ScreamTimer()
    {
        if (isScreamed == true)
        {
            if (screamTimer < 0)
            {
                isScreamed = false;
                screamTimer = 10;
            }
            screamTimer -= Time.deltaTime;
        }
    }


    private void ClearPath()
    {
        if (aiPath.reachedEndOfPath)
        {
            aiBase.SetPath(null);
        }
    }


    private void PlayWalkingSounds()
    {
        if (IsEnemyMoving())
        {
            footStepTimer -= Time.deltaTime;
            if (footStepTimer < 0)
            {
                footStepTimer = footStepTimerMax;
                audioSource.clip = SoundManager.instance.GetMosterWalkingClip();
                audioSource.Play();
            }
        }
    }


    private bool IsEnemyMoving()
    {
        if (aiPath.hasPath || isIdlingStarted)
        {
            return true;
        }
        else return false;
    }


    private void LosePlayer()
    {
        aiPath.SetPath(null);
        viewDistance = 0;
        Debug.Log("losing path");
    }


    private void IncreaseViewDistance()
    {
        viewDistance = viewDistanceFlashLight;
    }


    private void SetStandartViewDistance()
    {
        viewDistance = standartViewDistance;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerDied?.Invoke(gameObject);
        }
    }
}
