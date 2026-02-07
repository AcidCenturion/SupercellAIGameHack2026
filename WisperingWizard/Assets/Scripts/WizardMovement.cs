using UnityEngine;
using UnityEngine.InputSystem;

public class WizardMovement : MonoBehaviour
{
    // FIELDS
    public float moveSpd;
    public float jumpPower;
    public LayerMask groundLayer;
    public float coyoteLeniency;


    private Rigidbody2D rb;
    private float moveInput;
    private bool jumpInput;
    private RaycastHit2D groundCheck;
    private float coyoteTime;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement();

        jump();
    }


    // FUNCTIONS
    private void movement()
    {
        rb.linearVelocityX = moveInput * moveSpd;
    }

    private void jump()
    {
Debug.DrawRay(transform.position, Vector2.down * 1.1f, Color.red);

        // coyote time
        //timer reset
        if(isGrounded())
        {
            coyoteTime = coyoteLeniency;
        }
        //falling after ledge
        else if (!isGrounded() && rb.linearVelocityY <= 0)
        {
            coyoteTime -= Time.deltaTime;
        }
        //no coyote time to a regular jump
        else
        {
            coyoteTime = -1;
        }

        // main jump functionality
        //any input during coyote time
        //no jumping during a dash
        if(jumpInput && coyoteTime >= 0)
        {
            rb.linearVelocityY = jumpPower;
        }

        // short hop
        if(!jumpInput && rb.linearVelocityY > 0)
        {
            rb.linearVelocityY = 0;
        }
    }


    // HELPER FUNCTIONS
    /*
    true when raycast detects the groundLayer
    set animator to not jumping when on ground
    */
    private bool isGrounded()
    {
        groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return groundCheck;
    }


    // ACTION INPUT SYSTEM FUNCTIONS
    /*
    Gets the direction of the input 
    as a positive or negative value (-1, 1)
    based on bindings connected to the Move action in the Input System asset
    */
    private void OnMove(InputValue input)
    {
        moveInput = input.Get<float>();
    }

    /*
    jumpInput turned true when button is pressed
    stays true until button is released
    turns false upon release
    */
    private void OnJump(InputValue input)
    {
        jumpInput = input.isPressed;
Debug.Log("checked jump: " + jumpInput);
    }
}
