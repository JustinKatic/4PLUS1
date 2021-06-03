using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    //Physics Movement
    [Space]
    [SerializeField] public ActionBasedController controller;
    [SerializeField] InputActionReference controllerActionGrip;
    [SerializeField] InputActionReference controllerActionTrigger;

    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [Space]
    [SerializeField] private Transform palm;
    [SerializeField] float reachDistance = 0.1f;
    [SerializeField] float jointDistance = 0.05f;
    [SerializeField] LayerMask grabbableLayer;

    IEnumerator colliderBufferCo;


    private Transform _followTarget;
    private Rigidbody _body;

    private Collider[] cols;



    void Start()
    {
        //Physics Movement
        _followTarget = controller.gameObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f;

        //Teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;

        controllerActionGrip.action.performed += GripPress;
        controllerActionGrip.action.canceled += GripReleased;
        controllerActionTrigger.action.performed += TriggerPress;
        controllerActionTrigger.action.canceled += TriggerReleased;
        cols = gameObject.GetComponentsInChildren<Collider>();

    }



    private void TriggerReleased(InputAction.CallbackContext obj)
    {
        
    }

    private void TriggerPress(InputAction.CallbackContext obj)
    {
    }

    private void GripPress(InputAction.CallbackContext obj)
    {
        if (colliderBufferCo != null)
            StopCoroutine(colliderBufferCo);
        foreach (var col in cols)
        {
            col.enabled = false;
        }
    }
    private void GripReleased(InputAction.CallbackContext obj)
    {
        colliderBufferCo = DisableColliders();
        StartCoroutine(colliderBufferCo);

    }

    IEnumerator DisableColliders()
    {
        yield return new WaitForSeconds(1f);

        foreach (var col in cols)
        {
            col.enabled = true;
        }
    }


    void Update()
    {
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        //Position
        var positionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
    }
}


