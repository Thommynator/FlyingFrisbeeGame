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
    private GameObject selectedPlayer;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // assign it only once in the beginning to save performance during game
        frisbee = GameObject.FindGameObjectWithTag("Frisbee");
        mainCamera = Camera.main;

        selectedPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, playerLayerMask))
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

    /// Returns the currently selected player. Can be null if none is selected.
    public GameObject GetSelectedPlayer()
    {
        return selectedPlayer;
    }

    /// Sets the "canMove" property of all players to false and assigns the default player material to them.
    public void DeselectAllPlayers()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<PlayerMovement>().canMove = false;
            Transform playerCapsule = child.GetChild(0).GetChild(0);
            playerCapsule.GetComponent<MeshRenderer>().material = playerMaterial;
            selectedPlayer = null;
        }
    }

    /// Selects a given player, i.e. allow movement and assign different color
    private void SelectPlayer(GameObject player)
    {
        if (player != frisbee.GetComponent<Frisbee>().GetPlayerHoldingFrisbee())
        {
            player.GetComponentInChildren<Animator>().SetTrigger("selected");
            player.GetComponent<PlayerMovement>().canMove = true;
            Transform playerCapsule = player.transform.GetChild(0).GetChild(0);
            playerCapsule.GetComponent<MeshRenderer>().material = selectedPlayerMaterial;
            selectedPlayer = player;
        }
    }

}
