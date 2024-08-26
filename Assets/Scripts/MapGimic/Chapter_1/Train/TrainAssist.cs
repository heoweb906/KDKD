using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAssist : ObjectTrigger
{
    public TrainController[] trainControllers;
    public float fOneRoutDuration;
    public float fInterval;
    public Transform[] wayPoints;

    override public void ActivaObj() 
    {
        int index = 0;
        foreach (var train in trainControllers)
        {
            train.duration = fOneRoutDuration;

            if (index == 1) train.fStartinterval = fInterval * index + 0.08f;
            else train.fStartinterval = fInterval * index;
            train.Movetrain();
            index++;
            train.waypoints = wayPoints;
        }
    }



}
