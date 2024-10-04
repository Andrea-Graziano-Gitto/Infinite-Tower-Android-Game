using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class INgameScore : MonoBehaviour
{
    private TextMeshProUGUI myScoreText;  // Riferimento al TextMeshPro per il punteggio attuale
    public int mypoints = 0;               // Punteggio corrente
    public GameObject canvas;               // Riferimento al canvas

    private BaseChange baseChange;          // Riferimento allo script BaseChange
    private TempoDiGiocoController gameController; // Riferimento allo script TempoDiGiocoController
    public GameObject gamescreen;
    void Start()
    {
        myScoreText = GetComponent<TextMeshProUGUI>();

        // Ottieni i riferimenti al canvas
        baseChange = canvas.GetComponent<BaseChange>();
        gameController = canvas.GetComponent<TempoDiGiocoController>();

        // Assicurati che i riferimenti siano stati assegnati
        if (baseChange == null)
        {
            Debug.LogError("BaseChange not assigned!");
        }

        if (gameController == null)
        {
            Debug.LogError("TempoDiGiocoController not assigned!");
        }

        // Inizializza Money dai PlayerPrefs
      
    }

    void Update()
    {
        if (myScoreText != null)
        {
            int.TryParse(myScoreText.text, out mypoints); // Converte il testo in numero
        }
    }

    public void sGameOver()
    {
        if (baseChange != null)
        {
           
            baseChange.Money += mypoints; // Aggiungiamo i punti ai soldi
            PlayerPrefs.SetInt("Money", baseChange.Money); // Salviamo il nuovo valore nei PlayerPrefs
        }

        if (gameController != null)
        {
            gameController.SaveBestScore(mypoints); // Salviamo il nuovo best score
        }

        PlayerPrefs.Save(); // Assicurati che i cambiamenti vengano salvati
        if(gamescreen!=null) gamescreen.SetActive(false);
    }
}
