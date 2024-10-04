using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimazioneCarica : MonoBehaviour
{
    public RawImage arrivo; // La RawImage di destinazione
    public RawImage rawImage; // La RawImage corrente (di questo oggetto)
    //public RawImage sfondo;
    public GameObject camera; // Riferimento alla camera
    public float rotazioneVelocita = 200f; // Velocità della rotazione
    public float velocitaInizialeCaduta = 5f; // Velocità iniziale della caduta
    public float accelerazioneCaduta = 2f; // Accelerazione costante della caduta
    public float fermataRotazione = 0.3f; // Tempo di pausa dopo ogni rotazione
    public GameObject carica;
    public GameObject menu;
    public Material saved; // Materiale di destinazione per la transizione di colore
    public float tempoTransizioneColore = 1f; // Tempo totale per il cambio colore durante la caduta

    public GameObject caricatoSound;
    private Color coloreIniziale; // Colore iniziale del materiale
    private Vector3 posizioneArrivo;
    private Material materialeRawImage; // Riferimento al materiale della RawImage
    //Color coloreInizial = new Color(0f, 0f, 0f, 1f); // Colore iniziale (opaco)
   // Color coloreFinal = new Color(0f, 0f, 0f, 0f); // Colore finale (trasparente)

    void Start()
    {
        // Imposta la PlayerPrefs per animatocaricamento
      

        // Verifica se animatocaricamento è true
        if (PlayerPrefs.GetInt("animatocaricamento", 1) == 1)
        {
            carica.SetActive(true);
        }
        else
        {
            menu.SetActive(true);
            carica.SetActive(false);
            return; // Esci se non deve caricare
        }

        // Ottieni il materiale della RawImage
        materialeRawImage = rawImage.material;
        materialeRawImage.color = Color.white;

        // Salva il colore iniziale del materiale
        coloreIniziale = materialeRawImage.color;

        // Memorizza la posizione di arrivo
        posizioneArrivo = arrivo.transform.position;

        // Posiziona la camera all'inizio
        camera.transform.position = new Vector3(camera.transform.position.x, 20f, camera.transform.position.z);

        // Avvia la coroutine per la sequenza di animazioni
        StartCoroutine(RotazioneEAnimazione());
    }

    IEnumerator RotazioneEAnimazione()
    {
        PlayerPrefs.SetInt("animatocaricamento", 0);
        // Effettua 3 rotazioni di 190 gradi, poi torna indietro di 10 gradi
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(fermataRotazione/2);
            yield return StartCoroutine(Ruota190Gradi());
            yield return StartCoroutine(RuotaIndietro10Gradi());
            yield return new WaitForSeconds(fermataRotazione/2);
        }

        // Dopo il terzo giro, inizia la caduta verso la RawImage di destinazione
        yield return StartCoroutine(AnimazioneCaduta());

        // Imposta animatocaricamento a false
       
    }

    IEnumerator Ruota190Gradi()
    {
        float angoloIniziale = transform.eulerAngles.z;
        float angoloTarget = angoloIniziale + 190f;
        float tempo = 0f;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * rotazioneVelocita / 190f;
            float nuovoAngolo = Mathf.Lerp(angoloIniziale, angoloTarget, tempo);
            transform.rotation = Quaternion.Euler(0f, 0f, nuovoAngolo);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angoloTarget);
    }

    IEnumerator RuotaIndietro10Gradi()
    {
        float angoloIniziale = transform.eulerAngles.z;
        float angoloTarget = angoloIniziale - 10f;
        float tempo = 0f;

        while (tempo < 1f)
        {
            tempo += Time.deltaTime * rotazioneVelocita / 10f;
            float nuovoAngolo = Mathf.Lerp(angoloIniziale, angoloTarget, tempo);
            transform.rotation = Quaternion.Euler(0f, 0f, nuovoAngolo);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angoloTarget);
    }

    IEnumerator AnimazioneCaduta()
    {
        Vector3 posizioneIniziale = transform.position;
        float velocitaCorrente = velocitaInizialeCaduta;
        float distanza = Vector3.Distance(posizioneIniziale, posizioneArrivo);
        float tempoDiCaduta = 0f;

        // Altezza finale della camera
        Vector3 posizioneCameraArrivo = new Vector3(camera.transform.position.x, 15.45f, camera.transform.position.z);

        // Mentre non abbiamo raggiunto l'arrivo, aggiorna la posizione e il colore
        while (Vector3.Distance(transform.position, posizioneArrivo) > 0.1f)
        {
            velocitaCorrente += accelerazioneCaduta * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, posizioneArrivo, velocitaCorrente * Time.deltaTime);

            // Calcola il tempo passato durante la caduta
            tempoDiCaduta += Time.deltaTime;

            // Calcola la progressione del colore basata sul tempo di transizione
            float progressioneColore = Mathf.Clamp01(tempoDiCaduta / tempoTransizioneColore);
            materialeRawImage.color = Color.Lerp(coloreIniziale, saved.color, progressioneColore);
           //sfondo.color = Color.Lerp(coloreInizial, coloreFinal, progressioneColore);
            // Cambia la scala dell'oggetto in modo proporzionale all'arrivo
            transform.localScale = Vector3.Lerp(transform.localScale, arrivo.rectTransform.localScale, tempoDiCaduta);

            // Muovi la camera verso la posizione di arrivo
            camera.transform.position = Vector3.Lerp(camera.transform.position, posizioneCameraArrivo, tempoDiCaduta);

            yield return null;
        }

        // Assicurati che la posizione finale sia esattamente la posizione di arrivo
        transform.position = posizioneArrivo;

        // Attiva il menu e disattiva l'oggetto carica
        Instantiate(caricatoSound, transform.position, Quaternion.identity);
        menu.SetActive(true);
        carica.SetActive(false);
    }
}
