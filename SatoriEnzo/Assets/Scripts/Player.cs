using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D playerCollider;
    private GameObject currentPlatform;
    public float horizontalSpeed;
    public float jumpSpeed;
    private int maxJump = 2; // Default Double Jump
    private int currentJump;
    private int bonusJump = 0;
    private float bonusHorizontalSpeed = 0;
    private float bonusJumpSpeed = 0;

    private float bonusShieldCount = 0; 
    
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private string endingSceneName = "GameOver"; // Set this in Inspector
    [SerializeField] private AudioClip jumpSound;   // Drag jump sound here
    [SerializeField] private AudioClip hitSound;    // Drag hit sound here
    [SerializeField] private AudioClip getItemSound;    // Drag hit sound here

    private AudioSource audioSource;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        ResetJump();
    }

    private void Update()
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * (horizontalSpeed + bonusHorizontalSpeed), body.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && CheckJump())
        {
            currentJump--;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            PlaySound(jumpSound);
        }

        if(
            (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
            && currentPlatform != null
        )
        {
            StartCoroutine(DisablePlatformCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.transform.position.y < gameObject.transform.position.y - gameObject.transform.lossyScale.y) ResetJump(); // Check Above
        }

        if(collision.gameObject.CompareTag("Platform"))
        {
            if (collision.gameObject.transform.position.y < gameObject.transform.position.y - gameObject.transform.lossyScale.y) ResetJump(); // Check Above
            currentPlatform = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            PlaySound(hitSound);
            if (bonusShieldCount > 0) return;

            Invoke(nameof(GoToEnding), 0.5f); // Delay scene transition
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            PlaySound(getItemSound);
            Destroy(collision.gameObject);
            ApplyItemBonus(collision.gameObject);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = null;
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
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // Play sound once
        }
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

    IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
