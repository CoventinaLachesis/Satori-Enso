using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBullet : MonoBehaviour
{

    public float moveAngleDeg = 0f;
    private Vector2 moveDirection;
    public float moveSpeed = 1f;
    public float delay = 0f;
    private Vector3 spawnPoint;
    private bool isActive = false;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.CompareTag("Player")) {
            EndGame();
        }
    }

    void Start() 
    {
        moveDirection = GetUnitVector(moveAngleDeg);
        spawnPoint = transform.position;
    }

    void Update() 
    {
        if(isActive) 
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        
    }

    private Vector2 GetUnitVector(float moveAngleDeg) 
    {
        float angleRad;
        angleRad = moveAngleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void EndGame() 
    {
        // TODO end game
        Deactivate();
        Debug.Log("Bullet Hit");
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate() 
    {
        transform.position = spawnPoint;
        isActive = false;
    }
}
