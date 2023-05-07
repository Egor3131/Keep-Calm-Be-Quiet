using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StressSlider : MonoBehaviour
{
    [SerializeField] float scaleIncreaseTime = 0.5f;
    [SerializeField] FlashLight flashLight;
    [SerializeField] Image stressImage;
    [SerializeField] List<Sprite> scaleImages = new List<Sprite>();
    public Slider slider => GetComponent<Slider>();
    public static event Action OnStressFulled = delegate { };
    public static event Action OnStressCritical = delegate { };
    public static bool isStressEventStarted = false;


    private void Start()
    {

        StartControllStressSlider();
    }


    private void Update()
    {
        if (slider.value >= 30 && isStressEventStarted == false)
        {
            isStressEventStarted = true;
            OnStressCritical?.Invoke();
        }

        if (slider.value == slider.maxValue)
        {
            slider.value = 0;
            isStressEventStarted = false;
            OnStressFulled?.Invoke();
        }
    }


    public void OnSliderValueChanged()
    {
        int sliderValue = Mathf.RoundToInt(slider.value);
        if (sliderValue % 10 == 0)
        {
            int index = sliderValue / 10;
            stressImage.sprite = scaleImages[index];
        }
    }




    private void StartControllStressSlider()
    {
        StartCoroutine(ControllStressSlider());
    }


    IEnumerator ControllStressSlider()
    {
        while (true)
        {
            if (flashLight.isFlashLightOn == false)
            {
                yield return new WaitForSeconds(scaleIncreaseTime);
                if (slider != null)
                {
                    slider.value += 1;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

}
