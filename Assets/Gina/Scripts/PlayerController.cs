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
    public bool hidden = false;

    public bool isMoving = false;

    // count
    public int count = 0;
    public int currentCount;

    // animator
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        //animator.SetBool("Moving", true);
        
    }

    // Update is called once per frame
    void Update()
    {
        // move
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(horizontal, 0, vertical);

         
        //animator.SetFloat("Speed", speed);
      //  animator.SetBool("Moving", vertical != 0 || horizontal != 0);
        if(horizontal != 0 || vertical != 0)
        {
            animator.SetBool("Moving", true);
        }
        else{
            animator.SetBool("Moving", false)
;        }
        
        

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

        
    }

    // hidden state for when hidding in sea anemone
    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("sea anemone"))
            {
                hidden = true;
                Debug.Log("hidden status = " + hidden);
            }

            if(other.gameObject.CompareTag("throw"))
            {
                if (Input.GetKeyDown(KeyCode.E));
            }
    }

    public void UpdateCount(int count)
    {
        currentCount += count;
        Debug.Log("Count: " + currentCount);
    }
}
