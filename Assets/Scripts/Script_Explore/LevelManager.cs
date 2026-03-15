using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;

    void Start()
    {
        // Fetch unlock value from PlayerPrefs
        int unlock = PlayerPrefs.GetInt("unlock", 1); // Default to 1 if not set

        // Clamp the value to not exceed array bounds
        unlock = Mathf.Clamp(unlock, 0, levels.Length);

        // Activate the first 'unlock' levels, deactivate the rest
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i < unlock);
        }

        SceneHandler.lastSceneName = "ExploreMenu";
	}
}
