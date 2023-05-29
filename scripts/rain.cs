using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rain : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem rainFall;
    void Start()
    {
        rainFall = gameObject.GetComponent<ParticleSystem>();
        rainFall.Play();
        
    }

    // Update is called once per frame
  
}
