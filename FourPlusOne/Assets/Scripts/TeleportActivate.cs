using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class TeleportActivate : MonoBehaviour
{
    public GameObject teleportController;
    public InputActionReference teleportActivateRefrence;


    [Header("Teleport Events")]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    void Start()
    {
        teleportActivateRefrence.action.performed += TeleportModeActivate;
        teleportActivateRefrence.action.canceled += TeleportModeCancel; ;

    }


    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();

    private void TeleportModeCancel(InputAction.CallbackContext obj) => Invoke("DelayTeleportation", .1f);

    private void DelayTeleportation() => onTeleportCancel.Invoke();



}
