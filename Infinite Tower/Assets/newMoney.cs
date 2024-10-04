using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class newMoney : MonoBehaviour
{
    public string scoreName = "soldi"; // Nome dell'oggetto di destinazione
    public string scoreTag = "ScoreTarget"; // Usa un tag come alternativa
    public int points; // Punti da aggiungere
    public float moveSpeed = 5f; // Velocità di movimento
    public float minScale = 0.15f; // Scala minima
    public float maxScale = 1f; // Scala massima
    public Transform parentTransform; // Questo sarà impostato come il primo parent di "arrivo"
    private Transform targetTransform; // Transform dell'oggetto "arrivo"
    private bool isMoving = true;
    private bool targetFound = false;
    public GameObject moneyearned;

    void OnEnable()
    {
        // Imposta la scala iniziale al massimo
        transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        TextMeshProUGUI tmpmine = GetComponent<TextMeshProUGUI>();
        tmpmine.text = "+" + moneyearned.GetComponent<TextMeshProUGUI>().text;
        points = int.Parse(tmpmine.text);

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
            StartCoroutine(IncrementScore()); // Incrementa il punteggio in modo incrementale
        }
    }

    // Coroutine per incrementare il punteggio in modo graduale
    IEnumerator IncrementScore()
    {
        if (targetTransform != null && parentTransform != null)
        {
            // Trova il componente TextMeshProUGUI nel parent e ottieni il punteggio attuale
            TextMeshProUGUI scoreText = parentTransform.GetComponent<TextMeshProUGUI>();
            if (scoreText != null)
            {
                int currentScore = 0;
                if (int.TryParse(scoreText.text, out currentScore))
                {
                    int targetScore = currentScore + points; // Punteggio target

                    while (currentScore < targetScore)
                    {
                        currentScore++; // Incrementa di 1
                        scoreText.text = currentScore.ToString();
                        yield return new WaitForSeconds(0.01f); // Ritardo per vedere il cambiamento
                    }

                    // Assicurati che il punteggio finale sia esattamente il target
                    scoreText.text = targetScore.ToString();
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI non trovato sul parent dell'oggetto di destinazione.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("score"))
        {
            // Assicurati che non ci siano due chiamate alla funzione AddPointsToScore
            if (isMoving)
            {
                isMoving = false;
                StartCoroutine(IncrementScore());
            }
        }
    }
}
