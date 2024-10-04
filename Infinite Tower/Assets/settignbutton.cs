using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settignbutton : MonoBehaviour
{
    public GameObject attivo;
    public GameObject attivohome;
    public GameObject attivogame;
    public GameObject disattivo;
    public GameObject disattivogame;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void iniziagame()
    {
        attivo.SetActive(true);
        attivogame.SetActive(true);
        disattivogame.SetActive(false);
        attivohome.SetActive(false);
      
    }
    public void iniziahome()
    {
        attivo.SetActive(true);
        attivogame.SetActive(false);
        disattivo.SetActive(false);
        attivohome.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
