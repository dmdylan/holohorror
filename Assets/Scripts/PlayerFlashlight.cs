using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    private Light flashLight;
    [SerializeField] private float batteryDrainAmount = 0;
    [SerializeField] private float batteryFillAmount = 0;
    public FloatValue flashLightBattery;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponentInChildren<Light>();
    }

    //TODO: Add way for battery to fill up, either on pick up or over time
    // Update is called once per frame
    void Update()
    {
        if (flashLight.enabled == true)
        {
            flashLightBattery.Value -= batteryDrainAmount * Time.deltaTime;
        }

        if (flashLightBattery.Value >= 10)
        {
            flashLight.intensity = 2f;
        }

        if (flashLightBattery.Value <= 10)
        {
            flashLight.intensity = Mathf.PingPong(Time.time, 2);
        }

        if (flashLightBattery.Value <= 0)
        {
            flashLight.enabled = false;
        }

        if (!Input.GetKeyDown(KeyCode.F) || flashLightBattery.Value <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.enabled = !flashLight.enabled;
        }
    }
}
