using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WaypointManager : MonoBehaviour
{
    public GameObject waypointPrefab;
    public List<GameObject> waypoints = new List<GameObject>();

    public bool IsWaypointAvailable()
    {
        return waypoints.Count > 0;
    }

    public Vector3 GetNextWaypointPosition()
    {
        if (IsWaypointAvailable())
        {
            return waypoints[0].transform.position;
        }
        throw new NoWaypointAvailableException();
    }

    public void RemoveNextWaypoint()
    {
        if (IsWaypointAvailable())
        {
            Destroy(waypoints[0].gameObject);
            waypoints.RemoveAt(0);
        }
    }

    public void AddNewWaypoint(Vector3 position)
    {
        waypoints.Add(GameObject.Instantiate(waypointPrefab, position, Quaternion.identity));
    }

    public void RemoveLastWaypoint()
    {
        if (IsWaypointAvailable())
        {
            int index = waypoints.Count - 1;
            Destroy(waypoints[index].gameObject);
            waypoints.RemoveAt(waypoints.Count - 1);
        }
    }

    public void VisualizeWaypoints(bool show)
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (show)
            {
                // show waypoint
                waypoints[i].GetComponent<MeshRenderer>().enabled = true;

                // show number above waypoint
                waypoints[i].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                waypoints[i].GetComponentInChildren<TextMeshProUGUI>().SetText((i + 1).ToString());

            }
            else
            {
                // hide waypoint and number
                waypoints[i].GetComponent<MeshRenderer>().enabled = false;
                waypoints[i].GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }


    public class NoWaypointAvailableException : Exception
    {
        public NoWaypointAvailableException() : base() { }
    }
}
