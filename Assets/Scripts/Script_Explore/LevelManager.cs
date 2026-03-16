using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;

    void Start()
    {
        // Fetch unlock value from PlayerPrefs
        int unlock = PlayerPrefs.GetInt("unlock", 1);

        // If no levels configured, nothing to do
        if (levels == null || levels.Length == 0)
        {
            SceneHandler.lastSceneName = "ExploreMenu";
            return;
        }

        // Clamp the value to valid array indices (0 .. levels.Length-1)
        unlock = Mathf.Clamp(unlock, 0, levels.Length - 1);

        GameObject levelGameObject = levels[unlock-1];

        if (levelGameObject != null)
        {
            var image = levelGameObject.GetComponent<Image>();
            if (image != null)
                image.color = new Color(1f, 0f, 0f);
        }
		// Activate the first 'unlock' levels, deactivate the rest
		//for (int i = 0; i < levels.Length; i++)
		//{

		//levels[i].SetActive(i < unlock);
		//}

		SceneHandler.lastSceneName = "ExploreMenu";
	}
}
