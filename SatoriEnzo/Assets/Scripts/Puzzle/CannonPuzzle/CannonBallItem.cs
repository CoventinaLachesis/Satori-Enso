using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallItem : MonoBehaviour 
{
    private bool isHit = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isHit = true;
        }
    }

    public bool GetIsHit()
    {
        return isHit;
    }
}