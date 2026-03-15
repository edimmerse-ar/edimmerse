using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Include this namespace for scene management

public class SCChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Optional: Initialize anything here
    }

    // Update is called once per frame
    void Update()
    {
        // Optional: Handle updates here
    }

    // Method to change the scene
    public void chS(string args)
    {
        SceneManager.LoadScene(args);
    }
        public void exit()
    {
        Application.Quit();
    }
}
