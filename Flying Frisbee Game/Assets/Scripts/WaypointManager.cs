using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaypointManager : MonoBehaviour
{

    public List<Vector3> waypointPositions = new List<Vector3>();

    public GameObject waypointPrefab;

    public bool IsWaypointAvailable()
    {
        return waypointPositions.Count > 0;
    }

    public Vector3 GetNextWaypoint()
    {
        if (IsWaypointAvailable())
        {
            return waypointPositions[0];
        }
        throw new NoWaypointAvailableException();
    }

    public void RemoveNextWaypoint()
    {
        if (IsWaypointAvailable())
        {
            waypointPositions.RemoveAt(0);
        }
    }

    public void AddNewWaypoint(Vector3 position)
    {
        waypointPositions.Add(position);
    }

    public void RemoveLastWaypoint()
    {
        if (IsWaypointAvailable())
        {
            waypointPositions.RemoveAt(waypointPositions.Count - 1);
        }
    }

    public void VisualizeWaypoints(bool show)
    {
        foreach (Transform waypoint in transform)
        {
            GameObject.Destroy(waypoint.gameObject);
        }
        if (show)
        {
            foreach (Vector3 waypointPosition in waypointPositions)
            {
                GameObject waypoint = GameObject.Instantiate(waypointPrefab, waypointPosition, Quaternion.identity);
                waypoint.transform.parent = this.transform;
            }
        }
    }


    public class NoWaypointAvailableException : Exception
    {
        public NoWaypointAvailableException() : base() { }
    }
}
