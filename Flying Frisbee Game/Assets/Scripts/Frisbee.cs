using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frisbee : MonoBehaviour
{

    public float throwAngleDegree = 30f;

    public State state;

    private State previousState;

    private GameObject frisbeeObject;

    private GameObject playerHoldingTheFrisbee;

    public ThrowSide throwSide;


    // Start is called before the first frame update
    void Start()
    {
        frisbeeObject = GameObject.FindGameObjectWithTag("Frisbee");
        throwSide = ThrowSide.RIGHT;
        AttachToPlayer(GameObject.Find("Player"), throwSide);
    }


    void Update()
    {
        CheckIfOutOfBounds();

        if (Input.GetMouseButtonDown(1))
        {
            SwitchThrowSide();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsStateChangeFresh(state, State.AT_PLAYER))
        {

        }
        else if (IsStateChangeFresh(state, State.FLYING))
        {
            DetachFromPlayer();
        }
        else if (IsStateChangeFresh(state, State.ON_GROUND))
        {

        }
        else if (IsStateChangeFresh(state, State.OUT_OF_BOUNDS))
        {

        }
    }

    /// Checks if a given state is equal to a desiredState and if it's changed to it for the first time.
    /// Basically: Checks if a state is changed for the first time.
    private bool IsStateChangeFresh(State state, State desiredState)
    {
        if (state == desiredState && previousState != desiredState)
        {
            previousState = state;
            return true;
        }
        else
        {
            return false;
        }

    }

    /// Attaches the frisbee to another GameObject (= player).
    public void AttachToPlayer(GameObject player, ThrowSide throwSide)
    {
        DetachFromPlayer();

        state = State.AT_PLAYER;
        playerHoldingTheFrisbee = player;
        playerHoldingTheFrisbee.GetComponent<PlayerMovement>().canMove = false;

        if (throwSide == ThrowSide.RIGHT)
        {
            transform.position = playerHoldingTheFrisbee.transform.position + Vector3.right;
        }
        else if (throwSide == ThrowSide.LEFT)
        {
            transform.position = playerHoldingTheFrisbee.transform.position + Vector3.left;
        }

        FixedJoint fixedJoint = frisbeeObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = playerHoldingTheFrisbee.GetComponent<Rigidbody>();
        fixedJoint.breakForce = 250;
    }

    /// Detaches the frisbee from the player who is holding it.
    public void DetachFromPlayer()
    {
        if (playerHoldingTheFrisbee != null)
        {
            playerHoldingTheFrisbee.GetComponent<PlayerMovement>().canMove = true;
            playerHoldingTheFrisbee = null;
        }
        Destroy(GetComponent<FixedJoint>());
        // for some reason the frisbee has some velocity after removing the FixedJoint, 
        // which makes the distance incorrect
        frisbeeObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        state = State.FLYING;
    }

    private void SwitchThrowSide()
    {
        if (throwSide == ThrowSide.RIGHT)
        {
            throwSide = ThrowSide.LEFT;
        }
        else if (throwSide == ThrowSide.LEFT)
        {
            throwSide = ThrowSide.RIGHT;
        }
        AttachToPlayer(playerHoldingTheFrisbee, throwSide);
    }

    private bool CheckIfOutOfBounds()
    {
        if (frisbeeObject.transform.position.y < -0.5)
        {
            state = State.OUT_OF_BOUNDS;
            Debug.Log("Frisbee is out of bounds.");
            return true;
        }
        return false;
    }

    void OnJointBreak(float breakForce)
    {
        Debug.Log("The frisbee/player FixedJoint has broken, Force: " + breakForce);
        state = State.FLYING;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "GroundPlane")
        {
            Debug.Log("Frisbee hit the ground.");
            state = State.ON_GROUND;
        }

        if (collision.gameObject.tag == "Player")
        {
            AttachToPlayer(collision.gameObject, throwSide);
        }
    }

    public enum State
    {
        FLYING,
        ON_GROUND,
        AT_PLAYER,
        OUT_OF_BOUNDS
    }

    public enum ThrowSide
    {
        LEFT,
        RIGHT

    }
}
