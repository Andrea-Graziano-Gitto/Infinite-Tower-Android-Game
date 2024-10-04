using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroytime : MonoBehaviour
{
    public float dtime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, dtime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
