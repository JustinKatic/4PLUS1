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
    [Min(1)]
    public float HealthBeforeBreak = 150f;

    [Tooltip("How long a mole will stay ")]
    public float StayDuration = 2;
    
    public float TimeBeforePopout = 2f;
    public float PopoutTimeVariance = 0.25f;

    public bool Active = false;

    private Vector3 initalScale;
    private float timer = 0;
    private float popoutChange = 0;

    public UnityEvent OnMoleBreak;


    public void Start()
    {
        initalScale = transform.localScale;
        ScaleSpring = new Spring(1);

        popoutChange = TimeBeforePopout + Random.Range(-PopoutTimeVariance, PopoutTimeVariance);
    }

    public void Update()
    {
        //Make object scale down or up depending on Active
        transform.localScale = initalScale * ScaleSpring.UpdateSpring((Active)? 1f : 0,Stiffness, Damping,ValueThresh,VelocityThresh);
        //transform.localRotation = ScaleSpring.UpdateSpring(0, Stiffness, Damping, ValueThresh, VelocityThresh) * RotDirection;

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
    }

    public void MoleHit(SlapDetection slap)
    {
        if(Active)
        {
            if(slap != null)
            {
                Rotspring = new Spring(slap.PreviousHitVelocity.magnitude);
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
