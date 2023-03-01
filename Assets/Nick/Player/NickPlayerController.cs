using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class NickPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject camHolder;
    private Vector2 move, look;
    public float speed, sens, maxForce, jumpForce, crouchSpeed, fallSpeed;
    private float curSpeed, curJumpForce;
    private float lookRotation;
    private bool grounded, jumpHeld, crouchHeldInAir;

    public bool Grounded
    {
        get { return grounded; }
        set { grounded = value; }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Jump();

        if (context.performed)
            jumpHeld = true;
        if (context.canceled)
            jumpHeld = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // Crouch();
        if (grounded && context.performed)
        {
            curSpeed = crouchSpeed;
        }
        else if (!grounded && context.performed)
        {
            curJumpForce = -fallSpeed;
            crouchHeldInAir = true;
        }
        else if (context.canceled)
        {
            curSpeed = speed;
            curJumpForce = jumpForce;
            crouchHeldInAir = false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        curSpeed = speed;
        curJumpForce = jumpForce;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();

        if (jumpHeld || crouchHeldInAir)
        {
            rb.MovePosition(transform.position + Time.deltaTime * Vector3.up * curJumpForce);
        }
    }

    void LateUpdate()
    {
        Look();
    }

    private void Move()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity = targetVelocity.normalized;
        targetVelocity *= curSpeed;

        // rb.MovePosition(transform.position + Time.deltaTime * targetVelocity * curSpeed);

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * look.x * sens);

        lookRotation += -(look.y) * sens;
        lookRotation = Mathf.Clamp(lookRotation, -90, 70);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);

    }

    private void Jump()
    {
        Vector3 jF = Vector3.zero;

        if (grounded)
        {
            jF = Vector3.up * jumpForce;
        }

        rb.AddForce(jF, ForceMode.VelocityChange);
    }

    private void Crouch()
    {

    }
}
