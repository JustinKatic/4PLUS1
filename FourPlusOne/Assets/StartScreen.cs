using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartScreen : MonoBehaviour
{
    public Fade FadeManager;
    public int TargetBuildIndex;

    private bool active = false;

    void Update()
    {
        if(active)
        {
            if(FadeManager.FadeDone)
            {
                SceneManager.LoadScene(TargetBuildIndex);
            }
        }

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartTransition();
        }
    }

    public void StartTransition()
    {
        FadeManager.FadeOut();
        active = true;
    }
}
