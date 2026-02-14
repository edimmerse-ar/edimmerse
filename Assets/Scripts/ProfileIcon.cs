using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileIcon : MonoBehaviour
{
    public Sprite[] Profile;
    public Image uiImage;
    private void Start()
    {
        uiImage.sprite = Profile[GlobalVariables.character];
    }
}
