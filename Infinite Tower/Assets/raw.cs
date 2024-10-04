using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raw : MonoBehaviour
{
    public GameObject audio;
    public void OnEnable()
    {
        Instantiate(audio, transform);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
