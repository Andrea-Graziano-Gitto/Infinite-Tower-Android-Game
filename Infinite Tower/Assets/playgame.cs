using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playgame : MonoBehaviour
{
    public GameObject attivo;   
    public GameObject attivodue;
    public GameObject disattivo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void inizia()
    {
        attivo.SetActive(true);
        attivodue.SetActive(true);
        disattivo.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
