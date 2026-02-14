using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class SceneChange : MonoBehaviour
{
    [SerializeField] private int sceneIndex; // Serialized field for easy editing in the Inspector

    private void Start()
    {
        StartCoroutine(ChangeSceneAfterDelay(3f)); // Start coroutine with delay
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene(sceneIndex);  // Load the scene
    }
}
