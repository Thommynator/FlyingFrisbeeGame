using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertStackDefense : MonoBehaviour
{
    public GameObject playerToDefend;

    private float followSpeed = 0.01f;

    private GameObject frisbeeObject;

    private Vector3 defensePosition;

    // Start is called before the first frame update
    void Start()
    {
        frisbeeObject = GameObject.FindGameObjectWithTag("Frisbee");
    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerToDefendHoldingTheFrisbee();
        transform.position = Vector3.Lerp(transform.position, GetDefensePosition(), followSpeed);
    }

    private bool IsPlayerToDefendHoldingTheFrisbee()
    {
        GameObject playerHoldingFrisbee = frisbeeObject.GetComponent<Frisbee>().GetPlayerHoldingFrisbee();
        if (playerHoldingFrisbee != null && playerToDefend == playerHoldingFrisbee)
        {
            return true;
        }
        return false;
    }

    private Vector3 GetDefensePosition()
    {
        float markDistance = 3.0f;
        if (IsPlayerToDefendHoldingTheFrisbee())
        {
            if (frisbeeObject.GetComponent<Frisbee>().throwSide == Frisbee.ThrowSide.LEFT)
            {
                return playerToDefend.transform.position + new Vector3(-1, 0, 1).normalized * markDistance;
            }
            else if (frisbeeObject.GetComponent<Frisbee>().throwSide == Frisbee.ThrowSide.RIGHT)
            {
                return playerToDefend.transform.position + new Vector3(1, 0, 1).normalized * markDistance;
            }
        }
        else
        {
            if (frisbeeObject.GetComponent<Frisbee>().throwSide == Frisbee.ThrowSide.LEFT)
            {
                return playerToDefend.transform.position + new Vector3(1, 0, -1).normalized * markDistance;
            }
        }
        return playerToDefend.transform.position + new Vector3(-1, 0, -1).normalized * markDistance;
    }
}
