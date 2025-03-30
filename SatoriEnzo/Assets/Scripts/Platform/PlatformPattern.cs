using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformPattern : MonoBehaviour
{
    public float duration;
    public abstract void StartPattern();

    public abstract void EndPattern();
}
