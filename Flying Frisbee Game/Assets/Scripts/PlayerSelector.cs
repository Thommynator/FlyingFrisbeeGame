using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    /// layer used for raycast detection
    public LayerMask playerLayerMask;
    public Material playerMaterial;
    public Material selectedPlayerMaterial;

    private GameObject frisbee;

    // Start is called before the first frame update
    void Start()
    {
        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, playerLayerMask))
            // clicked on player in world?
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    GameObject hitPlayer = hit.transform.gameObject;
                    Debug.Log("Hit Player: " + hitPlayer.name);
                    DeselectAllPlayers();
                    SelectPlayer(hitPlayer);
                }
            }
        }
    }

    /// Sets the "canMove" property of all players to false and assigns the default player material to them.
    public void DeselectAllPlayers()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<PlayerMovement>().canMove = false;
            child.gameObject.GetComponent<MeshRenderer>().material = playerMaterial;
        }
    }

    /// Selects a given player, i.e. allow movement and assign different color
    private void SelectPlayer(GameObject player)
    {
        if (player != frisbee.GetComponent<Frisbee>().GetPlayerHoldingFrisbee())
        {
            player.GetComponent<PlayerMovement>().canMove = true;
            player.GetComponent<MeshRenderer>().material = selectedPlayerMaterial;
        }
    }
}
