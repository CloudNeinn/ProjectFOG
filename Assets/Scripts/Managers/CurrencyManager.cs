using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IDataPersistance
{
    public static CurrencyManager Instance;

    public GameData data;
    public int totalCurrency;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadData(GameData data)
    {
        //totalCurrency = data.totalCurrency;
    }

    public void SaveData(ref GameData data)
    {
        //data.totalCurrency = totalCurrency;
    }

    public void addCurrency(int amount)
    {
        totalCurrency += amount;
    }

    public void removeCurrency(int amount)
    {
        totalCurrency -= amount;
    }
}
