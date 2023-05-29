using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject instantiatePoints;
    Transform temp;
    int randomIndex;
    void Start()
    {
        instantiatePoints = GameObject.FindGameObjectWithTag("instantiatepoint");
        StartCoroutine(cloudControl());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator cloudControl()
    {
        yield return new WaitForSeconds(2f);
        temp = transform.GetChild(0);
        if (!temp.gameObject.GetComponent<cloud>().isBeingUsed)
        {
            temp.gameObject.SetActive(true);
            temp.gameObject.GetComponent<cloud>().isBeingUsed = true;
            randomIndex = (int)UnityEngine.Random.Range(0f, 8f);
            temp.position = instantiatePoints.transform.GetChild(randomIndex).position;
            temp.rotation = instantiatePoints.transform.GetChild(randomIndex).rotation;

        }
        StartCoroutine(cloudControl());
    }
}
