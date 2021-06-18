using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    //public int Score = 0;
    public IntVariableSO Score;
    public bool ShouldResetScoreValueOnStart;

    public Text3D[] TextToUpdate;

    private void Start()
    {
        if (ShouldResetScoreValueOnStart)
            Score.value = 0;

        foreach (Text3D texts in TextToUpdate)
        {
            texts.Text = Score.value.ToString();
        }
    }
    public void AddScore(int points)
    {
        if (points == 0) return;
        Score.value += points;

        foreach (Text3D texts in TextToUpdate)
        {
            texts.Text = Score.value.ToString();
        }
    }

    public void SetScoreValueToZero()
    {
        Score.value = 0;

        foreach (Text3D texts in TextToUpdate)
        {
            texts.Text = Score.value.ToString();
        }
    }
}
