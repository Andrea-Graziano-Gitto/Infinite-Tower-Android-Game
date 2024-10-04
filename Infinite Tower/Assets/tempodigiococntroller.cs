using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Per gestire TextMeshPro

public class TempoDiGiocoController : MonoBehaviour
{
    public bool accellerato;  // Flag per indicare se il gioco è accelerato
    public GameObject pausa;  // Riferimento al menu di pausa
    public GameObject main;   // Riferimento al menu principale
    public GameObject game;   // Riferimento alla schermata di gioco
    public GameObject setting; // Riferimento al menu impostazioni
    public GameObject bscore;  // Riferimento al GameObject "bscore"

    private TextMeshProUGUI bestScoreText;  // Componente TextMeshPro su "bscore"
    private int bestScore = 0;  // Variabile per il miglior punteggio

    // Riferimento allo script della fotocamera
    public camerameuscript cameraScript;

    // Limiti della velocità della fotocamera per scalare Time.timeScale
    private float velocitaIniziale = 0.25f;
    private float velocitaFinale = 0.43f;

    void Start()
    {
        // Imposta il tempo di gioco a 1 all'inizio del gioco
        Time.timeScale = 1;

        // Carica il best score dai PlayerPrefs
        LoadBestScore();

        // Ottieni il componente TextMeshPro dal GameObject bscore
        if (bscore != null)
        {
            bestScoreText = bscore.GetComponent<TextMeshProUGUI>();
            UpdateBestScoreText();  // Aggiorna il testo all'inizio
        }
    }

    void Update()
    {
        if (bscore.activeSelf) UpdateBestScoreText();  // Aggiorna il testo all'inizio

        // Se il menu impostazioni o il menu di pausa sono attivi, pausa il gioco
        if ((setting != null && setting.activeSelf) || (pausa != null && pausa.activeSelf))
        {
            Time.timeScale = 0;
        }
        // Se il menu principale è attivo, riprendi il gioco a velocità normale
        else if (main != null && main.activeSelf)
        {
            Time.timeScale = 1;
        }
        // Se siamo nella schermata di gioco, regola la velocità del gioco in base alla velocità della fotocamera
        else if (game != null && game.activeSelf)
        {
            // Ottieni la velocità della fotocamera dallo script della fotocamera
            float velocita = cameraScript.velocita;

            // Se la velocità è inferiore a 0.35, imposta il timeScale a 1
            if (velocita < 0.25f)
            {
                Time.timeScale = 1f;
            }
            // Se la velocità è pari o superiore a 0.35, inizia a incrementare il timeScale
            else
            {
                // Calcola la percentuale della velocità della fotocamera rispetto ai limiti
                float t = Mathf.InverseLerp(velocitaIniziale, velocitaFinale, velocita);

                // Imposta il timeScale in modo graduale da 1 a 3
                Time.timeScale = Mathf.Lerp(1f, 1.2f, t);
            }
        }

    }

    // Funzione per salvare il best score
    public void SaveBestScore(int newScore)
    {
        if (newScore > bestScore)
        {
            bestScore = newScore;
            PlayerPrefs.SetInt("BestScore", bestScore);  // Salva il nuovo best score
            PlayerPrefs.Save();  // Salva i PlayerPrefs immediatamente
            UpdateBestScoreText();  // Aggiorna il testo
        }
    }

    // Funzione per caricare il best score
    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);  // Carica il best score (default 0)
    }

    // Funzione per aggiornare il testo del best score
    private void UpdateBestScoreText()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = bestScore.ToString();
        }
    }
}
