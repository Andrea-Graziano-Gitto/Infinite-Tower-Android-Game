using System.Collections;
using UnityEngine;
using TMPro;

public class creanuovavita : MonoBehaviour
{
    public GameObject oney;
    public GameObject sound;
    public GameObject salvavita;
    public TMP_Text testoDaRidurre; // Riferimento al componente TMP_Text
    public float durata = 1f; // Durata totale per ridurre il valore

    public void compravita()
    {
        if (oney.GetComponent<BaseChange>().Money >= 50)
        {
            int importo = 50;
            int valoreAttuale;
            oney.GetComponent<BaseChange>().Money -= importo;
            oney.GetComponent<BaseChange>().SavePlayerPrefs();
            // Prova a convertire il testo in un intero
            if (int.TryParse(testoDaRidurre.text, out valoreAttuale) && valoreAttuale >= importo)
            {
                StartCoroutine(RiduciValore(valoreAttuale, importo));
                GameObject bas = Instantiate(salvavita, salvavita.transform.position, Quaternion.identity);
                bas.SetActive(true);
                Instantiate(sound, salvavita.transform.position, Quaternion.identity);
            }
            else
            {
                // Gestisci caso in cui il valore non è sufficiente
            }
        }
    }

    private IEnumerator RiduciValore(int valoreAttuale, int importo)
    {
       

       for(int i = 0; i < importo+1; i++)
        {
            int nuovoValore = valoreAttuale - i; // Calcola il valore finale
            testoDaRidurre.text = nuovoValore.ToString();
            yield return null; // Aspetta il frame successivo
        }

        // Assicurati che il valore finale sia corretto
      
    }
}
