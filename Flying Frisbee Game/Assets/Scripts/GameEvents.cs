using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }


    public event Action onMovementManagerEnter;
    public void MovementManagerEnter()
    {
        if (onMovementManagerEnter != null)
        {
            onMovementManagerEnter();
        }
    }

    public event Action onMovementManagerExit;
    public void MovementManagerExit()
    {
        if (onMovementManagerExit != null)
        {
            onMovementManagerExit();
        }
    }

    public event Action onPlayerScoredPoint;
    public void PlayerScoredPoint()
    {
        if (onPlayerScoredPoint != null)
        {
            onPlayerScoredPoint();
        }
    }

    public event Action onOpponentScoredPoint;
    public void OpponentScoredPoint()
    {
        if (onOpponentScoredPoint != null)
        {
            onOpponentScoredPoint();
        }
    }


}
