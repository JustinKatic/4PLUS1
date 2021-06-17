using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    public TextMeshProUGUI FpsText;

    float averageFPS = 0;

    // Update is called once per frame
    void Update()
    {
        FpsText.text = "DA FPS: " + UpdateFPS().ToString();
    }

    float UpdateFPS()
    {
        averageFPS += ((Time.deltaTime / Time.timeScale) - averageFPS) * 0.03f;

        return Mathf.Floor(1f/averageFPS);
    }
}