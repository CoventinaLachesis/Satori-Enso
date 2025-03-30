using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpawner : MonoBehaviour
{
    public GameObject[] puzzles;
    public float spawnInterval;
    public float puzzleDuration;
    private Puzzle currentPuzzle;

    void Start()
    {
        if (puzzles.Length == 0) return;
        
        InvokeRepeating("SpawnPuzzle", spawnInterval, spawnInterval + puzzleDuration);
        InvokeRepeating("RemovePuzzle", spawnInterval + puzzleDuration, spawnInterval + puzzleDuration);
    }

    void SpawnPuzzle()
    {
        currentPuzzle = puzzles[Random.Range(0, puzzles.Length)].GetComponent<Puzzle>();
        currentPuzzle.InitPuzzle();
    }

    void RemovePuzzle()
    {
        currentPuzzle.EndPuzzle();
    }

    void Update()
    {
        
    }
}
