using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // move
    public float speed = 5f;
    
    // ascend
    public float ascendSpeed = 3f;

    // descend
    public float descendSpeed = 3f;

    // rigidbody
    Rigidbody rb;

    // hidden when in sea anemone
    public static bool hidden = false;

    // count
    public int count = 0;
    public int currentCount;

    // check if crouching
    PlayerCrouch playerCrouch;

    void Awake()
    {
        playerCrouch = gameObject.GetComponent<PlayerCrouch>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // move
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(horizontal, 0, vertical);

        // ascend
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("space pressed");
            transform.Translate(0, Time.deltaTime * ascendSpeed, 0);
        }
        // decend
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("left shift pressed");
            transform.Translate(0, -Time.deltaTime * descendSpeed, 0);
        }
        //Debug.Log("hidden status = " + hidden);
    }

    // hidden state for when hidding in sea anemone
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sea anemone") && playerCrouch.isCrouching == true)
        {
            hidden = true;
            Debug.Log("hidden status = " + hidden);
            Debug.Log("crouching status = " + playerCrouch.isCrouching);
        }
        else
            hidden = false;
    }

    public void UpdateCount(int count)
    {
        currentCount += count;
        Debug.Log("Count: " + currentCount);
    }
}
