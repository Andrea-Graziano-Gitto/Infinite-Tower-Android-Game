using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
 
    public GameObject objectToPool; // Il prefab da poolare
    public int poolSize = 10; // Numero di oggetti da creare
    private List<GameObject> pooledObjects; // Lista degli oggetti poolati

    void Start()
    {
        pooledObjects = new List<GameObject>();

        // Crea gli oggetti poolati
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false); // Disattiva l'oggetto
            pooledObjects.Add(obj); // Aggiungilo alla lista
        }
    }

    public GameObject GetPooledObject()
    {
        // Cerca un oggetto inattivo
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj; // Restituisci l'oggetto inattivo
            }
        }

        // Se non ci sono oggetti inattivi, ne creiamo uno nuovo
        GameObject newObj = Instantiate(objectToPool);
        newObj.SetActive(false);
        pooledObjects.Add(newObj);
        return newObj;
    }
}
