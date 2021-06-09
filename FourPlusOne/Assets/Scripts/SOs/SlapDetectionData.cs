using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SlapDetectionData : ScriptableObject
{
    public GameObject[] comicTxtToSpawn;

    public GameEvent LhandSlapPerson;
    public GameEvent RhandSlapPerson;

    public GameEvent LhandSlapObject;
    public GameEvent RhandSlapObject;

    public GameEvent LhandSlapBalloon;
    public GameEvent RhandSlapBalloon;

}
