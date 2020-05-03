using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VertStackDefense : MonoBehaviour
{
    public float markDistance = 2.0f;

    public GameObject playerToDefend;

    private GameObject frisbeeObject;

    private Vector3 defensePosition;

    private Vector2 perlinNoise;

    // Start is called before the first frame update
    void Start()
    {
        perlinNoise = new Vector2(Random.Range(0, 1000), Random.Range(0, 1000));
        frisbeeObject = GameObject.FindGameObjectWithTag("Frisbee");
    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerToDefendHoldingTheFrisbee();
        GetComponent<NavMeshAgent>().SetDestination(GetDefensePosition());
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
        Vector3 throwVector = GameObject.Find("AimPlane").GetComponent<DragAim>().GetThrowDistanceVector().normalized;
        Vector3 randomOffset = new Vector3(GetRandomPerlinNoiseValue(), 0, GetRandomPerlinNoiseValue());

        // defend player, which is holding the frisbee
        if (IsPlayerToDefendHoldingTheFrisbee())
        {
            if (frisbeeObject.GetComponent<Frisbee>().throwSide == Frisbee.ThrowSide.LEFT)
            {
                return playerToDefend.transform.position + ((throwVector + new Vector3(-1, 0, 1).normalized) / 2).normalized * markDistance;
            }
            else
            {
                return playerToDefend.transform.position + ((throwVector + new Vector3(1, 0, 1).normalized) / 2).normalized * markDistance;
            }
        }
        // defend player on the field (without frisbee)
        else
        {
            if (frisbeeObject.GetComponent<Frisbee>().throwSide == Frisbee.ThrowSide.LEFT)
            {
                return playerToDefend.transform.position + ((-throwVector + new Vector3(1, 0, -1).normalized + randomOffset) / 3).normalized * markDistance;
            }
            else
            {
                return playerToDefend.transform.position + ((-throwVector + new Vector3(-1, 0, -1).normalized + randomOffset) / 3).normalized * markDistance;
            }
        }
    }

    private float GetRandomPerlinNoiseValue()
    {
        float randomValue = Mathf.PerlinNoise(perlinNoise.x, perlinNoise.y);
        float stepWidth = 0.002f;
        perlinNoise += new Vector2(stepWidth, stepWidth);
        if (perlinNoise.x > 1000 || perlinNoise.y > 1000)
        {
            perlinNoise = new Vector2(0, 0);
        }
        return 2 * (randomValue - 0.5f);
    }
}
