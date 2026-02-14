using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSeenManager : MonoBehaviour
{
    public GameObject group1Obj1;
    public GameObject group1Obj2;

    public GameObject group2Obj1;
    public GameObject group2Obj2;

    public void ActivateGroup1()
    {
        StartCoroutine(EnableAndDisableAfterDelay(group1Obj1, group1Obj2));
    }

    public void ActivateGroup2()
    {
        StartCoroutine(EnableAndDisableAfterDelay(group2Obj1, group2Obj2));
    }

    private IEnumerator EnableAndDisableAfterDelay(GameObject obj1, GameObject obj2)
    {
        obj1.SetActive(true);
        obj2.SetActive(true);

        yield return new WaitForSeconds(5f);

        obj1.SetActive(false);
        obj2.SetActive(false);
    }
}
