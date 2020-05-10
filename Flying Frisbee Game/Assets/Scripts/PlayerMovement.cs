using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8;
    public bool canMove;

    public GameObject waypointPrefab;
    public List<Vector3> plannedPath;
    private bool canPlanPath;
    private NavMeshAgent navMeshAgent;

    private PlayerSelector playerSelector;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onMovementManagerEnter += EnablePathPlanning;
        GameEvents.current.onMovementManagerExit += DisablePathPlanning;

        navMeshAgent = GetComponent<NavMeshAgent>();
        playerSelector = gameObject.GetComponentInParent<PlayerSelector>();
        canPlanPath = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddNewWaypoint(GetMousePositionOnPlaneAsWorldCoordinate());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RemoveLastWaypoint();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
            navMeshAgent.Move(velocity * Time.deltaTime);
        }


        if (plannedPath != null && plannedPath.Count > 0)
        {
            GetComponent<NavMeshAgent>().SetDestination(plannedPath[0]);
            if (Vector3.Distance(transform.position, plannedPath[0]) < 2)
            {
                plannedPath.RemoveAt(0);
            }
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

    private void AddNewWaypoint(Vector3 position)
    {
        if (canPlanPath && isPlayerSelected())
        {
            plannedPath.Add(position);
        }
    }

    private void RemoveLastWaypoint()
    {
        if (canPlanPath && isPlayerSelected())
        {
            if (plannedPath.Count > 0)
            {
                plannedPath.RemoveAt(plannedPath.Count - 1);
            }
        }
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

    private bool isPlayerSelected()
    {
        return GameObject.ReferenceEquals(playerSelector.GetSelectedPlayer(), this.gameObject);
    }

}
