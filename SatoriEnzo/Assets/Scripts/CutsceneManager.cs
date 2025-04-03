using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName; 
    [SerializeField] private float delayTime; 
    void Start()
    {
        StartCoroutine(GoToNextScene());
    }

    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(nextSceneName);
    }
}
