using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Mole : MonoBehaviour
{
    public Spring ScaleSpring;
    public Spring Rotspring;
    public Quaternion RotDirection = Quaternion.identity;

    [Header("Spring Values")]
    public float Stiffness = 0;
    public float Damping = 0;
    public float ValueThresh = 0;
    public float VelocityThresh = 0;

    [Header("Mole Values")]

    public float HealthBeforeBreak = 150f;

    [Tooltip("How long a mole will stay ")]
    public float StayDuration = 2;
    
    public float TimeBeforePopout = 2f;
    public float PopoutTimeVariance = 0.25f;

    public bool Active = false;

    private Vector3 initalPos;
    private Vector3 initalScale;
    private float timer = 0;
    private float popoutChange = 0;

    public UnityEvent OnMoleBreak;


    public void Start()
    {
        initalPos = transform.localPosition;
        initalScale = transform.localScale;
        ScaleSpring = new Spring(0);
        Rotspring = new Spring(0);

        popoutChange = TimeBeforePopout + Random.Range(-PopoutTimeVariance, PopoutTimeVariance);
    }

    public void Update()
    {
        //Make object scale down or up depending on Active
        transform.localPosition = initalPos + ((transform.up * 0.15f) * ScaleSpring.UpdateSpring((Active)? 1f : 0,Stiffness, Damping,ValueThresh,VelocityThresh));
        transform.localScale = initalScale * (1f + (ScaleSpring.UpdateSpring((Active)? 1f : -1f,Stiffness, Damping,ValueThresh,VelocityThresh) * 0.2f));
        Rotspring.UpdateSpring(0, Stiffness, Damping, ValueThresh, VelocityThresh);
        transform.localRotation = Quaternion.Euler(Vector3.Slerp(Vector3.zero, RotDirection.eulerAngles,Rotspring.Value));

        timer += Time.deltaTime;

        if (Active)
        {
            if (timer >= StayDuration)
            {
                timer = 0;
                Active = false;
                popoutChange = TimeBeforePopout + Random.Range(-PopoutTimeVariance, PopoutTimeVariance);
            }
        }
        else
        {
            if (timer >= popoutChange)
            {
                timer = 0;
                Active = true;
            }
        }

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Rotspring = new Spring(1);
            RotDirection = Quaternion.FromToRotation(transform.up, new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f) * 0.2f)) * transform.localRotation;
            MoleHit(null);
        }
    }

    public void MoleHit(SlapDetection slap)
    {
        if(Active)
        {
            if(slap != null)
            {
                Rotspring = new Spring(slap.PreviousHitVelocity.magnitude);
                RotDirection = Quaternion.FromToRotation(transform.up,slap.PreviousHitVelocity.normalized * 0.2f) * transform.localRotation;
            }

            timer = 0;
            Active = false;
            popoutChange = TimeBeforePopout + Random.Range(-PopoutTimeVariance, PopoutTimeVariance);

            HealthBeforeBreak -= 10;

            if(HealthBeforeBreak <= 0)
            {
                OnMoleBreak.Invoke();
            }
        }
    }
}
