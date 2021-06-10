using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    public int Score = 0;
    public float GameLengthInSeconds = 360;

    public Text3D[] TextToUpdate;

    public void AddScore(int points)
    {
        if (points == 0) return;
        Score += points;

        foreach(Text3D texts in TextToUpdate)
        {
            texts.Text = Score.ToString();
        }
    }
}
