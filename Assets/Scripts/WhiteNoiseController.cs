using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteNoiseController : MonoBehaviour
{
    private RawImage rawImage => GetComponent<RawImage>();
    private float currentStress = 0f;
    private float maxStress = -0.1f;
    private float elapsedTime = 0;
    private float increasingTime = 11f;
    private float decreasingTime = 4f;
    Material whiteNoiseMaterial;


    private void Start()
    {
        whiteNoiseMaterial = rawImage.material;
        whiteNoiseMaterial.SetFloat("_Transperency", 0);
    }


    private void OnEnable()
    {
        StressSlider.OnStressCritical += StartIncreasingEffect;
        StressSlider.OnStressFulled += StartDecreasingEffect;
    }


    private void OnDisable()
    {
        StressSlider.OnStressCritical -= StartIncreasingEffect;
        StressSlider.OnStressFulled -= StartDecreasingEffect;
    }


    private void StartIncreasingEffect()
    {
        StartCoroutine(IncreasingEffect());
        Debug.Log("effectStarted");
    }


    private void StartDecreasingEffect()
    {
        StopCoroutine(IncreasingEffect());
        StartCoroutine(DecreasingEffect());
    }


    IEnumerator DecreasingEffect()
    {
        elapsedTime = 0f;
        while (elapsedTime < decreasingTime)
        {
            elapsedTime += Time.deltaTime;
            currentStress = Mathf.Lerp(maxStress, 0, elapsedTime / decreasingTime);
            whiteNoiseMaterial.SetFloat("_Transperency", currentStress);
            yield return null;
        }
    }


    IEnumerator IncreasingEffect()
    {
        elapsedTime = 0f;
        while (elapsedTime < increasingTime)
        {
            elapsedTime += Time.deltaTime;
            currentStress = Mathf.Lerp(0, maxStress, elapsedTime / increasingTime);
            whiteNoiseMaterial.SetFloat("_Transperency", currentStress);
            yield return null;
        }
    }


}
