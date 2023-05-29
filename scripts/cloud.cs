using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isBeingUsed = false;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, Time.deltaTime*0.2f, 0));
    }
    /*public bool getUsedValue()
    {
        return this.isBeingUsed;
    }
    public void setUsedValue(bool value)
    {
        this.isBeingUsed = value;
    }*/
    private void OnBecameInvisible()
    {
        isBeingUsed = false;
    }
}
