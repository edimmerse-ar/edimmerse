using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;

    void Start()
    {
		int unlock = PlayerPrefs.GetInt("unlock", 1);

        if (levels == null || levels.Length == 0)
        {
            SceneHandler.lastSceneName = "ExploreMenu";
            return;
        }

        unlock = Mathf.Clamp(unlock, 0, levels.Length);

        int levelIndex = unlock - 1;

        int i = 0;
		foreach (var item in levels)
		{
			Button itemBtn = item.GetComponent<Button>();
            itemBtn.interactable = false;
            if(i <= levelIndex)
            {
                itemBtn.interactable = true;
			}
			i++;
		}

		GameObject levelGameObject = levels[levelIndex];

        if (levelGameObject != null)
        {
            var image = levelGameObject.GetComponent<Image>();
            if (image != null)
                image.color = new Color(1f, 0f, 0f);
        }
		
		SceneHandler.lastSceneName = "ExploreMenu";
	}
}
