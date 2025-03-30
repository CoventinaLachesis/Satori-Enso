using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GatlingGun gatling = FindObjectOfType<GatlingGun>();
            if (gatling != null)
            {
                gatling.AddAmmo(ammoAmount);
            }

            Destroy(gameObject);
        }
    }
}