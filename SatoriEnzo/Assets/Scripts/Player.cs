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
    [SerializeField] private AudioClip jumpSound;   // Drag jump sound here
    [SerializeField] private AudioClip hitSound;    // Drag hit sound here
    [SerializeField] private AudioClip getItemSound;    // Drag hit sound here

    private AudioSource audioSource;

    private string inputBuffer = ""; // Buffer for player typing

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
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


        // Capture typing input
        foreach (char c in Input.inputString)
        {
            if (c == '\b' && inputBuffer.Length > 0)
            {
                inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1); // Handle backspace
            }
            else if (c == '\n' || c == '\r')
            {
                CheckBulletsForAnswer(inputBuffer); // Check answer on enter
                inputBuffer = "";
            }
            else
            {
                inputBuffer += c; // Add to input buffer
            }
        }
    }
    private void CheckBulletsForAnswer(string input)
    {
        BossBullet[] bullets = FindObjectsOfType<BossBullet>();

        foreach (BossBullet bullet in bullets)
        {
            if (bullet.CheckAnswer(input))
            {
                Destroy(bullet.gameObject); // Destroy bullet on correct answer
                break;
            }
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
}
