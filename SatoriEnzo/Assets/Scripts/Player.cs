using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D playerCollider;
    private Animator anim;
    private GameObject currentPlatform;
    public float horizontalSpeed;
    public float jumpSpeed;
    private int maxJump = 2; // Default Double Jump
    private int currentJump;
    private int bonusJump = 0;
    private float bonusHorizontalSpeed = 0;
    private float bonusJumpSpeed = 0;
    private float bonusScale = 0;
    private int platformDissolveEffectCounter = 0;
    private int movementReverseCounter = 0;

    private float bonusShieldCount = 0; 
    private Vector3 initScale;
    private Vector3 playerScale;

    [SerializeField] private GameObject shieldObject;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] bool immortal=false;    // Drag hit sound here
    [SerializeField] private string endingSceneName = "GameOver"; // Set this in Inspector
    [SerializeField] private AudioClip jumpSound;   // Drag jump sound here
    [SerializeField] private AudioClip hitSound;    // Drag hit sound here
    [SerializeField] private AudioClip getItemSound;    // Drag hit sound here

    private AudioSource audioSource;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        shieldObject = transform.GetChild(0).gameObject;
        shieldObject.SetActive(false);

        initScale = transform.localScale;
        playerScale = initScale;
        ResetJump();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * (horizontalSpeed + bonusHorizontalSpeed), body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);

        if (Input.GetKeyDown(KeyCode.Space) && CheckJump())
        {
            currentJump--;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed + bonusJumpSpeed);
            PlaySound(jumpSound);
            anim.SetBool("OnGround", false);
        }

        if(
            (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
            && currentPlatform != null
        )
        {
            StartCoroutine(DisablePlatformCollision());
            anim.SetBool("OnGround", false);
        }

        anim.SetBool("IsRunning", horizontalInput != 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.transform.position.y < gameObject.transform.position.y - gameObject.transform.lossyScale.y) ResetJump(); // Check Above
            anim.SetBool("OnGround", true);
        }

        if(collision.gameObject.CompareTag("Platform"))
        {
            if (collision.gameObject.transform.position.y < gameObject.transform.position.y - gameObject.transform.lossyScale.y) ResetJump(); // Check Above
            currentPlatform = collision.gameObject;
            anim.SetBool("OnGround", true);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {

            PlaySound(hitSound);
            if (bonusShieldCount > 0)
            {
                Destroy(collision.gameObject);
                return;
            }
                

            if (immortal == false)
            Invoke(nameof(GoToEnding), 0.5f); // Delay scene transition
        }

        if (collision.gameObject.CompareTag("StageBullet"))
        {

            PlaySound(hitSound);
            if (bonusShieldCount > 0)
            {
                StageBullet stageBulletScript = collision.gameObject.GetComponent<StageBullet>();
                stageBulletScript.Deactivate();
                return;
            }
                

            if (immortal == false)
            Invoke(nameof(GoToEnding), 0.5f); // Delay scene transition
        }

        if (collision.gameObject.CompareTag("KillZone"))
        {
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
    
    private bool CheckMovementReversed()
    {
        return movementReverseCounter > 0;
    }

    private void SpawnTempPlatform(float duration, Vector2 offset)
    {
        // Spawn Platform
        Debug.Log("Spawn Platform with Duration of " + duration + " at " + offset + " of player position.");
    }

    private void Resize()
    {
        playerScale = new Vector3(initScale.x + bonusScale, initScale.y + bonusScale, playerScale.z);
        transform.localScale = playerScale;
    }

    private void ApplyItemBonus(GameObject Item)
    {
        Item itemScript = Item.GetComponent<Item>();

        if(itemScript == null)
        {
            Debug.LogError("Cannot find Item script");
        }

        bonusJump += itemScript.bonusJump;
        currentJump += itemScript.bonusJump;
        bonusHorizontalSpeed += itemScript.bonusHorizontalSpeed;
        bonusJumpSpeed += itemScript.bonusJumpSpeed;

        if(itemScript.bonusScale != 0)
        {
            bonusScale += itemScript.bonusScale;
            Resize();
        }
        
        if(itemScript.bonusShield) 
        {
            bonusShieldCount += 1;
            shieldObject.SetActive(true);
        }

        if(itemScript.reverseMovement)
        {
            movementReverseCounter += 1;
        }

        if(itemScript.spawnPlatform)
        {
            SpawnTempPlatform(itemScript.platformDuration, itemScript.spawnPlatformOffset);
        }

        if(itemScript.platformDissolveEffect)
        {
            platformDissolveEffectCounter += 1;
        }
            

        StartCoroutine(RevertItemBonus(itemScript));
    }

    IEnumerator RevertItemBonus(Item itemScript)
    {
        yield return new WaitForSeconds(itemScript.duration);

        bonusJump -= itemScript.bonusJump;
        currentJump -= itemScript.bonusJump;
        bonusHorizontalSpeed -= itemScript.bonusHorizontalSpeed;
        bonusJumpSpeed -= itemScript.bonusJumpSpeed;
        if(itemScript.bonusScale != 0)
        {
            bonusScale -= itemScript.bonusScale;
            Resize();
        }
        if(itemScript.bonusShield) bonusShieldCount -= 1;
        if(bonusShieldCount == 0) shieldObject.SetActive(false);
        if(itemScript.reverseMovement) movementReverseCounter -= 1;
        if(itemScript.platformDissolveEffect) platformDissolveEffectCounter -=1;
    }

    IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    
}
