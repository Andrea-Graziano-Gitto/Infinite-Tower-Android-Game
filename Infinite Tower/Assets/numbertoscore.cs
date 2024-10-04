using System.Collections;
using UnityEngine;
using TMPro;

public class NumberToScore : MonoBehaviour
{
    public string scoreName = "arrivo"; // Nome dell'oggetto di destinazione
    public string scoreTag = "ScoreTarget"; // Usa un tag come alternativa
    public int points;
    public float moveSpeed = 5f; // Velocità di movimento
    public float minScale = 0.15f; // Scala minima
    public float maxScale = 1f; // Scala massima
    public Transform parentTransform; // Questo sarà impostato come il primo parent di "arrivo"
    private Transform targetTransform; // Transform dell'oggetto "arrivo"
    private bool isMoving = true;
    private bool targetFound = false;

    void OnEnable()
    {
        // Imposta la scala iniziale al massimo
        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        LoadColor(gameObject.transform.GetComponent<TextMeshProUGUI>());
        // Prova a trovare l'oggetto subito
        FindTarget();
    }

    void FindTarget()
    {
        // Prova a trovare l'oggetto per nome
        GameObject scoreObject = GameObject.Find(scoreName);

        // Se non trovato per nome, prova con il tag
        if (scoreObject == null)
        {
            scoreObject = GameObject.FindWithTag(scoreTag); // Usa il tag come backup
        }

        // Verifica se l'oggetto è stato trovato
        if (scoreObject != null)
        {
            targetTransform = scoreObject.transform;
            parentTransform = targetTransform.parent; // Assegna il primo parent di "arrivo" a parentTransform
            targetFound = true;
        }
        else
        {
            Debug.LogWarning($"Oggetto con il nome/tag {scoreName}/{scoreTag} non trovato.");
            isMoving = false; // Ferma il movimento se non trovi l'oggetto
        }
    }

    void Update()
    {
        if (!targetFound)
        {
            // Se non hai ancora trovato il target, cerca di nuovo
            FindTarget();
        }

        if (isMoving && targetTransform != null)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Movimento morbido verso il target
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, step);

        // Riduzione delle dimensioni basata sulla distanza
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        float maxDistance = 10f; // Distanza massima per la scala
        float scale = Mathf.Lerp(minScale, maxScale, 1 - Mathf.Clamp01(distance / maxDistance));
        transform.localScale = new Vector3(scale, scale, scale);

        // Verifica se è arrivato a destinazione
        if (distance < 0.1f)
        {
            isMoving = false;
            AddPointsToScore();
            Destroy(gameObject);
        }
    }

    void AddPointsToScore()
    {
        if (targetTransform != null && parentTransform != null)
        {
            // Trova il componente TextMeshProUGUI nel parent e aggiorna la stringa
            TextMeshProUGUI scoreText = parentTransform.GetComponent<TextMeshProUGUI>();
            if (scoreText != null)
            {
                int currentScore = 0;
                if (int.TryParse(scoreText.text, out currentScore))
                {
                    scoreText.text = (currentScore + points).ToString();
                    // Carica il colore salvato e applicalo al testo
                    LoadColor(scoreText);
                }
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI non trovato sul parent dell'oggetto di destinazione.");
            }
        }
    }

    private void LoadColor(TextMeshProUGUI scoreText)
    {
        float r = PlayerPrefs.GetFloat("SavedColorR", 1f); // Default a bianco
        float g = PlayerPrefs.GetFloat("SavedColorG", 1f);
        float b = PlayerPrefs.GetFloat("SavedColorB", 1f);
        float a = PlayerPrefs.GetFloat("SavedColorA", 1f);
        Color savedColor = new Color(r, g, b, a);
        if (savedColor == Color.white)
        {
            scoreText.color = Color.white;
        }
        else
        {
            // Carica il colore salvato dai PlayerPrefs
            

            // Converti il colore in HSV
            Color.RGBToHSV(savedColor, out float h, out float s, out float v);

            // Imposta il nuovo valore H e mantiene S e V
            h = Mathf.Clamp(h, 0f, 1f); // Assicurati che H sia tra 0 e 1
            s = 0.15f; // Imposta S a 15%
            v = 1f; // Imposta V a 100%

            // Converte di nuovo in RGB
            Color newColor = Color.HSVToRGB(h, s, v);

            // Applica il nuovo colore al testo
            scoreText.color = newColor;
        }
        // Debug: stampa il colore caricato
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("score"))
        {
            if (isMoving)
            {
                isMoving = false;
                AddPointsToScore();
                Destroy(gameObject);
            }
        }
    }
}
