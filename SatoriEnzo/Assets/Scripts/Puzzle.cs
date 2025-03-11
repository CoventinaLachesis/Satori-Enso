using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public GameObject player;
    public GameObject boss;

    public abstract void InitPuzzle();
    public abstract void EndPuzzle();
}
