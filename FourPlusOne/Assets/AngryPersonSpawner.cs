using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AngryPersonSpawner : MonoBehaviour
{
    public Transform Target;
    public GameObject AngryPersonPrefab;
    public Fade FadeController;

    public UnityEvent OnPlayerFailEvent;
    public UnityEvent OnFadededOut;

    public GameObject[] objectsToDisable;

    private bool done = false;

    private void Start()
    {
        AngryPerson angry = AngryPersonPrefab.GetComponent<AngryPerson>();
        if (angry != null)
        {
            angry.TargetTransform = Target;
            angry.WithTransform = OnPlayerFailEvent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0f, 1f) > 0.99f)
        {
            //SpawnPrefab
            Instantiate(AngryPersonPrefab, transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)), Quaternion.identity).SetActive(true);
        }

        if (done)
        {
            if (FadeController.FadeDone)
            {
                OnFadededOut.Invoke();
                enabled = false;
            }
        }
    }

    public void MoveTargetToTransform(Transform teleportPoint)
    {
        Target.gameObject.transform.position = teleportPoint.position;
    }

    public void ChangeTargetTransform(Transform newTarget)
    {
        Target = newTarget;
        AngryPerson angry = AngryPersonPrefab.GetComponent<AngryPerson>();
        if (angry != null)
        {
            angry.TargetTransform = Target;
            angry.WithTransform = OnPlayerFailEvent;
        }
    }

    public void WaveDone()
    {
        if (!done)
        {
            done = true;
            FadeController.FadeOut();
        }
    }
    public void DisableObjects()
    {
        foreach (var item in objectsToDisable)
        {
            item.SetActive(false);
        }
    }
}
