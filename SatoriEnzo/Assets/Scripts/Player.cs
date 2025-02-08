using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    public float horizontalSpeed;
    public float jumpSpeed;
    private int maxJump = 2; // Default Double Jump
    private int currentJump;
    private int bonusJump = 0; // From Item etc.

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        ResetJump();
    }

    private void Update() {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * horizontalSpeed, body.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && CheckJump()) {
            currentJump--;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            ResetJump();
        }
    }

    private void ResetJump(){
        currentJump = maxJump + bonusJump;
    }

    private bool CheckJump() {
        return currentJump > 0;
    }
}
