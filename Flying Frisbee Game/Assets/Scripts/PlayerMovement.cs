using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8;
    public bool canMove;

    private Animator animator;

    private bool canPlanPath;
    private NavMeshAgent navMeshAgent;

    private PlayerSelector playerSelector;

    private WaypointManager waypointManager;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onMovementManagerEnter += () => { canPlanPath = true; };
        GameEvents.current.onMovementManagerExit += () => { canPlanPath = false; };

        // assign it only once in the beginning to save performance during game
        mainCamera = Camera.main;

        navMeshAgent = GetComponent<NavMeshAgent>();
        playerSelector = gameObject.GetComponentInParent<PlayerSelector>();
        waypointManager = gameObject.GetComponentInChildren<WaypointManager>();
        animator = gameObject.GetComponentInChildren<Animator>();
        canPlanPath = false;
    }

    void Update()
    {
        if (!PauseMenu.isGamePaused && canPlanPath && IsPlayerSelected())
        {
            // add new waypoints
            if (Input.GetMouseButtonDown(0))
            {
                try
                {
                    waypointManager.AddNewWaypoint(GetMousePositionOnPlaneAsWorldCoordinate());
                }
                catch (NoHitOnGroundPlaneException ex)
                {
                    Debug.Log(ex.Message);
                }
            }

            // remove last waypoint
            else if (Input.GetMouseButtonDown(1))
            {
                waypointManager.RemoveLastWaypoint();
            }
        }
        waypointManager.VisualizeWaypoints(IsPlayerSelected());

    }

    void FixedUpdate()
    {

        Vector3 manualMovementVelocity = Vector3.zero;
        if (canMove)
        {
            // manual movement
            manualMovementVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
            navMeshAgent.Move(manualMovementVelocity * Time.deltaTime);

            // automated movement to waypoints
            try
            {
                Vector3 nextWaypoint = waypointManager.GetNextWaypointPosition();
                navMeshAgent.SetDestination(nextWaypoint);
                if ((transform.position - nextWaypoint).sqrMagnitude < 4)
                {
                    waypointManager.RemoveNextWaypoint();
                }
                animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
            }
            catch (WaypointManager.NoWaypointAvailableException)
            {
                navMeshAgent.ResetPath();
            }
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        animator.SetFloat("speed", Mathf.Max(manualMovementVelocity.magnitude, navMeshAgent.velocity.magnitude));
    }

    private Vector3 GetMousePositionOnPlaneAsWorldCoordinate()
    {
        Plane plane = new Plane(Vector3.up, 0.0f);

        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Groundplane Layer"))
            {
                return hitInfo.point;
            }
        }

        // throw exception if player did not click on the ground plane (e.g. selected a player)
        if (hitInfo.collider)
        {
            throw new NoHitOnGroundPlaneException("Player didn't click on the ground plane. Instead: " + hitInfo.collider.name);
        }
        else
        {
            throw new NoHitOnGroundPlaneException("Player didn't click on the ground plane.");
        }
    }

    private bool IsPlayerSelected()
    {
        return GameObject.ReferenceEquals(playerSelector.GetSelectedPlayer(), this.gameObject);
    }

    public class NoHitOnGroundPlaneException : Exception
    {
        public NoHitOnGroundPlaneException(string message) : base(message) { }
    }

}
