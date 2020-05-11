using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WaypointManager : MonoBehaviour
{
    public GameObject waypointPrefab;
    public List<GameObject> waypoints = new List<GameObject>();

    private LineRenderer waypointLineConnector;

    void Start()
    {
        waypointLineConnector = GetComponent<LineRenderer>();
    }

    public bool IsWaypointAvailable()
    {
        return waypoints.Count > 0;
    }

    public void AddNewWaypoint(Vector3 position)
    {
        waypoints.Add(GameObject.Instantiate(waypointPrefab, position, Quaternion.identity));

        UpdateLinesBetweenWaypoints();
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
        RemoveWaypointAtIndex(0);
    }

    public void RemoveLastWaypoint()
    {
        RemoveWaypointAtIndex(waypoints.Count - 1);
    }

    private void RemoveWaypointAtIndex(int index)
    {
        if (IsWaypointAvailable())
        {
            Destroy(waypoints[index].gameObject);
            waypoints.RemoveAt(index);

            UpdateLinesBetweenWaypoints();
        }
    }

    public void VisualizeWaypoints(bool show)
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            TextMeshProUGUI text = waypoints[i].GetComponentInChildren<TextMeshProUGUI>();
            if (show)
            {
                // show waypoint
                waypoints[i].GetComponent<MeshRenderer>().enabled = true;

                // show number above waypoint
                text.enabled = true;
                text.SetText((i + 1).ToString());

                // show line between waypoints
                waypointLineConnector.enabled = true;
            }
            else
            {
                // hide waypoint and number
                waypoints[i].GetComponent<MeshRenderer>().enabled = false;
                text.enabled = false;

                // hide line between waypoints
                waypointLineConnector.enabled = false;
            }
        }
    }

    private void UpdateLinesBetweenWaypoints()
    {
        Vector3[] positions = new Vector3[waypoints.Count];
        for (int i = 0; i < waypoints.Count; i++)
        {
            positions[i] = waypoints[i].transform.position;
        }

        waypointLineConnector.positionCount = positions.Length;
        waypointLineConnector.SetPositions(positions);
    }

    public class NoWaypointAvailableException : Exception
    {
        public NoWaypointAvailableException() : base() { }
    }
}
