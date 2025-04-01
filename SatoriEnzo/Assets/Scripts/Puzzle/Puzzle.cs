using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    protected Player player;
    protected BossPattern boss;

    protected virtual void Start()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (boss == null) boss = FindObjectOfType<BossPattern>();
    }

    public abstract void InitPuzzle();
    public abstract void EndPuzzle();
}
