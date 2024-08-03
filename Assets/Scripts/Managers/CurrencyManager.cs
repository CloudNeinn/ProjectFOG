using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public GameData data;
    public int totalCurrency;
    private TextMeshProUGUI currencyDisplayText;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        currencyDisplayText = GameObject.Find("CharacterCurrencyCount").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void addCurrency(int amount)
    {
        totalCurrency += amount;
        SetCurrency();
    }

    public void removeCurrency(int amount)
    {
        totalCurrency -= amount;
        SetCurrency();
    }

    public void SetCurrency()
    {
        currencyDisplayText.text = totalCurrency.ToString();
    }
}
