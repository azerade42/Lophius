using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public NickPlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        SetGrounded(other, true);
    }

    private void OnTriggerStay(Collider other)
    {
        SetGrounded(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        SetGrounded(other, false);
    }

    private void SetGrounded(Collider other, bool state)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            player.Grounded = state;
        }
    }
}
