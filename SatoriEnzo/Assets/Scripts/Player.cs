using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    public float horizontalSpeed;
    public float jumpSpeed;
    private int maxJump = 2; // Default Double Jump
    private int currentJump;
    private int bonusJump = 0;
    private float bonusHorizontalSpeed = 0;
    private float bonusJumpSpeed = 0;

    private float bonusShieldCount = 0; 

    [SerializeField] private string endingSceneName = "GameOver"; // Set this in Inspector

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ResetJump();
    }

    private void Update()
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * (horizontalSpeed + bonusHorizontalSpeed), body.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && CheckJump())
        {
            currentJump--;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetJump();
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(bonusShieldCount > 0) return;

            GoToEnding(); // Call function to change scene
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            ApplyItemBonus(collision.gameObject);
        }
    }

    private void ResetJump()
    {
        currentJump = maxJump + bonusJump;
    }

    private bool CheckJump()
    {
        return currentJump > 0;
    }

    private void GoToEnding()
    {
        SceneManager.LoadScene(endingSceneName);
    }

    private void ApplyItemBonus(GameObject Item)
    {
        Item itemScript = Item.GetComponent<Item>();

        if(itemScript == null)
        {
            Debug.LogError("Cannot find Item script");
        }

        bonusJump += itemScript.bonusJump;
        bonusHorizontalSpeed += itemScript.bonusHorizontalSpeed;
        bonusJumpSpeed += itemScript.bonusJumpSpeed;
        if(itemScript.bonusShield) bonusShieldCount += 1;

        StartCoroutine(RevertItemBonus(itemScript));
    }

    IEnumerator RevertItemBonus(Item itemScript)
    {
        yield return new WaitForSeconds(itemScript.duration);

        bonusJump -= itemScript.bonusJump;
        bonusHorizontalSpeed -= itemScript.bonusHorizontalSpeed;
        bonusJumpSpeed -= itemScript.bonusJumpSpeed;
        if(itemScript.bonusShield) bonusShieldCount -= 1;
    }
}
