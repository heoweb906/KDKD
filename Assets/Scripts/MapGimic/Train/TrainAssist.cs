using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAssist : MonoBehaviour
{
    public TrainController[] trainControllers;
    public float fOneRoutDuration;
    public float fInterval;
    public Transform[] wayPoints;


    private void Start()
    {
        int index = 0;
        foreach (var train in trainControllers)
        {
            train.duration = fOneRoutDuration;
            train.fStartinterval = fInterval * index;
            train.Movetrain();
            index++;
            train.waypoints = wayPoints;
        }

    }
}
