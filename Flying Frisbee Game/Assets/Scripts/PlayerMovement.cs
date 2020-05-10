using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8;
    public bool canMove;

    private bool canPlanPath;
    private NavMeshAgent navMeshAgent;

    private PlayerSelector playerSelector;

    private WaypointManager waypointManager;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onMovementManagerEnter += EnablePathPlanning;
        GameEvents.current.onMovementManagerExit += DisablePathPlanning;

        navMeshAgent = GetComponent<NavMeshAgent>();
        playerSelector = gameObject.GetComponentInParent<PlayerSelector>();
        waypointManager = gameObject.GetComponentInChildren<WaypointManager>();
        canPlanPath = false;
    }

    void Update()
    {
        if (canPlanPath && IsPlayerSelected())
        {
            if (Input.GetMouseButtonDown(0))
            {
                waypointManager.AddNewWaypoint(GetMousePositionOnPlaneAsWorldCoordinate());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                waypointManager.RemoveLastWaypoint();
            }
        }
        waypointManager.VisualizeWaypoints(IsPlayerSelected());

    }

    void FixedUpdate()
    {
        // manual movement
        if (canMove)
        {
            Vector3 velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
            navMeshAgent.Move(velocity * Time.deltaTime);
        }

        // automated movement to waypoints
        try
        {
            if (waypointManager.IsWaypointAvailable())
            {
                Vector3 nextWaypoint = waypointManager.GetNextWaypoint();
                GetComponent<NavMeshAgent>().SetDestination(nextWaypoint);
                if (Vector3.Distance(transform.position, nextWaypoint) < 2)
                {
                    waypointManager.RemoveNextWaypoint();
                }

            }
        }
        catch (WaypointManager.NoWaypointAvailableException)
        {
            // do nothing
        }
    }

    private void EnablePathPlanning()
    {
        canPlanPath = true;
    }

    private void DisablePathPlanning()
    {
        canPlanPath = false;
    }

    private Vector3 GetMousePositionOnPlaneAsWorldCoordinate()
    {
        Plane plane = new Plane(Vector3.up, 0.0f);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    private bool IsPlayerSelected()
    {
        return GameObject.ReferenceEquals(playerSelector.GetSelectedPlayer(), this.gameObject);
    }

}
