using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneyfake : MonoBehaviour
{
    public GameObject money;
    public int monei;
    public int points;
    public GameObject moneyearned;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        // Imposta la scala iniziale al massimo
        if(moneyearned != null )points = int.Parse(moneyearned.GetComponent<TextMeshProUGUI>().text);
        Debug.Log(money.GetComponent<BaseChange>().Money);
       monei = money.GetComponent<BaseChange>().Money;
        TextMeshProUGUI tmpmine = GetComponent<TextMeshProUGUI>();
        tmpmine.text = (monei).ToString(); 
        
        // Prova a trovare l'oggetto subito
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
