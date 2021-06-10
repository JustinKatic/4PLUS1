using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlapAMoleTable : MonoBehaviour
{
    public List<GameObject> Moles = new List<GameObject>();
    public UnityEvent OnMolesEmpty;

    public int moleCount = 0;

    public void Start()
    {
        moleCount = Moles.Count;
    }

    public void SubtractMole()
    {
        moleCount--;

        if(moleCount <= 0)
        {
            OnMolesEmpty.Invoke();
        }
    }
}
