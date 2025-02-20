using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float moveAngleDeg = 0;
    private Vector2 moveDirection;
    public float moveSpeed = 1;
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
        Debug.Log(moveDirection);
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
        Debug.Log("Bullet Hit");
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate() 
    {
        isActive = false;
    }
}
