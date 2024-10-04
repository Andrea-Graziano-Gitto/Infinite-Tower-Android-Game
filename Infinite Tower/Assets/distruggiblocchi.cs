using System.Collections;
using UnityEngine;

public class distruggiblocchi : MonoBehaviour
{
    public GameObject camera; // Riferimento alla tua camera (se necessario per altre operazioni)
    public GameObject textPrefab; // Prefab del testo da istanziare
    public Transform canvasTransform; // Trasform del canvas in world space
    public float offsetX = 0f; // Offset orizzontale per posizionare il prefab
    public float offsetY = 100f; // Offset verticale per posizionare il prefab
    public float activeDuration = 2f; // Durata per cui il GameObject principale rimane attivo
    public GameObject sound;
    void OnEnable()
    {
        // Chiama direttamente il metodo per l'istanza del testo
        ass();
    }

    public void ass()
    {
        Instantiate(sound, new Vector3(1.7f, 3f, gameObject.transform.position.z), Quaternion.identity);
        // Verifica che i riferimenti non siano nulli
        if (textPrefab != null && canvasTransform != null)
        {

            // Istanziamento del testo prefab
            GameObject textInstance = Instantiate(textPrefab);

            // Verifica iniziale della posizione e dimensioni
            RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();
            Debug.Log($"Prefab instantiated with size: {textRectTransform.rect.size} and position: {textRectTransform.anchoredPosition}");

            // Posizionamento
            RectTransform canvasRectTransform = (RectTransform)canvasTransform;
            Vector2 canvasCenter = canvasRectTransform.rect.center;
            Vector2 offsetPosition = canvasCenter + new Vector2(offsetX, offsetY);
            textRectTransform.SetParent(canvasTransform, false);
            textRectTransform.localScale = Vector3.one;
            textRectTransform.anchoredPosition = offsetPosition;

            // Log per verificare le nuove dimensioni e posizione
            Debug.Log($"Text position after adjustment: {textRectTransform.anchoredPosition}");

            // Avvia una coroutine per disattivare il GameObject principale dopo un certo tempo
            StartCoroutine(DeactivateAfterDelay());
        }
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(activeDuration);
        gameObject.SetActive(false); // Disattiva il GameObject principale
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo"))
        {
            // Logica quando l'oggetto è in collisione con "Cubo"
        }
    }

    void Update()
    {
        // Logica aggiornata ogni frame
    }
}
