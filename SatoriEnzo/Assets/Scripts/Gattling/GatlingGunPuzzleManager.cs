using UnityEngine;

public class GatlingGunPuzzleManager : Puzzle
{
    public GameObject gatlingGunPrefab;
    public int requiredParts = 3;
    private int collectedParts = 0;
    private GatlingGun gatling;
    private GatlingGunPartSpawner spawner;


    private void Awake() {
        gatling = FindObjectOfType<GatlingGun>();
        spawner = FindObjectOfType<GatlingGunPartSpawner>();

    }
    public void CollectPart()
    {
        collectedParts++;
        if (collectedParts >= requiredParts)
        {
            gatling = FindObjectOfType<GatlingGun>();
            gatling.StartFiring();
            collectedParts = 0;
        }
    }

    public override void InitPuzzle()
    {

        spawner.StartPuzzle();

    }

    public override void EndPuzzle()
    {
        gatling.deActive();
        spawner.EndPuzzle();
        collectedParts = 0;

    }




}

