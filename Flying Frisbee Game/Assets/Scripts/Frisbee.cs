﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frisbee : MonoBehaviour
{

    public State state;

    private State previousState;

    private GameObject frisbeeObject;

    private GameObject playerHoldingTheFrisbee;

    // Start is called before the first frame update
    void Start()
    {
        frisbeeObject = GameObject.FindGameObjectWithTag("Frisbee");
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (isStateChangeFresh(state, State.AtPlayer))
        {
            AttachToPlayer(GameObject.Find("Player"), Vector3.right);
        }
        else if (isStateChangeFresh(state, State.Flying))
        {
            DetachFromPlayer();
        }
        else if (isStateChangeFresh(state, State.OnGround))
        {
            DetachFromPlayer();
        }
        else if (isStateChangeFresh(state, State.OutOfBounds))
        {
            DetachFromPlayer();
        }
    }

    /// Checks if a given state is equal to a desiredState and if it's changed to it for the first time.
    private bool isStateChangeFresh(State state, State desiredState)
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
    /// This means the frisbee will move together with the player.
    /// Its relative position to the attached object is defined by 'relativePosition'.
    public void AttachToPlayer(GameObject player, Vector3 relativePosition)
    {
        playerHoldingTheFrisbee = player;
        playerHoldingTheFrisbee.GetComponent<PlayerMovement>().canMove = false;

        transform.position = playerHoldingTheFrisbee.transform.position + relativePosition;
        FixedJoint fixedJoint = frisbeeObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = playerHoldingTheFrisbee.GetComponent<Rigidbody>();
        fixedJoint.breakForce = 250;

    }

    /// Detached the frisbee from the currently attached object.
    public void DetachFromPlayer()
    {
        if (playerHoldingTheFrisbee != null)
        {
            playerHoldingTheFrisbee.GetComponent<PlayerMovement>().canMove = true;
            playerHoldingTheFrisbee = null;
        }
        Destroy(GetComponent<FixedJoint>());
        state = State.Flying;
    }

    void OnJointBreak(float breakForce)
    {
        Debug.Log("The frisbee/player FixedJoint has broken, Force: " + breakForce);
        state = State.Flying;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "GroundPlane")
        {
            Debug.Log("Frisbee hit the ground.");
            state = State.OnGround;
        }

        if (collision.gameObject.tag == "Player")
        {
            AttachToPlayer(collision.gameObject, Vector3.right);
        }
    }

    public enum State
    {
        Flying,
        OnGround,
        AtPlayer,
        OutOfBounds
    }
}
