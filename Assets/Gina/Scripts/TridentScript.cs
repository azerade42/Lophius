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

    // light
    public GameObject Bullet_Emitter;
    public GameObject Bullet;
    public float Bullet_Forward_Force;

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
            if (Input.GetMouseButtonDown(0))
            {
                GameObject Temporary_Bullet_Handler;
                Temporary_Bullet_Handler = Instantiate(Bullet,Bullet_Emitter.transform.position,Bullet_Emitter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);
                Rigidbody Temporary_RigidBody;
                Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();
                Temporary_RigidBody.AddForce(transform.forward * Bullet_Forward_Force);
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
