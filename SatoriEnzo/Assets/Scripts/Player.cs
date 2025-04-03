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
    private Vector3 lastPlatformPosition;

    public float horizontalSpeed;
    public float jumpSpeed;
    public float gravityDiveSpeed;
    public float canDiveInterval;


    private bool canDive = true;
    private int maxJump = 2; // Default Double Jump
    private int currentJump;
    private int bonusJump = 0;
    private float bonusHorizontalSpeed = 0;
    private float bonusJumpSpeed = 0;
    private float bonusScale = 0;
    private int platformDissolveEffectCounter = 0;
    private int movementReverseCounter = 0;
    private float platformDisableDuration = 7;
    private bool isDiving = false;
    public GameObject diveTrailPrefab;

    private float bonusShieldCount = 0; 
    private Vector3 initScale;
    private Vector3 playerScale;

    [SerializeField] private GameObject shieldObject;
    [SerializeField] private GameObject platformDissolverObject;
    [SerializeField] private GameObject tempPlatform;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] bool immortal=false;    // Drag hit sound here
    [SerializeField] private string endingSceneName = "GameOver"; // Set this in Inspector
    [SerializeField] private AudioClip jumpSound;   // Drag jump sound here
    [SerializeField] private AudioClip hitSound;    // Drag hit sound here
    [SerializeField] private AudioClip getItemSound;    // Drag hit sound here


    [Header("Air Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool canDash = true;
    private bool isDashing = false;
    [Header("Double Tap Dash")]
    [SerializeField] private float doubleTapThreshold = 0.25f;
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;
    [Header("Dash VFX")]
    public GameObject dashTrailPrefab;

    private AudioSource audioSource;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        shieldObject.SetActive(false);
        platformDissolverObject.SetActive(false);

        initScale = transform.localScale;
        playerScale = initScale;
        ResetJump();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal") * (CheckMovementReversed() ? -1 : 1);
        body.velocity = new Vector2(horizontalInput * (horizontalSpeed + bonusHorizontalSpeed), body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && CheckJump())
        {
            isDiving = false;
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
            canDive = false;
            StartCoroutine(SetCanDive());
            anim.SetBool("OnGround", false);
        }
        
        if(
            ( Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ) &&
            ( !IsGrounded() && !isDashing && !isDiving && canDive )
        )
        {
            isDiving = true;
            canDive = false;
            StartCoroutine(SetCanDive());
        }

        if(isDiving)
        {
            body.velocity = new Vector2(body.velocity.x, -gravityDiveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash && !IsGrounded())
        {
            StartCoroutine(DoAirDash());
        }
        // Double-tap dash
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - lastLeftTapTime <= doubleTapThreshold && canDash && !isDashing && !IsGrounded())
            {
                StartCoroutine(DoAirDash(-1));
            }
            lastLeftTapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastRightTapTime <= doubleTapThreshold && canDash && !isDashing && !IsGrounded())
            {
                StartCoroutine(DoAirDash(1));
            }
            lastRightTapTime = Time.time;
        }
        if (currentPlatform != null)
        {
            Vector3 deltaPosition = currentPlatform.transform.position - lastPlatformPosition;
            transform.position += deltaPosition;

            lastPlatformPosition = currentPlatform.transform.position;
        }

        anim.SetBool("IsRunning", horizontalInput != 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (IsFromBelow(collision))
            {
                ResetJump();
                anim.SetBool("OnGround", true);
            }
                
        }

        if(collision.gameObject.CompareTag("Platform"))
        {
            if (IsFromBelow(collision))
            {
                if(platformDissolveEffectCounter > 0)
                {
                    DissolvePlatform(collision.gameObject, platformDisableDuration);
                    return;
                }

                currentPlatform = collision.gameObject;
                lastPlatformPosition = currentPlatform.transform.position;
                anim.SetBool("OnGround", true);
                
                ResetJump(); 
            } 
            
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
                Death();
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
                Death();
        }

        if (collision.gameObject.CompareTag("KillZone"))
        {
            Death();
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
    private bool IsFromBelow(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
                return true; // We're landing from above
        }
        return false;
    }
    
    private bool IsGrounded()
    {
        return anim.GetBool("OnGround");
    }

    private void ResetJump()
    {
        currentJump = maxJump + bonusJump;
        canDash = true; //  reset dash on landing
        if(isDiving)
        {
            isDiving = false;
            if (diveTrailPrefab)
                Instantiate(diveTrailPrefab, transform.position, Quaternion.identity);

        }
    }

    private bool CheckJump()
    {
        return currentJump > 0;
    }

    private IEnumerator DoAirDash(int dir = 0)
    {
        isDashing = true;
        canDash = false;

        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;

        float elapsed = 0f;
        float dashDir = dir != 0 ? dir : Mathf.Sign(transform.localScale.x);

        // Optional: trail effect
        if (dashTrailPrefab)
            Instantiate(dashTrailPrefab, transform.position, Quaternion.identity);

        while (elapsed < dashDuration)
        {
            body.velocity = new Vector2(dashDir * dashSpeed, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        body.gravityScale = originalGravity;
        isDashing = false;
    }

    IEnumerator SetCanDive()
    {
        yield return new WaitForSeconds(canDiveInterval);

        canDive = true;
    }
    public void Death()
    {
        // Freeze time
        Time.timeScale = 0.05f;

        // Zoom camera
        Camera.main.GetComponent<CameraZoomOnDeath>().FocusOn(transform);
        Invoke(nameof(GoToEnding), 0.2f); // Delay scene transition

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

        GameObject tempPlatformObject = Instantiate(
            tempPlatform, 
            new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z),
            Quaternion.identity
        );

        Platform platformScript = tempPlatformObject.GetComponent<Platform>();
        platformScript.WaitAndDestroy(duration);
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
            platformDisableDuration = itemScript.platformDisableDuration;
            platformDissolverObject.SetActive(true);
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
        if(platformDissolveEffectCounter == 0) platformDissolverObject.SetActive(false);
    }

    private void DissolvePlatform(GameObject platform, float duration)
    {
        Platform platformScript = platform.GetComponent<Platform>();

        if(platformScript == null) 
        {
            Debug.LogError("Cannot Find Platform Script");
            return;
        }

        platformScript.TempDisablePlatform(duration);
    }

    IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }


    
}
