using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript: MonoBehaviour
{

    public InputField InputBox;
    private static Stack<string> sceneHistory = new Stack<string>();

    public void GoToScene(string sceneName)
    {
        if (sceneName == "botMain")
        {
            GlobalVariables.updateScore(5);
        }

		sceneHistory.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);
    }

    public void GoBack()
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop();
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.Log("No previous scene in history.");
        }
    }
}
