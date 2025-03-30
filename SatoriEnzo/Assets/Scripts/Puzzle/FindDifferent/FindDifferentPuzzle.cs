using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindDifferentPuzzle : Puzzle
{
    [Header("Sprite Groups")]
    public Sprite[] groupA;
    public Sprite[] groupB;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Sprite Container")]
    public GameObject spriteContainerPrefab; // prefab with SpriteRenderer + Collider2D

    [Header("Sprite Display")]
    public Vector2 targetSize = new Vector2(1f, 1f); // Desired size in world units

    [Header("Sprite Delay")]
    public float popinDelay = 0.2f;
    public float colDelay = 0.3f;


    [Header("Timer")]
    public float timeLimit = 5f;
    public Image timerBarUI;

    [Header("VFX / SFX")]
    public ParticleSystem correctVFX;
    public ParticleSystem wrongVFX;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private float timer;
    private bool isRunning = false;
    private List<GameObject> spawnedObjects = new();

    public override void InitPuzzle()
    {
        isRunning = true;
        timer = timeLimit;

        Shuffle(spawnPoints);

        // Pick 3 random from group A
        List<Sprite> selectedA = new();
        for (int i = 0; i < 3; i++)
        {
            selectedA.Add(groupA[Random.Range(0, groupA.Length)]);
        }

        // Pick 1 random from group B
        Sprite different = groupB[Random.Range(0, groupB.Length)];
        int differentIndex = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            Sprite chosenSprite;
            string tag;

            if (i == differentIndex)
            {
                chosenSprite = different;
                tag = "Correct";
            }
            else
            {
                chosenSprite = selectedA[i < differentIndex ? i : i - 1];
                tag = "Wrong";
            }

            GameObject obj = SpawnSprite(chosenSprite, spawnPoints[i].position, tag);
            spawnedObjects.Add(obj);
        }

        if (timerBarUI != null)
        {
            timerBarUI.fillAmount = 1f;
            timerBarUI.enabled = true;
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;

        if (timerBarUI != null)
        {
            timerBarUI.fillAmount = Mathf.Clamp01(timer / timeLimit);
        }

        if (timer <= 0f)
        {
            OnTimeExpired();
        }
    }

    public override void EndPuzzle()
    {
        isRunning = false;

        foreach (var obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }

        spawnedObjects.Clear();

        if (timerBarUI != null)
        {
            timerBarUI.fillAmount = 0f;
            timerBarUI.enabled = false;
        }
    }

    public void OnCorrectTouched(Vector3 pos)
    {
        FXPlayer.PlayVFX(correctVFX, pos);
        FXPlayer.PlaySound(correctSound, pos);
        boss.TakeDamage(10);
        EndPuzzle();
    }

    public void OnWrongTouched(Vector3 pos)
    {
        FXPlayer.PlayVFX(wrongVFX, pos);
        FXPlayer.PlaySound(wrongSound, pos);
        StartCoroutine(DelayedDeath());


    }

    private void OnTimeExpired()
    {
        FXPlayer.PlayVFX(wrongVFX, player.transform.position);
        FXPlayer.PlaySound(wrongSound, player.transform.position);
        StartCoroutine(DelayedDeath());
        EndPuzzle();
    }

    private GameObject SpawnSprite(Sprite sprite, Vector3 position, string tag)
    {
        GameObject obj = Instantiate(spriteContainerPrefab, position, Quaternion.identity);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        obj.tag = tag;
        if (sr.sprite != null)
        {
            Vector2 spriteSize = sr.sprite.bounds.size;
            Vector3 scale = obj.transform.localScale;
            scale.x = targetSize.x / spriteSize.x;
            scale.y = targetSize.y / spriteSize.y;
            obj.transform.localScale = scale;
        }

        var trigger = obj.AddComponent<FindDifferentTrigger>();
        var popin = obj.AddComponent<PopIn>();
        var delay = obj.AddComponent<ColliderDelay>();
        popin.popTime = popinDelay;
        delay.delay = colDelay;
        trigger.manager = this;

        return obj;
    }

    private void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int j = Random.Range(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }    
    private System.Collections.IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(0.5f);
        player.Death();
    }
}
