using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach to Space Level Spinner
public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//rotates 50 degrees per second around y axis
        transform.Rotate (0,50*Time.deltaTime,0); 
    }
}
