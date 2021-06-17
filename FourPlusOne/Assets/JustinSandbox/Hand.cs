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

    [SerializeField] private float followSpeed = 200f;
    [SerializeField] private float rotateSpeed = 300f;
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

    public float minRotationDiff = 15f;
    public float rotationStrength = 40f;





    void Start()
    {
        //Physics Movement
        _followTarget = controller.gameObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f;
        _body.ResetInertiaTensor();
        _body.ResetCenterOfMass();

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
        var positionWithOffset = _followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        if (distance < 0.01)
        {
            transform.position = _followTarget.position;
        }
        else
        {

            _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
        }
        //Position


        // If the difference in angle is less than min, set the rotation
        if (Quaternion.Angle(_body.rotation, _followTarget.rotation) < minRotationDiff)
        {
            _body.MoveRotation(_followTarget.rotation);
        }
        else
        {
            // Constants relating to rotation strength. Could be calculated in awake, but leave here so strength can be changed in play mode
            float rotConst1 = (6 * rotationStrength) * (6 * rotationStrength) * 0.25f;
            float rotConst2 = 4.5f * rotationStrength;



            // Get the tracked rotation relitive to this, then conver it to axis angle
            Quaternion localTrackedRot = _followTarget.rotation * Quaternion.Inverse(transform.rotation);
            localTrackedRot.ToAngleAxis(out float xMag, out Vector3 x);
            x = x.normalized * Mathf.Deg2Rad;



            // Find the change in angular momentum
            Vector3 torque = rotConst1 * x * xMag - rotConst2 * _body.angularVelocity;
            // Transform by inertia tensor
            Quaternion rotInertiaToWorld = _body.inertiaTensorRotation * transform.rotation;
            torque = Quaternion.Inverse(rotInertiaToWorld) * torque;
            torque.Scale(_body.inertiaTensor);
            torque = rotInertiaToWorld * torque;



            // Apply torque
            _body.AddTorque(torque);
        }





    }
}


