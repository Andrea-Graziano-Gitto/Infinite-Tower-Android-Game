using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Per usare RawImage
using TMPro;

public class ChangeSkybox : MonoBehaviour
{
    public Material daySkybox;     // Skybox per il giorno
    public Material nightSkybox;   // Skybox per la notte

    public Texture dayImage;       // Immagine per il giorno
    public Texture nightImage;     // Immagine per la notte

    public RawImage targetRawImage; // La RawImage da modificare
    public Light directionalLight; // La luce da cambiare
    public Color dayLightColor;    // Colore della luce diurna (impostato dall'editor)
    public Color nightLightColor;  // Colore della luce notturna (impostato dall'editor)
    public Material uiMaterial; // Il materiale che vuoi applicare
    public bool isDay = true;     // Stato attuale (giorno o notte)
    public RawImage savedcolor;
    public Material colorePlayer;
    public cubeMov cubeMov;

    void Start()
    {
        

        // Carica il colore salvato
        Color savedColor = LoadRawImageColor();
        savedcolor.color = savedColor; // Imposta il colore salvato nella RawImage

        // Carica lo stato salvato (giorno/notte) dai PlayerPrefs
        LoadSkyboxState();

        // Applica il materiale a tutti i componenti UI nel canvas
        if (savedcolor.color == Color.white)
        {
            changecolorToWhite(); // Chiama il metodo per impostare il colore a bianco
        }
        else
        {
            changecolor(savedcolor); // Chiama il metodo per prendere il colore dalla RawImage
        }
    }

    private void SaveRawImageColor(Color color)
    {
        PlayerPrefs.SetFloat("SavedColorR", color.r);
        PlayerPrefs.SetFloat("SavedColorG", color.g);
        PlayerPrefs.SetFloat("SavedColorB", color.b);
        PlayerPrefs.SetFloat("SavedColorA", color.a);
        PlayerPrefs.Save();
    }

    private Color LoadRawImageColor()
    {
        float r = PlayerPrefs.GetFloat("SavedColorR", 1f); // Default a bianco
        float g = PlayerPrefs.GetFloat("SavedColorG", 1f);
        float b = PlayerPrefs.GetFloat("SavedColorB", 1f);
        float a = PlayerPrefs.GetFloat("SavedColorA", 1f);
        return new Color(r, g, b, a);
    }

    public void changecolorToWhite()
    {
        // Imposta il colore a bianco
        Color newColor = Color.white;

        // Assegna il colore bianco al materiale
        uiMaterial.color = newColor;

        // Salva il colore bianco
        SaveRawImageColor(newColor);

        // Applica il materiale e il colore a tutti gli elementi UI
        foreach (var graphic in GetComponentsInChildren<Graphic>(true))
        {
            if (graphic.name != "shade") // Ignora le immagini con nome "shade"
            {
                graphic.material = uiMaterial;
                graphic.color = newColor; // Cambia il colore
            }
        }

        // Applica il colore bianco ai TextMeshPro
        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (text.name != "shade") // Ignora i testi con nome "shade"
            {
                text.fontMaterial.SetColor("_FaceColor", newColor); // Usa il colore bianco
            }
        }
        
     
        cubeMov.targetColor  = LoadRawImageColor(); // Cambia il colore del materiale;
        colorePlayer.color = cubeMov.targetColor;
    }

    public void changecolor(RawImage colorata)
    {
        // Estrai il colore originale dal RawImage
        Color colorFromRawImage = colorata.color;

        // Salva il colore
        SaveRawImageColor(colorFromRawImage);

        // Converti il colore in HSV
        Color.RGBToHSV(colorFromRawImage, out float h, out float s, out float v);

        // Imposta il nuovo valore H e mantiene S e V
        h = Mathf.Clamp(h, 0f, 1f); // Assicurati che H sia tra 0 e 1
        s = 0.15f; // Imposta S a 15%
        v = 1f; // Imposta V a 100%

        // Converte di nuovo in RGB
        Color newColor = Color.HSVToRGB(h, s, v);

        // Assegna il colore al materiale
        uiMaterial.color = newColor;

        // Applica il materiale e il colore a tutti gli elementi UI
        foreach (var graphic in GetComponentsInChildren<Graphic>(true))
        {
            if (graphic.name != "shade") // Ignora le immagini con nome "shade"
            {
                graphic.material = uiMaterial;
                graphic.color = newColor; // Cambia il colore
            }
        }

        // Applica il colore ai TextMeshPro
        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (text.name != "shade") // Ignora i testi con nome "shade"
            {
                text.fontMaterial.SetColor("_FaceColor", newColor); // Usa il nuovo colore
            }
        }
      
        cubeMov.targetColor = LoadRawImageColor(); // Cambia il colore del materiale;
        colorePlayer.color = cubeMov.targetColor;
    }

    // Funzione per cambiare Skybox, immagine e colore della luce
    public void SwitchSkybox()
    {
        if (isDay)
        {
            // Cambia lo Skybox a notte
            RenderSettings.skybox = nightSkybox;

            // Cambia la texture della RawImage all'immagine notturna
            if (targetRawImage != null && nightImage != null)
            {
                targetRawImage.texture = nightImage;
            }

            // Cambia il colore della luce a quello notturno
            if (directionalLight != null)
            {
                directionalLight.color = nightLightColor;
            }

            isDay = false; // Ora è notte
        }
        else
        {
            // Cambia lo Skybox a giorno
            RenderSettings.skybox = daySkybox;

            // Cambia la texture della RawImage all'immagine diurna
            if (targetRawImage != null && dayImage != null)
            {
                targetRawImage.texture = dayImage;
            }

            // Cambia il colore della luce a quello diurno
            if (directionalLight != null)
            {
                directionalLight.color = dayLightColor;
            }

            isDay = true; // Ora è giorno
        }

        // Aggiorna l'illuminazione dinamica
        DynamicGI.UpdateEnvironment();

        // Salva lo stato corrente (giorno/notte) nei PlayerPrefs
        SaveSkyboxState();
    }

    // Funzione per salvare lo stato dello Skybox nei PlayerPrefs
    private void SaveSkyboxState()
    {
        PlayerPrefs.SetInt("IsDay", isDay ? 1 : 0);
        PlayerPrefs.Save(); // Salva immediatamente le modifiche
    }

    // Funzione per caricare lo stato dello Skybox dai PlayerPrefs
    private void LoadSkyboxState()
    {
        // Se non esiste il valore salvato, di default sarà giorno (isDay = true)
        isDay = PlayerPrefs.GetInt("IsDay", 0) == 1;


        // Imposta lo Skybox e la luce in base allo stato caricato
        if (isDay)
        {
            RenderSettings.skybox = daySkybox;
            if (targetRawImage != null && dayImage != null)
            {
                targetRawImage.texture = dayImage;
            }
            if (directionalLight != null)
            {
                directionalLight.color = dayLightColor;
            }
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
            if (targetRawImage != null && nightImage != null)
            {
                targetRawImage.texture = nightImage;
            }
            if (directionalLight != null)
            {
                directionalLight.color = nightLightColor;
            }
        }

        // Aggiorna l'illuminazione dinamica
        DynamicGI.UpdateEnvironment();
    }
}
