using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
    public float speed = 0;
    public float jumpForce = 20f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private bool isGrounded;
    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
    }

    public void Move(InputAction.CallbackContext context) {
        Vector2 movementVector = context.ReadValue<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            if (isGrounded) {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                canDoubleJump = true;
            } else {
                if (canDoubleJump) {
                canDoubleJump = false;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                }
            }
        }
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();
        if (count >= 12) {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate() {
        isGrounded = (rb.position.y == 0.5f);

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }
}
