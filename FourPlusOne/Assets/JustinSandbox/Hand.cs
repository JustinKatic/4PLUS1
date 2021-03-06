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
    [SerializeField] private float followSpeed = 30;
    [SerializeField] private float rotateSpeed = 100;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    IEnumerator colliderBufferCo;

    private Transform _followTarget;
    private Rigidbody _body;
    private Collider[] cols;
    public AudioSource source;
    public AudioClip slapObj;
    public AudioClip slapPerson;
    public AudioClip slapBaloon;
    bool canPlayBalloonPokeSound = true;


    void Start()
    {
        //Physics Movement
        _followTarget = controller.gameObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f;
        cols = gameObject.GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }
        colliderBufferCo = EnableColliders();
        StartCoroutine(colliderBufferCo);
        //Teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
        controllerActionGrip.action.performed += GripPress;
        controllerActionGrip.action.canceled += GripReleased;
        controllerActionTrigger.action.performed += TriggerPress;
        controllerActionTrigger.action.canceled += TriggerReleased;
    }
    public void PlaySlapPersonSound()
    {
        source.PlayOneShot(slapPerson);
    }
    public void PlaySlapObjectSound()
    {
        source.PlayOneShot(slapObj);
    }
    public void PlaySlapBalloonSound()
    {
        source.PlayOneShot(slapBaloon, 5);
    }
    public void PlayPokeBalloonSound()
    {
        if (canPlayBalloonPokeSound)
        {
            StartCoroutine(PlayBalloonPokeSound());
            source.PlayOneShot(slapBaloon, 1);
        }
    }
    IEnumerator PlayBalloonPokeSound()
    {
        canPlayBalloonPokeSound = false;
        yield return new WaitForSeconds(.3f);
        canPlayBalloonPokeSound = true;
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
        colliderBufferCo = EnableColliders();
        StartCoroutine(colliderBufferCo);
    }
    IEnumerator EnableColliders()
    {
        yield return new WaitForSeconds(1f);
        foreach (var col in cols)
        {
            col.enabled = true;
        }
    }
    private void FixedUpdate()
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