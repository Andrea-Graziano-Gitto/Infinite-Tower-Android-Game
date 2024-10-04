using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerameuscript : MonoBehaviour
{
    public bool movimento = false;  // Booleana per controllare il movimento
    public float velocita = 0.2f;     // Velocità di movimento sull'asse Y
    public GameObject laser;
    public GameObject lasernotice;
    private bool canInstantiate = true;  // Flag per controllare se è possibile instanziare il laser

    void Start()
    {
        // Avvia la coroutine per instanziare il laser
        StartCoroutine(InstantiateLaser());
    }

    void Update()
    {
        if (movimento)
        {
            if (velocita <= 0.45f) velocita += 0.0013f* Time.deltaTime;
            // Movimento verso l'alto sull'asse Y in modo fluido
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.Lerp(newPosition.y, newPosition.y + velocita, Time.deltaTime);
            transform.position = newPosition;
        }
    }

    // Coroutine per instanziare il laser ad intervalli casuali tra 16 e 20 secondi
    IEnumerator InstantiateLaser()
    {
        while (true)
        {
            // Genera un intervallo casuale tra 16 e 20 secondi
            float randomInterval = Random.Range(45f, 55f);
            yield return new WaitForSeconds(randomInterval);

            // Instanzia il laser solo se il movimento è attivo
            if (movimento)
            {
                lasernotice.SetActive(true);
               
            }
        }
    }

    public void Stairdestroy()
    {
        // Logica per il metodo Stairdestroy
    }
}
