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

    public event Action onCatchInEndzone;
    public void CatchInEndzone()
    {
        if (onCatchInEndzone != null)
        {
            onCatchInEndzone();
        }
    }

    public event Action onPlayerLost;
    public void PlayerLost()
    {
        if (onCatchInEndzone != null)
        {
            onPlayerLost();
        }
    }


}
