using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public float WaitTimeBefore = 5f;
    public Fade FadeController;

    public bool hasFaded;

    private float timer = 0;

    public void OnEnable()
    {
        FadeController.FadeIn();
        timer = 0;
        hasFaded = false;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if(timer >= WaitTimeBefore)
        {
            if (hasFaded)
            {
                FadeController.FadeOut();
                hasFaded = true;
            }
            else if(FadeController.FadeDone)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
