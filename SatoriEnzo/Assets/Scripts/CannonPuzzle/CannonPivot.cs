using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPivot : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void RotateToTarget(Vector3 targetPosition)
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetDir = targetPosition - currentPosition;
        float angle = Vector3.Angle(targetDir, targetPosition);
        
        bool Reverse = currentPosition.x < targetPosition.x;
        if(Reverse) angle = -angle;

        transform.Rotate(0, 0, angle, Space.Self);
    }
}
