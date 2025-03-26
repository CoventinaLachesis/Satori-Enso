using UnityEngine;

public class GatlingGunPuzzleManager : MonoBehaviour
{
    public GameObject gatlingGunPrefab;
    public Transform player;
    public int requiredParts = 3;
    private int collectedParts = 0;

    private GatlingGun activeGun;

    public void CollectPart()
    {
        collectedParts++;
        if (collectedParts >= requiredParts)
        {
            GatlingGun manager = FindObjectOfType<GatlingGun>();
            manager.StartFiring();
            collectedParts = 0;
        }
    }

    private void ActivateGatlingGun()
    {
        GameObject gunObj = Instantiate(gatlingGunPrefab);
        activeGun = gunObj.GetComponent<GatlingGun>();
        activeGun.player = player;
        activeGun.StartFiring(); // optional method to control start
    }

    public void AddAmmo(int amount)
    {
        if (activeGun != null)
            activeGun.AddAmmo(amount);
    }
}

