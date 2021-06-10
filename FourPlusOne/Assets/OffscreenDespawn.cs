using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenDespawn : MonoBehaviour
{
    [Tooltip("If we should destroy the object, or deactivate it")]
    public bool UseDestroy = true;
    public bool DestroyParent = false;
    [Min(1)]
    public int ParentDepth = 0;
    private IEnumerator destoryRoutine;

    private void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            Debug.Log("Invis");
            destoryRoutine = DeswpawnAfterTime();
            StartCoroutine(destoryRoutine);
        }
    }

    private void OnBecameVisible()
    {
        if (gameObject.activeSelf)
        {
            Debug.Log("Devis");

            if (destoryRoutine != null) StopCoroutine(destoryRoutine);
            destoryRoutine = null;
        }
    }

    IEnumerator DeswpawnAfterTime()
    {
        yield return new WaitForSeconds(5f);

        //Destroy correct object
        GameObject obj = gameObject;
        if (DestroyParent)
        {
            for (int i = 0; i < ParentDepth; i++)
            {
                if (obj.transform.parent != null)
                    obj = obj.transform.parent.gameObject;
            }
        }

        if (UseDestroy) Destroy(obj);
        else gameObject.SetActive(false);
    }
}
