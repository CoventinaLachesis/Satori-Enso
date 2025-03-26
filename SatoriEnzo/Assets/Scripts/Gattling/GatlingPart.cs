using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingPart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GatlingGunPuzzleManager manager = FindObjectOfType<GatlingGunPuzzleManager>();
            if (manager != null)
            {
                manager.CollectPart();
            }

            Destroy(gameObject);
        }
    }
}