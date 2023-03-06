using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFlash : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    public float throwForce = 10;
    public float flashSpeed = 5;
    public int flash = 0;
    bool hasPlayer = false;
    bool beingCarried = false;
    private bool touched = false;

    // player
    private PlayerController playerController;

    public AudioSource audioSource;
    public AudioClip hitMarshy;

    // awake
    void Awake()
    {
        // player
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        if (hasPlayer && (Input.GetKeyDown(KeyCode.E)) && (gameObject.tag == "throw") && playerController.currentCount <= 1)
        {
            Debug.Log("throw");
            playerController.UpdateCount(1);
            GetComponent<Rigidbody>().isKinematic = true;
            beingCarried = true;
            transform.parent = playerCam;
        }
        else if (playerController.currentCount > 1)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            beingCarried = false;
            transform.parent = null;
            playerController.UpdateCount(-1);
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
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce * flashSpeed);
                GetComponent<Rigidbody>().useGravity = true;
                playerController.UpdateCount(-1);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
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