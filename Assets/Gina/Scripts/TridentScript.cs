using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentScript : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    bool hasPlayer = false;
    public bool beingCarried = false;
    private bool touched = false;

    // player
    private PlayerController playerController;

    // awake
    void Awake()
    {
        // player
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
       float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 2.5f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        } 
        if (hasPlayer && (Input.GetKeyDown(KeyCode.E)) && (gameObject.tag == "Trident"))
        {
            Debug.Log("Trident");
            GetComponent<Rigidbody>().isKinematic = true;
            beingCarried = true;
            transform.parent = playerCam;
        }
        if (beingCarried)
        {
            if (touched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                touched = false;
            }
        }

        void OnTriggerEnter()
        {
            if (beingCarried)
            {
                touched = true;
            }
        }
    }
}
