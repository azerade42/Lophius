using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invisibility : MonoBehaviour
{
    // ability to be active for 5 seconds
    public float isInvisible = 5f;
    public static bool invisibile = false;

    // cooldown
    public float cooldownTime = 20;
    private float nextFireTime = 0;

    void Update()
    {
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

    // ability to be active for 5 seconds
    IEnumerator Ability(float isInvisible)
    {
        Debug.Log("Invisibility started");
        invisibile = true;
        yield return new WaitForSeconds(isInvisible);
        invisibile = false;
        Debug.Log("Invisibility ended");
    }
}
