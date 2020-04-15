using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frisbee : MonoBehaviour
{

    public State state;

    private State previousState;

    private GameObject frisbeeObject;

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
            AttachToPlayer(GameObject.Find("Player"));
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

    //
    // Checks if a given state is equal to a desiredState and if it's changed to it for the first time.
    //
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

    public void AttachToPlayer(GameObject player)
    {
        transform.SetParent(player.transform);
        transform.localPosition = new Vector3(0.8f, 0.0f, -0.11f);
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void DetachFromPlayer()
    {
        Vector3 position = transform.position;
        transform.SetParent(null, true);
    }

    public enum State
    {
        Flying,
        OnGround,
        AtPlayer,
        OutOfBounds
    }
}
