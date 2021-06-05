using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    public GameObject baseControllerGameObject;
    public GameObject teleportationGameObject;
    public InputActionReference teleportActivationReference;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCanel;

    private void Start()
    {
        teleportActivationReference.action.performed += teleportModeActivate;
        teleportActivationReference.action.canceled += teleportModeCancel;

    }

    private void teleportModeCancel(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", .1f);

    private void teleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();

    void DeactivateTeleporter() => onTeleportCanel.Invoke();
}
