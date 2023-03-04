using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    // get collider from player
    PlayerController playerCollider;

    public Transform groundCheck;
    public LayerMask ground;
    public bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        // player
        playerCollider = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && IsGrounded())
        {
            // change speed and height if crouching
            playerCollider.transform.localScale = new Vector3 (100f, 100f, 100f);
            playerCollider.speed = 3f;
            isCrouching = true;
            Debug.Log("crouching status = " + isCrouching);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl) && IsGrounded())
        {
            // change speed and height back to normal when NOT crouching
            playerCollider.transform.localScale = new Vector3 (100f, 200f, 100f);
            playerCollider.speed = 5f;
            isCrouching = false;
            Debug.Log("crouching status = " + isCrouching);
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .1f, ground);
    }
}
