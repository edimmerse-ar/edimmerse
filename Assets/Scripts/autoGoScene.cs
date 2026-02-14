using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class autoGoScene : MonoBehaviour
{
 public string sceneName;  

    void Start()
    {
        Invoke("LoadScene", 5f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }    
}
