using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public Player player;
    public BossPattern boss;

    public abstract void InitPuzzle();
    public abstract void EndPuzzle();
}
