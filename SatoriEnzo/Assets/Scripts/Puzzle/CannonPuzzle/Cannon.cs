using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour 
{
    private bool isHit = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isHit = false;
        }
    }

    public bool GetIsHit()
    {
        return isHit;
    }

    public void RotateToTarget(Vector3 targetPosition)
    {
        GameObject cannonPivotObject = this.transform.GetChild(0).gameObject;
        if(cannonPivotObject == null) Debug.LogError("Cannon find Cannon Pivot Game Object (First Child Object of Cannon Object)");

        CannonPivot cannonPivotScript = cannonPivotObject.GetComponent<CannonPivot>();
        if(cannonPivotScript == null) Debug.LogError("Cannon find Cannon Pivot Script");

        cannonPivotScript.RotateToTarget(targetPosition);
    }
}