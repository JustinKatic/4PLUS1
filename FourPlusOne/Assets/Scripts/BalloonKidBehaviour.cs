using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonKidBehaviour : MonoBehaviour
{
    public AngryPerson angryPersonScript;
    public float timeToFollowPlayer;
    void Start()
    {
        StartCoroutine(DisableMovementAfterX());
    }

    IEnumerator DisableMovementAfterX()
    {
        yield return new WaitForSeconds(timeToFollowPlayer);
        angryPersonScript.enabled = false;
    }
}
