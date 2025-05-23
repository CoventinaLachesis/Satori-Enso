using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName; 

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) GoToNextScene();
    }

    private void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
