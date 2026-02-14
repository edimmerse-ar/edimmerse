using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStatus : MonoBehaviour
{
    public GameObject btn1,btn2;
    // Start is called before the first frame update
    void Start()
    {
        int a = PlayerPrefs.GetInt("status1");
        int b = PlayerPrefs.GetInt("status2");

            if (a == 1)
        {
            btn1.SetActive(true);
        }
            if (b == 1)
        {
            btn2.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
