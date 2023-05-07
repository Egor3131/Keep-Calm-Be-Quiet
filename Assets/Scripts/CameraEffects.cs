using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;
public class CameraEffects : MonoBehaviour
{
    [SerializeField] float accumulationSpeed = 1;
    [SerializeField] Slider stressSlider;
    Cinemachine.CinemachineVirtualCamera[] cameras;
    float intencity = 0;

    private void Start()
    {
        cameras = FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();

        foreach (Cinemachine.CinemachineVirtualCamera camera in cameras)
        {
            CinemachineBasicMultiChannelPerlin noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0;
        }
    }


    private void OnEnable()
    {
        StressSlider.OnStressCritical += StartShaking;
        StressSlider.OnStressFulled += StopShaking;
    }


    private void OnDisable()
    {
        StressSlider.OnStressCritical -= StartShaking;
        StressSlider.OnStressFulled -= StopShaking;
    }


    private void StartShaking()
    {
        StartCoroutine(Shake());
        Debug.Log("Started");
    }


    private void StopShaking()
    {
        StopCoroutine(Shake());
        SetShakeIntecity(0);
        Debug.Log("stoped");
    }


    IEnumerator Shake()
    {
        while (stressSlider.value >= 29)
        {
            IncreaseShakeIntecity(accumulationSpeed);
            yield return new WaitForSeconds(1f);
        }
    }


    private void SetShakeIntecity(float shakeIntencity)
    {
        intencity = shakeIntencity;
        foreach (Cinemachine.CinemachineVirtualCamera camera in cameras)
        {
            CinemachineBasicMultiChannelPerlin noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = intencity;
        }
    }


    private void IncreaseShakeIntecity(float speed)
    {
        foreach (Cinemachine.CinemachineVirtualCamera camera in cameras)
        {
            CinemachineBasicMultiChannelPerlin noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            intencity += Time.deltaTime * speed;
            noise.m_AmplitudeGain = intencity;
        }
    }
}





