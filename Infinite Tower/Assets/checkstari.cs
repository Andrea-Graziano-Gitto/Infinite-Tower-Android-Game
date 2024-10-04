using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkstari : MonoBehaviour
{
    public GameObject stair;     // Riferimento alla camera
  public float timer = 0f;     // Timer per il tempo di permanenza nel trigger
    private float requiredTime = 0.1f;  // Tempo necessario nel trigger per attivare il movimento

    void OnTriggerStay(Collider other)
    {
        // Verifica se l'oggetto con cui è in contatto appartiene al layer "Cubo"
        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo") && other.gameObject.GetComponent<cubeMov>().clonato)
        {
           
                stair.SetActive(true);
             
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Resetta il timer quando esce dal trigger
        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo"))
        {
            timer = 0f;
        }
    }
}

