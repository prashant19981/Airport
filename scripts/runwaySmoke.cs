using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class runwaySmoke : MonoBehaviour
{
    private string scene;
    [SerializeField] ParticleSystem smoke_1;
    [SerializeField] ParticleSystem smoke_2;
    // Start is called before the first frame update
    private void Start()
    {
        smoke_1.Stop();
        smoke_2.Stop();
        scene = SceneManager.GetActiveScene().name;
    }
    void OnTriggerEnter2D(Collider2D other) {
       
        if(other.tag == "Airplane" || other.tag == "jet")
        {
           
            if ( scene == "3" || scene == "4")
            {
                if (other.GetComponent<NightControl1>().hasLanded)
                {
                    
                    smoke_1.Play();
                    smoke_2.Play();
                }
            }
            else
            {
                if (other.GetComponent<Control>().hasLanded)
                {
                    smoke_1.Play();
                    smoke_2.Play();
                }
            }
        }


    }
}
