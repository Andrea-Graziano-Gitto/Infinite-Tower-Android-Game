using System.Collections;
using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public float speed; // Cambia la velocità da qui
    private GameObject centro; // Riferimento al GameObject "Centro"
    private TrailRenderer trailRenderer;

    void Awake()
    {
        // Trova il GameObject chiamato "Centro"
        centro = GameObject.Find("Centro");
        if (centro == null)
        {
            Debug.LogError("GameObject 'Centro' non trovato!");
            return; // Esci se non trovi il GameObject
        }
        float randomX;
        // Posiziona la stella in una posizione casuale
        if (Random.value > 0.5f)
        {
            randomX = Random.Range(-5f, -2f); // Prima parte del range
        }
        else
        {
            randomX = Random.Range(7f, 10f); // Seconda parte del range
        }
        float randomY;
        // Posiziona la stella in una posizione casuale
        if (Random.value > 0.5f)
        {
            randomY = Random.Range(10f, 16f); // Prima parte del range
        }
        else
        {
            randomY = Random.Range(20f, 25f); // Seconda parte del range
        }
        
        transform.position = new Vector3(randomX, randomY+centro.transform.position.y-18f, 13f);

        // Ruota la stella verso il centro
        transform.LookAt(centro.transform.position);

        // Ottieni l'angolo attuale e crea un nuovo vettore di angolazione
        Vector3 currentRotation = transform.eulerAngles;
        float angleX = currentRotation.x;
        float angleY = currentRotation.y;
        float angleZ = currentRotation.x; // Ottieni l'angolo Z
        if (transform.position.x <1.7) transform.eulerAngles = new Vector3(0f, 0f, -angleZ); // Imposta il nuovo angolo
        if (transform.position.x > 3) transform.eulerAngles = new Vector3(0f, 180f, -angleZ); // Imposta il nuovo angolo
        // Inizializza il TrailRenderer
        trailRenderer = GetComponent<TrailRenderer>();

        // Avvia la coroutine per disattivare il TrailRenderer e distruggere l'oggetto
        StartCoroutine(HandleTrailRenderer());
    }

    void Update()
    {
        // Muovi la stella verso avanti lungo l'asse X
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private IEnumerator HandleTrailRenderer()
    {
        // Aspetta 8 secondi
        yield return new WaitForSeconds(8f);

        // Disattiva il TrailRenderer
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        // Aspetta 1 secondo
        yield return new WaitForSeconds(1f);

        // Distruggi l'oggetto
        Destroy(gameObject);
    }
}
