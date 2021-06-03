using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestComicTextFX : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            anim.enabled = true;
        }
    }
}
