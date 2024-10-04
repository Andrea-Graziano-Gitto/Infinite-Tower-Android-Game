using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggeraltezza : MonoBehaviour
{
    public GameObject camera;     // Riferimento alla camera
    private float timer = 0f;     // Timer per il tempo di permanenza nel trigger
    private float requiredTime = 4f;  // Tempo necessario nel trigger per attivare il movimento

    void OnTriggerStay(Collider other)
    {
        // Verifica se l'oggetto con cui è in contatto appartiene al layer "Cubo"
        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo"))
        {
            timer += Time.deltaTime;

            // Se è rimasto nel trigger per almeno 4 secondi
            if (timer >= requiredTime)
            {
                // Imposta la booleana movimento della camera su true
                camerameuscript cameraScript = camera.GetComponent<camerameuscript>();
                if (cameraScript != null)
                {
                    cameraScript.movimento = true;
                }

                // Distruggi l'oggetto
                Destroy(gameObject);
            }
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
