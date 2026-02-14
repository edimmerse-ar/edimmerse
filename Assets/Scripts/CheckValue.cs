using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckValue : MonoBehaviour
{
    public InputField Namer;
    public InputField Ager;

    public GameObject NextModer;

    public void CheckVal()
    {
        if (Namer.text != "" && Ager.text != "")
        {
            NextModer.SetActive(true);
        }
    }
}
