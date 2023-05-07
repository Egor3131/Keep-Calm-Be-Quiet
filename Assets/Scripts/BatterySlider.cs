using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BatterySlider : MonoBehaviour
{
    [SerializeField] List<Sprite> batteryStates = new List<Sprite>();
    [SerializeField] Image batteryImage;
    [SerializeField] float batteryReductionSpeed;
    [SerializeField] FlashLight flashLight;
    private Slider batterySlider => GetComponent<Slider>();
    private Slider slider => GetComponent<Slider>();
    public static event Action OnBatteryDied = delegate { };



    private void Start()
    {
        StartControllBatteryDicrease();
    }


    private void OnEnable()
    {
        Inventory.OnBatteryCollected += AddBatteryLevel;
    }


    private void OnDisable()
    {
        Inventory.OnBatteryCollected -= AddBatteryLevel;
    }


    public void OnSliderValueChanged()
    {
        int sliderValue = Mathf.RoundToInt(slider.value);
        if (sliderValue % 10 == 0)
        {
            int index = sliderValue / 10;
            batteryImage.sprite = batteryStates[index];
        }

    }



    private void StartControllBatteryDicrease()
    {
        StartCoroutine(ControllBatteryDicrease());
    }


    IEnumerator ControllBatteryDicrease()
    {
        while (true)
        {
            if (flashLight.isFlashLightOn)
            {
                batterySlider.value -= 1;
                if (batterySlider.value <= 0)
                {
                    OnBatteryDied?.Invoke();
                }
                yield return new WaitForSeconds(batteryReductionSpeed);
            }
            else
            {
                yield return null;
            }
        }
    }


    private void AddBatteryLevel()
    {
        flashLight.isBatteryDied = false;
        flashLight.isFlashLightOn = true;
        batterySlider.value += 50;
        OnSliderValueChanged();
    }

}
