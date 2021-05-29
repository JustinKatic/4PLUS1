using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics DeviceCharecteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;
    private UnityEngine.XR.InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    private ActionBasedController controller;





    void Start()
    {
        controller = gameObject.GetComponentInParent<ActionBasedController>();

        //Select Action
        controller.selectAction.action.performed += SelectActionPerformed;
        controller.selectAction.action.canceled += SelectActionCanceled;

        //Activate Action
        controller.activateAction.action.performed += ActivateActionPerformed;
        controller.activateAction.action.canceled += ActivateActionCanceled;

        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(DeviceCharecteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
                spawnedController = Instantiate(prefab, transform);
            else
            {
                Debug.Log("didnt find correct device");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("no device connected");
            spawnedController = Instantiate(controllerPrefabs[0], transform);
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }




    //Activate Action
    private void ActivateActionPerformed(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Grip", 1);
    }
    private void ActivateActionCanceled(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Grip", 0);
    }




    //Select Action
    private void SelectActionPerformed(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Trigger", 1);
    }
    private void SelectActionCanceled(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Trigger", 0);
    }


    private void Update()
    {
        if (showController)
        {
            spawnedHandModel.SetActive(false);
            spawnedController.SetActive(true);
        }
        else
        {
            spawnedHandModel.SetActive(true);
            spawnedController.SetActive(false);
        }
    }
}
