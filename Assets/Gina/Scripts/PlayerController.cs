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

    // pickup iutems
    private bool keyDown;
    public Transform itemSlot;

    public float throwForce;
    public float throwUpwardForce; 
    public float flashSpeed = 5;

    public bool holdingCrystal = false;

    // Crystal instantiated that actually gets thrown
    public GameObject crystalThrow;

    // Crystal in hand
    public Transform crystalHand;


   // Vector3 m_NewForce;
 

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        crystalHand.gameObject.SetActive(false);
        crystalThrow = Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
        crystalThrow.SetActive(false);


        //animator.SetBool("Moving", true);
         //  m_NewForce = new Vector3(-5.0f, 1.0f, 0.0f);
        
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
        else
        {
            animator.SetBool("Moving", false);        
        }
        
        

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

        //pickups
    
        // Throw crystal
        if (Input.GetMouseButtonDown(0) && currentCount == 1 && crystalHand.gameObject.activeInHierarchy)
        {
          //  Rigidbody crystalRb = crystalThrow.GetComponent<Rigidbody>();
            
            Throw(); 
            animator.SetTrigger("ThrowCrystal");
           
            crystalHand.gameObject.SetActive(false);
            
           // Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
           // crystalThrow.transform.parent = itemSlot.transform;
            //crystalThrow.gameObject.GetComponent<Rigidbody>().velocity = itemSlot.transform.forward * 500f;
         //   crystalRb.AddForce(throwForce, ForceMode.Impulse);
            animator.SetBool("HoldCrystal", false);
            
            
        }
        
    }

    private void Throw()
    {
      // Rigidbody crystalRb = crystalThrow.GetComponent<Rigidbody>();
       //var newProjectile =  Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
       //Rigidbody crystalRb = crystalThrow.GetComponent<Rigidbody>();
       //Vector3 forcetoAdd = itemSlot.transform.forward * throwForce + transform.up * throwUpwardForce; 
        //crystalThrow.gameObject.GetComponent<Rigidbody>().velocity = itemSlot.forward * 500f;
        //crystalThrow.gameObject.GetComponent<Rigidbody>().AddForce(forcetoAdd, ForceMode.Impulse);
       // crystalRb.AddForce(forcetoAdd, ForceMode.Impulse); 
      // newProjectile.velocity = transform.TransformDirection( Vector3( 0, 0, speed));

        crystalThrow.transform.position = itemSlot.transform.position;
        //crystalThrow = Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
        crystalThrow.SetActive(true);
       // Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
        // //crystalThrow.gameObject.GetComponent<Rigidbody>().AddForce(itemSlot.forward * throwForce * flashSpeed);
        crystalThrow.gameObject.GetComponent<Rigidbody>().AddRelativeForce(itemSlot.right * throwForce);
        // var dir : Vector3 = itemSlot.forward;
        // var bulletInstant : Rigidbody = Instantiate(crystalThrow, itemSlot.position, itemSlot.rotation);
        // bulletInstant.AddForce(dir * 2000);
        UpdateCount(-1); 

    }

    

    // hidden state for when hidding in sea anemone
    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("sea anemone"))
            {
                hidden = true;
                Debug.Log("hidden status = " + hidden);
            }

            if( other.CompareTag("Crystal") && currentCount <= 1)
            {
                // holding crystal animation
                animator.SetBool("HoldCrystal", true);
                holdingCrystal = true;

                // show crystal in hand
                crystalHand.gameObject.SetActive(true);
                // destroy crystal pickup
                Destroy(other.gameObject);
                
                // is holding something
                UpdateCount(1);
            }
    }

    public void UpdateCount(int count)
    {
        currentCount += count;
        Debug.Log("Count: " + currentCount);
    }
}
