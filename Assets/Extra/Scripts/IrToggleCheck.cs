using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class IrToggleCheck : MonoBehaviour
{
    public ScoreGen ScoreScript;
    public Dropdown[] dd;
    public string[] valUe;
    public GameObject objectToToggle;
    public GameObject Error, finishLoose, finishWin,codeBtn;
    public Dropdown dropdown;
    private float delay = 1f;
    private Coroutine toggleCoroutine;
    public Transform pointA;
    public Transform pointB;
    public float maxDistance = 5f;
    public bool flag = false;

    void Start()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(DropdownValueChanged);
        }
        else
        {
            Debug.LogWarning("Dropdown reference is missing!");
        }
    }

    void Update()
    {
        Debug.Log(flag);
        if (flag)
        {
            if (CheckRaycastCondition())
            {
                if (toggleCoroutine == null)
                {
                    toggleCoroutine = StartCoroutine(ToggleObject());
                }
            }
            else
            {
                Debug.Log("Wrong!");
                if (toggleCoroutine != null)
                {
                    StopCoroutine(toggleCoroutine);
                    toggleCoroutine = null;
                    objectToToggle.SetActive(false);
                }
            }
        }
    }

    public void checkALLCode()
    {
        if (CheckConditions())
        {
            Debug.Log("Wwwww!");
            flag = true;
            finishWin.SetActive(true);
            ScoreScript.TotalScore+=9;
            codeBtn.SetActive(false);

        }
        else
        {
            ScoreScript.TotalError+=1;
            Error.SetActive(true);
            codeBtn.SetActive(true);
        }
    }

    private bool CheckRaycastCondition()
    {
Vector3 direction = new Vector3(0.93f, 0.06f, -0.35f);
        float distance = Vector3.Distance(pointA.position, pointB.position);
        Debug.Log(direction);
        
        // Debug.DrawRay(pointA.position, direction * maxDistance, Color.red);

        if (distance < maxDistance)
        {
            if (Physics.Raycast(pointA.position, direction, out RaycastHit hit, maxDistance))
            {
                Debug.Log("All OK!");
                Debug.DrawRay(pointA.position, direction * hit.distance, Color.green);
                return hit.collider.transform == pointB;
            }
        }
        Debug.Log("Not OK!");
        return false;
    }

    private bool CheckConditions()
    {
        if (dd.Length < 14)
        {
            Debug.LogWarning("Dropdown array does not contain enough elements.");
            return false;
        }

        return dd[0].options[dd[0].value].text == valUe[0] &&
               dd[1].options[dd[1].value].text == valUe[1] &&
               dd[2].options[dd[2].value].text == valUe[2] &&
               dd[3].options[dd[3].value].text == valUe[3] &&
               dd[4].options[dd[4].value].text == valUe[4] &&
               dd[5].options[dd[5].value].text == valUe[5] &&
               dd[6].options[dd[6].value].text == valUe[6] &&
               dd[7].options[dd[7].value].text == valUe[7] &&
               dd[8].options[dd[8].value].text == valUe[8] &&
            //    dd[9].options[dd[9].value].text == valUe[9] &&
               dd[10].options[dd[10].value].text == valUe[10] &&
               dd[11].options[dd[11].value].text == valUe[11] &&
               dd[12].options[dd[12].value].text == valUe[12];
            //    dd[13].options[dd[13].value].text == valUe[13]
    }

    private void DropdownValueChanged(int value)
    {
        delay = value switch
        {
            0 => 1f,
            1 => 1f,
            2 => 2f,
            _ => 1f
        };
    }

    public IEnumerator ToggleObject()
    {
        while (true)
        {
            objectToToggle.SetActive(true);
            yield return new WaitForSeconds(0);
        }
    }
}
