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

    // count
    public int count = 0;
    public int currentCount;

    // animator
    private Animator animator;

    // ability to be active for 5 seconds
    public float isInvisible = 5f;
    public bool invisibile = false;

    // cooldown
    public float cooldownTime = 20f;
    private float nextFireTime = 0f;

    public TextMeshProUGUI invisibilityText;


    // Start is called before the first frame update
    void Start()
    {
        // rigidbody
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    
    }

    // Update is called once per frame
    void Update()
    {
        // move
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(horizontal, 0, vertical);

        //animator.SetFloat("Speed", speed);

        // if (speed == 5f)
        // {
        //     Debug.Log("movin");
        //     animator.SetFloat("Speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        // }
        
        // else if (speed == 0f)
        // {
        //     Debug.Log("not movin");
        //     animator.SetFloat("Speed", 0f);
        // }
        

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

        if (Time.time > nextFireTime)
        {
            if (Input.GetKey(KeyCode.Q) && !invisibile)
            {
                // cooldown
                nextFireTime = Time.time + cooldownTime;
                // ability in use again
                StartCoroutine(Ability(5f));
            }
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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("sea anemone"))
        {
            hidden = false;
            Debug.Log("hidden status = " + hidden);           
        }
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

    // public void PlaySound(AudioClip clip)
    // {
    //     audioSource.PlayOneShot(clip);
    // }
}
