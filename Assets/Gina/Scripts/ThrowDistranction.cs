using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDistranction : MonoBehaviour
{
    public GameObject myHands; //reference to your hands/the position where you want your object to go
    bool canpickup; //a bool to see if you can or cant pick up the item
    GameObject ObjectIwantToPickUp; // the gameobject onwhich you collided with
    bool hasItem; // a bool to see if you have an item in your hand

    public int count = 0;
    public Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {
        canpickup = false;    //setting both to false
        hasItem = false;
    }
 
    // Update is called once per frame
    void Update()
    {
        if(canpickup == true) // if you enter thecollider of the objecct
        {
            if (Input.GetKey(KeyCode.E) && count <= 1)  // can be e or any key
            {
                //transform.parent = playerCam;
                count += 1;
                Debug.Log("Count: " + count);
                hasItem = true;
                ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                ObjectIwantToPickUp.transform.position = myHands.transform.position; // sets the position of the object to your hand position
                ObjectIwantToPickUp.transform.parent = myHands.transform; //makes the object become a child of the parent so that it moves with the hands
            }
            /*else if (Input.GetKey(KeyCode.E) && count > 1)  // can be e or any key
            {
                //transform.parent = playerCam;
                count -= 1;
                Debug.Log("Count: " + count);
                hasItem = true;;
                //ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                //ObjectIwantToPickUp.transform.position = myHands.transform.position; // sets the position of the object to your hand position
                ObjectIwantToPickUp.transform.parent = null; //makes the object become a child of the parent so that it moves with the hands
            }*/
        }
        if (Input.GetKey(KeyCode.T) && hasItem == true) // if you have an item and get the key to remove the object, again can be any key
        {
            ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again
         
            ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
        }
    }
    private void OnTriggerEnter(Collider other) // to see when the player enters the collider
    {
        if(other.gameObject.tag == "flash") //on the object you want to pick up set the tag to be anything, in this case "object"
        {
            canpickup = true;  //set the pick up bool to true
            Debug.Log("Can pick up = " + canpickup);
            Debug.Log("gameobject tag = " + other.gameObject.tag);
            ObjectIwantToPickUp = other.gameObject; //set the gameobject you collided with to one you can reference
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canpickup = false; //when you leave the collider set the canpickup bool to false
        Debug.Log("Have item = " + hasItem);
    }

    /*public Transform weaponSlot;
 
    private GameObject crowbar;
    public GameObject crowbarDrop;
    private GameObject axe;
    public GameObject axeDrop;
    public bool weaponEquipped;
 
    // Start is called before the first frame update
    public void Start()
    {
        weaponEquipped = false;
 
        crowbar = GameObject.Find("CrowBar_Hand");
        crowbar.SetActive(false);
        axe = GameObject.Find("Axe_Hand");
        axe.SetActive(false);
    }
 
    // Update is called once per frame
    public void Update()
    {   //Hide crowbar child object and spawn another on drop
     
        if (Input.GetKey(KeyCode.F) && weaponEquipped && crowbar)
        {
            crowbar.SetActive(false);
            weaponEquipped = false;
            Instantiate(crowbarDrop, weaponSlot.position, Quaternion.identity);
        }
 
 
        else if (Input.GetKey(KeyCode.F) && weaponEquipped && axe)
        {
           
            axe.SetActive(false);
            weaponEquipped = false;
            Instantiate(axeDrop, weaponSlot.position, Quaternion.identity);
        }  
 
    }  
   
 
 
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Crowbar") && Input.GetKey(KeyCode.E) && !weaponEquipped)
        {
            crowbar.SetActive(true);
            Destroy(other.gameObject);
            weaponEquipped = true;
        }
 
        if (other.CompareTag("Axe") && Input.GetKey(KeyCode.E) && !weaponEquipped)
        {
            axe.SetActive(true);
            Destroy(other.gameObject);
            weaponEquipped = true;
        }
       
    }*/
}
