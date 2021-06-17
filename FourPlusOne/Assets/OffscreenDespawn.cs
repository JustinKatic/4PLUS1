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

    public float MaxAliveTime = 10f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= MaxAliveTime)
        {
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
}
