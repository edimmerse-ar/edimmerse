using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Sprite[] Characters;
    public Image uiImage;
    private void Start()
    {
        if (GlobalVariables.character == 0) GlobalVariables.character = 0;
		uiImage.sprite = Characters[GlobalVariables.character-1];
    }

}
