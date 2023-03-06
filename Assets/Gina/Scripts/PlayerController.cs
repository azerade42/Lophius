using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private bool isNearCrystal = false; 

    //rocks
    public bool holdingRock = false;
    public GameObject rockThrow;
    public Transform rockHand;
    private bool isNearRock = false; 

//     // trident 
//     public bool holdingTrident = false;
//   //  public GameObject tridentShoot;
//     public Transform tridentHand;
//     private bool isNearTrident = false; 

    // ability to be active for 5 seconds
    public float isInvisible = 5f;
    public bool invisibile = false;

    // cooldown
    public float cooldownTime = 20f;
    private float nextFireTime = 0f;

    public TextMeshProUGUI invisibilityText;


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

        rockHand.gameObject.SetActive(false);
        rockThrow = Instantiate(rockThrow, itemSlot.position, Quaternion.identity);
        rockThrow.SetActive(false);

        //tridentHand.gameObject.SetActive(false);
       // tridentShoot = Instantiate(rockThrow, itemSlot.position, Quaternion.identity);
       // tridentShoot.SetActive(false);


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

        if (Input.GetKeyDown(KeyCode.E))
        {
            isNearCrystal = true; 
            isNearRock = true;
          //  isNearTrident = true;
        }

        //pickups
    
        if (Input.GetMouseButtonDown(0) && currentCount == 1)
        {
       
        // Throw crystal
            if(crystalHand.gameObject.activeInHierarchy)
            {
                CrystalThrow(); 
                animator.SetTrigger("ThrowCrystal");
           
                crystalHand.gameObject.SetActive(false);

                animator.SetBool("HoldCrystal", false);

            }
    //throw rock
            if(rockHand.gameObject.activeInHierarchy)
            {
                RockThrow(); 
                animator.SetTrigger("ThrowRock");
           
                rockHand.gameObject.SetActive(false);

                animator.SetBool("HoldRock", false);
            }

                //throw rock
            // if(tridentHand.gameObject.activeInHierarchy)
            // {
            //    // TridentShoot(); 
            //     animator.SetTrigger("ShootTrident");
           
            //     //tridentHand.gameObject.SetActive(false);

            //     //animator.SetBool("HoldTrident", false);
            // }
     
            
            
        }
        
    }

    private void CrystalThrow()
    {
    
        crystalThrow.transform.position = itemSlot.transform.position;
        //crystalThrow = Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
        crystalThrow.SetActive(true);
        crystalThrow.gameObject.GetComponent<Rigidbody>().AddRelativeForce(itemSlot.right * throwForce);
     
        UpdateCount(-1); 

    }
    private void RockThrow()
    {
    
        rockThrow.transform.position = itemSlot.transform.position;
        //crystalThrow = Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
        rockThrow.SetActive(true);
        rockThrow.gameObject.GetComponent<Rigidbody>().AddRelativeForce(itemSlot.right * throwForce);
     
        UpdateCount(-1); 

    }

   // private void TridentShoot()
   // {
    
        //tridentShoot.transform.position = itemSlot.transform.position;
        //crystalThrow = Instantiate(crystalThrow, itemSlot.position, Quaternion.identity);
       // tridentShoot.SetActive(true);
       
     
        //UpdateCount(-1); 

   // }

    

    // hidden state for when hidding in sea anemone
    void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("sea anemone"))
            {
                hidden = true;
                Debug.Log("hidden status = " + hidden);
            }

            if(other.CompareTag("Crystal") && currentCount <= 1 && isNearCrystal)
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
            
            if(other.CompareTag("Rock") && currentCount <= 1 && isNearRock)
            {
                // holding crystal animation
                animator.SetBool("HoldRock", true);
                holdingRock = true;

                // show crystal in hand
                rockHand.gameObject.SetActive(true);
                // destroy crystal pickup
                Destroy(other.gameObject);
                
                // is holding something
                UpdateCount(1);
            }
            // if(other.CompareTag("Trident") && currentCount <= 1 && isNearTrident)
            // {
            //     // holding crystal animation
            //     animator.SetBool("HoldTrident", true);
            //     holdingTrident = true;

            //     // show crystal in hand
            //     tridentHand.gameObject.SetActive(true);
            //     // destroy crystal pickup
            //     Destroy(other.gameObject);
                
            //     // is holding something
            //     UpdateCount(1);
            // }
    }

    public void UpdateCount(int count)
    {
        currentCount += count;
        Debug.Log("Count: " + currentCount);
    }

        // ability to be active for 5 seconds
    IEnumerator Ability(float isInvisible)
    {
        Debug.Log("Invisibility started");
        invisibile = true;
        invisibilityText.gameObject.SetActive(true);
        yield return new WaitForSeconds(isInvisible);
        invisibile = false;
        invisibilityText.gameObject.SetActive(false);
        Debug.Log("Invisibility ended");
    }
}
