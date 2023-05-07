using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class VolumeSlider : MonoBehaviour
{
    [SerializeField] List<Sprite> sliderSprites = new List<Sprite>();
    [SerializeField] Image currentImage;
    private Slider slider => GetComponent<Slider>();



    private void Start()
    {
        OnSliderValueChanged(slider.value);
    }

    public void OnSliderValueChanged(float sliderValue)
    {
        int index = Mathf.RoundToInt(sliderValue);
        currentImage.sprite = sliderSprites[index];
    }
}
