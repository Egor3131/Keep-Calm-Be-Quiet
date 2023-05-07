using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class FlashLight : MonoBehaviour
{

    [SerializeField] GameObject flashLight;
    [SerializeField] BatterySlider batterySlider;
    [SerializeField] GraphUpdateScene graphUpdate;
    public bool isFlashLightOn = false;
    public bool isBatteryDied = false;
    public static event Action OnFlashLightOn = delegate { };
    public static event Action OnFlashLightOff = delegate { };


    private void Start()
    {
        StartCoroutine(UpdateGraphScene());
        StartCoroutine(UpdateWalkableArea());
    }

    private void OnEnable()
    {
        BatterySlider.OnBatteryDied += BatteryDied;
    }

    private void OnDisable()
    {
        BatterySlider.OnBatteryDied -= BatteryDied;
    }


    void Update()
    {
        TurnOn();
    }


    private void BatteryDied()
    {
        isBatteryDied = true;
        flashLight.SetActive(false);
        isFlashLightOn = false;
    }

    private void TurnOn()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isBatteryDied == false)
            {
                isFlashLightOn = !isFlashLightOn;
                flashLight.SetActive(isFlashLightOn);
            }
            if (isFlashLightOn == true)
            {
                OnFlashLightOn.Invoke();
            }
            else OnFlashLightOff.Invoke();
        }
    }


    IEnumerator UpdateWalkableArea()
    {
        while (true)
        {

            Bounds bounds = new Bounds(flashLight.transform.position, Vector3.one * 30);
            GraphUpdateObject guo = new GraphUpdateObject(bounds);


            AstarPath.active.UpdateGraphs(guo);
            yield return new WaitForSeconds(1);
        }
    }



    IEnumerator UpdateGraphScene()
    {
        while (true)
        {
            if (!isFlashLightOn)
            {
                graphUpdate.setWalkability = true;
            }
            else
            {
                graphUpdate.setWalkability = false;
            }
            graphUpdate.Apply();

            yield return new WaitForSeconds(0.2f);

        }
    }
}
