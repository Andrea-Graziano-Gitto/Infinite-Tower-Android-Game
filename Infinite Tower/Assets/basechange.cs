using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Importa la libreria TextMeshPro

public class BaseChange : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundEntry
    {
        public GameObject background; // Il GameObject dello sfondo
        public bool isUnlocked;       // Stato di sblocco
    }

    public int Money = 0;
    public TextMeshProUGUI moneyText; // Riferimento al TextMeshPro per visualizzare i soldi
    public List<BackgroundEntry> backgrounds; // Lista degli sfondi con stato di sblocco
    public GameObject custom;                 // Riferimento al GameObject "custom"
    public GameObject lockIcon;               // Icona del lucchetto condivisa tra tutti gli sfondi
    private int currentIndex = 0;             // Indice dello sfondo attivo
    private int savedBackgroundIndex = 0;     // Indice dello sfondo salvato
    public GameObject explode;

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("animatocaricamento", 1);
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetInt("animatocaricamento", 1);
            // L'app è stata messa in pausa (o chiusa)
            Debug.Log("Applicazione in pausa o chiusa");
            // Salva le preferenze o esegui altre azioni
        }
        else
        {
            // L'app è stata riattivata
            Debug.Log("Applicazione riattivata");
            // Riprendi eventuali attività necessarie
        }
    }
    void Start()
    {
        backgrounds[0].isUnlocked = true;
       


        PlayerPrefs.SetInt("BackgroundUnlocked_0", backgrounds[0].isUnlocked ? 1 : 0);

        PlayerPrefs.Save(); // Assicurati di salvare le modifiche
        // Carica i dati dai PlayerPrefs all'inizio
        LoadPlayerPrefs();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        // Aggiorna il testo dei soldi
        UpdateMoneyText();
    }

    void OnEnable()
    {
        //PlayerPrefs.SetInt("Money", 0);
      // LockAllBackgroundsExceptFirst();
        // Carica i dati dai PlayerPrefs ogni volta che l'oggetto è abilitato
        LoadPlayerPrefs();
        UpdateMoneyText(); // Aggiorna il testo dei soldi
        LoadSavedBackground(); // Attiva solo lo sfondo salvato
    }

    public void LockAllBackgroundsExceptFirst()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].isUnlocked = (i == 0); // Sblocca solo il primo sfondo
        }
        SavePlayerPrefs(); // Salva lo stato aggiornato
    }


    void Update()
    {
        // Se il gameobject "custom" è attivo, aggiorna lo stato del lucchetto
        if (custom != null && custom.activeSelf)
        {
            UpdateLockIcon();
            UpdateMoneyText();
        }
        else
        {
            // Se "custom" è disattivato, attiva solo lo sfondo salvato
            LoadSavedBackground();
        }
    }

    // Funzione per aggiornare l'icona del lucchetto
    private void UpdateLockIcon()
    {
        lockIcon.SetActive(!backgrounds[currentIndex].isUnlocked);
    }

    // Funzione per aggiornare il testo dei soldi
    private void UpdateMoneyText()
    {
        moneyText.text = Money.ToString(); // Aggiorna il testo con il valore di Money
    }

    private IEnumerator RiduciValore()
    {


        for (int i = 0; i < 100; i++)
        {
           Money  -=  1; // Calcola il valore finale
            
            UpdateMoneyText(); // Aggiorna il testo dei soldi dopo l'acquisto
            SavePlayerPrefs();  // Salva lo stato aggiornato
            yield return null; // Aspetta il frame successivo
        }

        // Assicurati che il valore finale sia corretto

    }

    public void BuyBackground()
    {
        if (Money >= 100 && !backgrounds[currentIndex].isUnlocked)
        {
            backgrounds[currentIndex].isUnlocked = true;
            StartCoroutine(RiduciValore());
            
            Instantiate(explode, transform.position, Quaternion.identity);
            if (backgrounds[currentIndex].isUnlocked)
            {
                savedBackgroundIndex = currentIndex;
                SavePlayerPrefs(); // Salva il nuovo indice dello sfondo sbloccato
            }
        }
    }

    // Metodo per cambiare al prossimo sfondo
    public void ChangeNext()
    {
        // Disattiva lo sfondo corrente
        backgrounds[currentIndex].background.SetActive(false);

        // Incrementa l'indice e avvolge se supera la lunghezza della lista
        currentIndex = (currentIndex + 1) % backgrounds.Count;

        // Attiva il nuovo sfondo, indipendentemente dal suo stato di sblocco
        backgrounds[currentIndex].background.SetActive(true);

        // Aggiorna l'icona del lucchetto in base allo stato di sblocco
        UpdateLockIcon();

        // Se lo sfondo è sbloccato, salva l'indice
        if (backgrounds[currentIndex].isUnlocked)
        {
            savedBackgroundIndex = currentIndex;
            SavePlayerPrefs(); // Salva il nuovo indice dello sfondo sbloccato
        }
    }

    // Metodo per cambiare allo sfondo precedente
    public void ChangePrevious()
    {
        // Disattiva lo sfondo corrente
        backgrounds[currentIndex].background.SetActive(false);

        // Decrementa l'indice e avvolge se scende sotto 0
        currentIndex = (currentIndex - 1 + backgrounds.Count) % backgrounds.Count;

        // Attiva il nuovo sfondo, indipendentemente dal suo stato di sblocco
        backgrounds[currentIndex].background.SetActive(true);

        // Aggiorna l'icona del lucchetto in base allo stato di sblocco
        UpdateLockIcon();

        // Se lo sfondo è sbloccato, salva l'indice
        if (backgrounds[currentIndex].isUnlocked)
        {
            savedBackgroundIndex = currentIndex;
            SavePlayerPrefs(); // Salva il nuovo indice dello sfondo sbloccato
        }
    }

    // Funzione per attivare solo lo sfondo salvato e disattivare gli altri
    public void LoadSavedBackground()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            // Attiva solo lo sfondo salvato, disattiva gli altri
            backgrounds[i].background.SetActive(i == savedBackgroundIndex);
        }

        // Imposta currentIndex allo sfondo salvato
        currentIndex = savedBackgroundIndex;

        // Aggiorna l'icona del lucchetto in base allo sfondo salvato
        UpdateLockIcon();
    }

    // Funzione per salvare lo stato attuale nei PlayerPrefs
    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("SavedBackgroundIndex", savedBackgroundIndex);

        // Salva lo stato di sblocco di ciascun sfondo
        for (int i = 0; i < backgrounds.Count; i++)
        {
            PlayerPrefs.SetInt("BackgroundUnlocked_" + i, backgrounds[i].isUnlocked ? 1 : 0);
        }

        PlayerPrefs.Save(); // Salva immediatamente le modifiche
    }

    // Funzione per caricare lo stato dai PlayerPrefs
    private void LoadPlayerPrefs()
    {
        Money = PlayerPrefs.GetInt("Money", 100); // Default a 100 se non esiste
        savedBackgroundIndex = PlayerPrefs.GetInt("SavedBackgroundIndex", 0); // Default al primo sfondo

        // Carica lo stato di sblocco di ciascun sfondo
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (PlayerPrefs.HasKey("BackgroundUnlocked_" + i))
            {
                backgrounds[i].isUnlocked = PlayerPrefs.GetInt("BackgroundUnlocked_" + i, 0) == 1;
            }
            else
            {
                // Blocca tutti gli sfondi tranne il primo
                backgrounds[i].isUnlocked = (i == 0);
            }
        }
    }

}
