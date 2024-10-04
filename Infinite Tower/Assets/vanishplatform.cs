using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanishplatform : MonoBehaviour
{
    public GameObject dinamicizza; // L'oggetto che diventerà dinamico
    private Material material; // Il materiale della piattaforma (primo child)
    private Color originalColor; // Il colore originale del materiale
    private bool isVanishing = false; // Flag per gestire la scomparsa

    public float fadeDuration = 5f; // Durata del fading in secondi
    public float transparencyThreshold = 0.05f; // Soglia di trasparenza per distruggere l'oggetto
    public float delayBeforeDestroy = 0.5f; // Ritardo prima della distruzione

    void Start()
    {
        // Ottieni il materiale del primo child dell'oggetto
        Renderer childRenderer = transform.GetChild(0).GetComponent<Renderer>();
        if (childRenderer != null)
        {
            material = childRenderer.material;
            originalColor = material.color;
        }
        else
        {
            Debug.LogWarning("Il primo child non ha un componente Renderer.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Controlla se l'oggetto che ha colpito è nel layer "Cubo" e se è kinematic
        if (other.gameObject.layer == LayerMask.NameToLayer("Cubo") && other.attachedRigidbody.isKinematic)
        {
            // Se non sta già scomparendo, inizia il processo
            if (!isVanishing)
            {
                isVanishing = true;
                StartCoroutine(FadeOut());
            }
        }
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        // Finché il tempo non supera la durata di fading
        while (elapsedTime < fadeDuration)
        {
            // Calcola l'alpha (trasparenza) attuale basato sul tempo trascorso
            float newAlpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            material.color = newColor;

            // Aumenta il tempo trascorso
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assicurati che la trasparenza sia ridotta a 0 e distruggi la piattaforma dopo un breve ritardo
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        StartCoroutine(DestroyPlatformWithDelay());
    }

    IEnumerator DestroyPlatformWithDelay()
    {
        // Attiva l'oggetto "dinamicizza"
        dinamicizza.SetActive(true);

        // Rendi dinamico (non-kinematic) il rigidbody se presente
        Rigidbody rb = dinamicizza.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Disattiva il kinematic
        }

        // Aspetta per 0.5 secondi prima di distruggere l'oggetto
        yield return new WaitForSeconds(delayBeforeDestroy);

        // Distruggi la piattaforma originale
        Destroy(gameObject);
    }
}
