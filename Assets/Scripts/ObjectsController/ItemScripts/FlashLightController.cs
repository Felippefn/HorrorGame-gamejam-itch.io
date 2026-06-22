using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlightLight;
    private bool isOn = false;

    void Start()
    {
        flashlightLight.enabled = false;
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn;
    }

    public void TurnOff()
    {
        isOn = false;
        flashlightLight.enabled = false;
    }
}
