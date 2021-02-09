using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    [SerializeField] private Light flashlightOne = null;
    [SerializeField] private Light flashlightTwo = null;
    [SerializeField] private float batteryDrainAmount = 0;
    [SerializeField] private float batteryFillAmount = 0;
    public FloatValue flashLightBattery;
    private float floneIntensity;
    private float fltwoIntensity;

    // Start is called before the first frame update
    void Start()
    {
        floneIntensity = flashlightOne.intensity;
        fltwoIntensity = flashlightTwo.intensity;
    }

    //TODO: Add way for battery to fill up, either on pick up or over time
    // Update is called once per frame
    void Update()
    {
        if (flashlightOne.enabled == true)
        {
            flashLightBattery.Value -= batteryDrainAmount * Time.deltaTime;
        }
        else if(flashLightBattery.Value >= 100)
        {
            flashLightBattery.Value = 100;
        }
        else
        {
            flashLightBattery.Value += batteryFillAmount * Time.deltaTime;
        }

        if (flashLightBattery.Value >= 10)
        {
            flashlightOne.intensity = floneIntensity;
            flashlightTwo.intensity = fltwoIntensity;
        }

        if (flashLightBattery.Value <= 10)
        {
            flashlightOne.intensity = Mathf.PingPong(Time.time, floneIntensity);
            flashlightTwo.intensity = flashlightOne.intensity;
        }

        if (flashLightBattery.Value <= 0)
        {
            flashlightOne.enabled = false;
            flashlightTwo.enabled = false;
        }

        if (!Input.GetKeyDown(KeyCode.F) || flashLightBattery.Value <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightOne.enabled = !flashlightOne.enabled;
            flashlightTwo.enabled = !flashlightTwo.enabled;
        }
    }
}
