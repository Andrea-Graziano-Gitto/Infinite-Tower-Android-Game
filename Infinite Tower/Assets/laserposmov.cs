using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserposmov : MonoBehaviour
{
    public GameObject cameraObject;  // Riferimento alla camera
    public bool go;
    public float speed = 30f;  // Velocità di movimento
    public float minY = -6f;   // Limite minimo del movimento sull'asse Y
    public float maxY = 7f;    // Limite massimo del movimento sull'asse Y
    public GameObject laser;
    private float timeElapsed = 0f;  // Variabile per tracciare il tempo interno
    public GameObject sound;
    void Awake()
    {
        go = true;
    }

    void OnEnable()  // Metodo chiamato quando l'oggetto viene attivato
    {
        go = true;  // Ripristina il movimento
        timeElapsed = Random.Range(0f, (maxY - minY) / speed);  // Assegna un valore casuale per far partire il movimento da una posizione casuale
        StartCoroutine(StopMovementAfterTime(4.3f));  // Avvia di nuovo la coroutine
    }

    void Update()
    {
        if (go)
        {
            timeElapsed += Time.deltaTime;  // Aggiorna il tempo interno

            // Movimento oscillante tra minY e maxY
            float pingPong = Mathf.PingPong(timeElapsed * speed, maxY - minY) + minY;  // Calcola il nuovo valore oscillante

            Vector3 newPosition = transform.position;
            newPosition.y = cameraObject.transform.position.y + pingPong;  // Aggiorna la posizione Y in base all'oscillazione

            transform.position = newPosition;  // Applica la nuova posizione
        }
    }

    // Coroutine per fermare il movimento dopo un certo intervallo di tempo
    IEnumerator StopMovementAfterTime(float time)
    {
        // Istanzia il laser nella posizione specificata
        GameObject las = Instantiate(laser, new Vector3(200f, gameObject.transform.position.y+1f, laser.transform.position.z), Quaternion.identity);

        // Attende il tempo specificato prima di fermare il movimento
        yield return new WaitForSeconds(time);

        // Disabilita il movimento dopo i 3 secondi
        go = false;

        // Imposta la posizione Y di 'las' uguale a quella di 'gameObject'
        Vector3 lasPosition = las.transform.position;  // Ottieni la posizione corrente di 'las'
        lasPosition.y = gameObject.transform.position.y ;  // Imposta la Y di 'las' uguale a quella di 'gameObject'
        las.transform.position = lasPosition;  // Applica la nuova posizione a 'las'

        // Attende 2 secondi dopo che il movimento si è fermato
        yield return new WaitForSeconds(1f);
        Instantiate(sound, new Vector3(1.7f, 3f, gameObject.transform.position.z), Quaternion.identity);
        // Imposta l'oggetto corrente (gameObject) come inattivo
        gameObject.SetActive(false);
    }

}
