using System.Collections;
using UnityEngine;

public class ShootingTimer : MonoBehaviour
{
    public GameObject starPrefab; // Drag & drop il prefab della stella qui nel Inspector
    public Transform spawnPoint; // Facoltativo: se vuoi specificare un punto di spawn
    public bool isFirstTimerCompleted; // Indica se il primo timer di 20-60 secondi è stato completato

    private Coroutine starCoroutine;

    private void OnEnable()
    {
        bool isNight = PlayerPrefs.GetInt("IsDay", 0) == 0;

        if (isNight) StartCoroutine(SpawnStars());

    }

    // Metodo pubblico per avviare l'istanziamento delle stelle
    public void StartSpawningStars()
    {
        bool isNight = PlayerPrefs.GetInt("IsDay", 0) == 0;

        if (isNight) Instantiate(starPrefab, spawnPoint.position, Quaternion.identity);

    }

    private IEnumerator SpawnStars()
    {
        while (true)
        {
            // Aspetta un intervallo casuale tra 20 e 60 secondi
            float waitTime = Random.Range(20f, 30f);
            yield return new WaitForSeconds(waitTime);

            // Istanzia la stella senza alcun parent
            Instantiate(starPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
