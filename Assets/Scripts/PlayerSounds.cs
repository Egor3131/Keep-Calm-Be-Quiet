using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player => GetComponent<Player>();
    private float footStepTimer = 0;


    void Update()
    {
        if (player.isRunning)
        {
            float footStepTimerMax = 0.35f;
            footStepTimer -= Time.deltaTime;
            if (footStepTimer < 0)
            {
                footStepTimer = footStepTimerMax;
                float volume = 0.5f;


                SoundManager.instance.PlayFootStepsSound(transform.position, volume);
            }
        }
        else if (player.IsPlayerWalking())
        {
            float footStepTimerMax = 0.4f;

            footStepTimer -= Time.deltaTime;
            if (footStepTimer < 0)
            {
                footStepTimer = footStepTimerMax;
                float volume = 0.5f;


                SoundManager.instance.PlayFootStepsSound(transform.position, volume);

            }
        }


    }
}