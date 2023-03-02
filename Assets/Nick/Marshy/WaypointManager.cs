using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class WaypointManager : MonoBehaviour
{
    public Transform [] _trackedWaypoints;

    public Color [] _rainbow;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < _trackedWaypoints.Length; i++)
        {
            Gizmos.color = _rainbow[i % _rainbow.Length];
            Gizmos.DrawSphere(_trackedWaypoints[i].position, 0.5f);
            //Gizmos.color = Color.white;

            if (i !=  _trackedWaypoints.Length - 1)
                Gizmos.DrawLine(_trackedWaypoints[i].position, _trackedWaypoints[i + 1].position);
            else
                Gizmos.DrawLine(_trackedWaypoints[i].position, _trackedWaypoints[0].position);
        }
    }

#endif
}
