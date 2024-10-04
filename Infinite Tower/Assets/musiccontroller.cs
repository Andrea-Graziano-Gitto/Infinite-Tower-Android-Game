using System.Collections;
using UnityEngine;
using NativeFilePickerNamespace;
using UnityEngine.UI;
using TMPro;

public class MusicController : MonoBehaviour
{
    public GameObject sourcePrefab; // Prefab con un AudioSource
    private GameObject momentanSource; // Riferimento all'AudioSource attuale istanziato

    public AudioClip audioDefault; // Clip audio predefinito
    public AudioClip audioCustom; // Clip audio personalizzato

    // Riferimenti per i bottoni (RawImages)
    public RawImage audioListenerImage; // Volume
    public RawImage audioSourceImage; // Musica
    public RawImage switchDefaultImage; // Default
    public RawImage switchCustomImage; // Custom

    // Texture attiva e inattiva
    public Texture2D audioListenerActiveTexture;
    public Texture2D audioListenerInactiveTexture;
    public Texture2D audioSourceActiveTexture;
    public Texture2D audioSourceInactiveTexture;
    public Texture2D switchActiveTexture;
    public Texture2D switchInactiveTexture;

    // Testo per il nome della canzone personalizzata
    public TMP_Text customTrackNameText;

    public GameObject caricamento; // Riferimento al GameObject di caricamento
    private bool isAudioListenerActive;
    private bool isAudioSourceActive;
    private bool audioInitialized = false;

    void Start()
    {
        

        StartCoroutine(CleanAudioRoutine());
        StartCoroutine(ReplaceAudioSourceEvery20Seconds());
    }

    void Update()
    {
        if (caricamento != null && !caricamento.activeSelf && !audioInitialized)
        {
            InitializeAudioSettings();
            audioInitialized = true;
        }
    }

    private void InitializeAudioSettings()
    {
        // Carica lo stato dell'AudioListener e AudioSource dai PlayerPrefs
        isAudioListenerActive = PlayerPrefs.GetInt("AudioListener", 1) == 1;
        isAudioSourceActive = PlayerPrefs.GetInt("AudioSource", 1) == 1;

        // Se non esiste un'impostazione per IsCustomAudio, imposta su default
        if (!PlayerPrefs.HasKey("IsCustomAudio"))
        {
            PlayerPrefs.SetInt("IsCustomAudio", 0); // Predefinito su audio default
            PlayerPrefs.Save();
        }

        // Imposta il volume iniziale dell'AudioListener
        AudioListener.volume = isAudioListenerActive ? 1 : 0;

        // Controlla se c'è un percorso audio personalizzato salvato nei PlayerPrefs
        string customAudioPath = PlayerPrefs.GetString("CustomAudioPath", "");
        if (!string.IsNullOrEmpty(customAudioPath))
        {
            // Carica l'audio personalizzato se esiste
            StartCoroutine(LoadAudio(customAudioPath));
        }
        else
        {
            // Se non esiste, imposta l'audio predefinito
            ReplaceAudioSource(0f);
        }

        UpdateButtonImages();
        UpdateAudioSwitchImages();
    }

    private IEnumerator ReplaceAudioSourceEvery20Seconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);

            if (momentanSource != null)
            {
                AudioSource currentAudioSource = momentanSource.GetComponent<AudioSource>();
                float currentTime = currentAudioSource.time;
                ReplaceAudioSource(currentTime);
            }
        }
    }

    private void ReplaceAudioSource(float startTime)
    {
        if (momentanSource != null)
        {
            Destroy(momentanSource);
        }

        // Instanzia un nuovo AudioSource
        momentanSource = Instantiate(sourcePrefab, transform);
        AudioSource newAudioSource = momentanSource.GetComponent<AudioSource>();

        // Controlla se è impostato l'audio custom
        bool isCustomAudio = PlayerPrefs.GetInt("IsCustomAudio", 0) == 1;

        // Se è impostato l'audio custom e c'è un audio personalizzato, lo usa
        newAudioSource.clip = isCustomAudio && audioCustom != null ? audioCustom : audioDefault;
        newAudioSource.time = startTime;
        newAudioSource.Play();
        newAudioSource.mute = !isAudioSourceActive;

        UpdateButtonImages();
        UpdateAudioSwitchImages();
    }

    public void ToggleAudioListener()
    {
        isAudioListenerActive = !isAudioListenerActive;
        AudioListener.volume = isAudioListenerActive ? 1 : 0;
        PlayerPrefs.SetInt("AudioListener", isAudioListenerActive ? 1 : 0);
        PlayerPrefs.Save();
        UpdateButtonImages();
    }

    public void ToggleAudioSource()
    {
        isAudioSourceActive = !isAudioSourceActive;
        if (momentanSource != null)
        {
            momentanSource.GetComponent<AudioSource>().mute = !isAudioSourceActive;
        }
        PlayerPrefs.SetInt("AudioSource", isAudioSourceActive ? 1 : 0);
        PlayerPrefs.Save();
        UpdateButtonImages();
    }

    public void SetAudioDefault()
    {
        PlayerPrefs.SetInt("IsCustomAudio", 0);
        PlayerPrefs.Save();
        ReplaceAudioSource(0f);
        UpdateAudioSwitchImages();
    }

    public void SetAudioCustom()
    {
        PlayerPrefs.SetInt("IsCustomAudio", 1);
        PlayerPrefs.Save();
        ReplaceAudioSource(0f);
        UpdateAudioSwitchImages();
    }

    public void PickFile()
    {
        NativeFilePicker.PickFile((path) =>
        {
            if (path != null)
            {
                StartCoroutine(LoadAudio(path));
                PlayerPrefs.SetString("CustomAudioPath", path);
                PlayerPrefs.SetInt("IsCustomAudio", 1);
                PlayerPrefs.Save();
            }
        }, "audio/*");
    }

    private IEnumerator LoadAudio(string path)
    {
        using (var www = new WWW("file:///" + path))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                audioCustom = www.GetAudioClip();
                ReplaceAudioSource(0f);
            }
            else
            {
                Debug.LogError("Errore durante il caricamento dell'audio: " + www.error);
            }
        }
    }

    private void UpdateButtonImages()
    {
        audioListenerImage.texture = isAudioListenerActive ? audioListenerActiveTexture : audioListenerInactiveTexture;
        audioSourceImage.texture = isAudioSourceActive ? audioSourceActiveTexture : audioSourceInactiveTexture;
    }

    private void UpdateAudioSwitchImages()
    {
        if (momentanSource != null)
        {
            AudioSource currentAudioSource = momentanSource.GetComponent<AudioSource>();
            bool isDefaultActive = currentAudioSource.clip == audioDefault;
            bool isCustomActive = currentAudioSource.clip == audioCustom;

            switchDefaultImage.texture = isDefaultActive ? switchActiveTexture : switchInactiveTexture;
            switchCustomImage.texture = isCustomActive ? switchActiveTexture : switchInactiveTexture;
        }
    }

    private IEnumerator CleanAudioRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            ClearUnusedAudioClip();
        }
    }

    private void ClearUnusedAudioClip()
    {
        if (momentanSource != null)
        {
            AudioSource currentAudioSource = momentanSource.GetComponent<AudioSource>();
            if (currentAudioSource.clip != null && !currentAudioSource.isPlaying)
            {
                Debug.Log("Pulizia del clip audio inutilizzato.");
                currentAudioSource.clip.UnloadAudioData();
            }
        }
    }
}
