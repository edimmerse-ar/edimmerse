using UnityEngine;
using System.Collections;

public class ActivateObjects : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    public void StartLight()
    {
        StartCoroutine(ActivateAfterDelay(true)); // true = turn ON
    }

    public void StopLight()
    {
        StartCoroutine(ActivateAfterDelay(false)); // false = turn OFF
    }

    IEnumerator ActivateAfterDelay(bool activate)
    {
        yield return new WaitForSeconds(0.71f);

        if (object1 != null) object1.SetActive(activate);
        if (object2 != null) object2.SetActive(activate);
    }
}
