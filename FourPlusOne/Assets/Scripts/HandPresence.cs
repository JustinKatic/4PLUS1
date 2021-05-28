using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics DeviceCharecteristics;
    public GameObject ModelPrefab;

    private InputDevice targetDevice;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        //Get controllers with charecteristics, Don't know if the list is replaced or added too, we'll find out
        InputDevices.GetDevicesWithCharacteristics(DeviceCharecteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            if(ModelPrefab != null)
            {
                Instantiate(ModelPrefab, transform);
            }
        }
    }
}
