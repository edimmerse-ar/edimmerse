using UnityEngine;
using UnityEngine.UI;

public class ChangeValue : MonoBehaviour
{
    public int character;

    public void charChange()
    {
        GlobalVariables.character = character;
		PlayerPrefs.SetInt("character", character);
		PlayerPrefs.Save();
		Debug.Log("Updated Value: " + GlobalVariables.character);
    }
}
