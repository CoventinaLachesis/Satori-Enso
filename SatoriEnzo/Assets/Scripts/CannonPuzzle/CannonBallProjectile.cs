using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallProjectile : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {

            BossPattern bossScript = collision.gameObject.GetComponent<BossPattern>();
            bossScript.TakeDamage(5);
            Destroy(gameObject);
        }
    }

    public void StartMoving(Vector3 targetPosition)
    {
        body = GetComponent<Rigidbody2D>();
        Vector3 direction = (targetPosition - transform.position).normalized;

        body.velocity = direction * speed;
    }

    private void Update()
    {

    }
}
