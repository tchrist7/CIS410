using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float jumpSpeed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private Collider collider;
    // private Transform transform;
    private int count;
    private float movementX;
    private float movementZ;
    // private bool isGrounded;
    private bool canDoubleJump;
    private float distToGround;
    

    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        // transform = GetComponent<Transform>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        distToGround = collider.bounds.extents.y;
    }

    public void Move(InputAction.CallbackContext context) 
    {
        Vector2 movementVector = context.ReadValue<Vector2>();
        movementX = movementVector.x;
        movementZ = movementVector.y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded()) 
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                canDoubleJump = true;
            } else {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                    canDoubleJump = false;
                }
            }
        }
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12) 
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate() 
    {
        // IsGrounded() = (0.4999f <= rb.position.y && rb.position.y <= 0.5001f);
        if (IsGrounded())
        {
            Vector3 movement = new Vector3(movementX, 0f, movementZ);
            rb.AddForce(movement * speed);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
