using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Mole : MonoBehaviour
{
    public Spring ScaleSpring;

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

    private Collider collider;

    public UnityEvent OnMoleBreak;


    public void Start()
    {
        initalPos = transform.localPosition;
        initalScale = transform.localScale;
        ScaleSpring = new Spring(0);
        collider = GetComponent<Collider>();

        popoutChange = TimeBeforePopout + Random.Range(-PopoutTimeVariance, PopoutTimeVariance);
    }

    public void Update()
    {
        // Make object scale down or up depending on Active
        transform.localPosition = initalPos + ((transform.up * 0.15f) * ScaleSpring.UpdateSpring((Active)? 1f : 0,Stiffness, Damping,ValueThresh,VelocityThresh));
        transform.localScale = initalScale * (1f + (ScaleSpring.UpdateSpring((Active)? 1f : -1f,Stiffness, Damping,ValueThresh,VelocityThresh) * 0.2f));

        timer += Time.deltaTime;

        if (Active)
        {
            if (timer >= StayDuration)
            {
                timer = 0;
                Active = false;
                popoutChange = TimeBeforePopout + Random.Range(-PopoutTimeVariance, PopoutTimeVariance);
                collider.isTrigger = true;
            }
        }
        else
        {
            if (timer >= popoutChange)
            {
                timer = 0;
                Active = true;
                collider.isTrigger = false;
            }
        }

    }

    public void MoleHit(SlapDetection slap)
    {
        if(Active)
        {
            ScaleSpring.Value = 1.3f;

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
