using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleManager : MonoBehaviour
{
    
     public int _speed;

    [HideInInspector]
    public int Lenght;
    [HideInInspector]
    public int Streng;
    [HideInInspector]
    public int OfflineEarning;
    [HideInInspector]
    public int LenghtCost;
    [HideInInspector]
    public int StrengCost;
    [HideInInspector]
    public int OfflineEarningCost;
    [HideInInspector]
    public int Wallet;
    [HideInInspector]
    public int TotalGain;

    private int[] costs = new int[]
    {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1151,
        1487,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687,
    };

    public static IdleManager instance;
    void Awake()
    {
        if (IdleManager.instance)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        else
            IdleManager.instance = this;
        Lenght = -PlayerPrefs.GetInt("Lenght",30);
        Streng = PlayerPrefs.GetInt("Streng", 3);
        OfflineEarning = PlayerPrefs.GetInt("Offline", 3);
        LenghtCost = costs[-Lenght / 10 - 3];
        StrengCost = costs[Streng - 3];
        OfflineEarningCost = costs[OfflineEarning - 3];
        Wallet = PlayerPrefs.GetInt("Wallet", 0);
    }

    private void OnApplicationPause(bool paused)
    {
        if(paused)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
        }
        else
        {
            string @string = PlayerPrefs.GetString("Date", string.Empty);
            if(@string != string.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                TotalGain = (int)((DateTime.Now - d).TotalMinutes * OfflineEarning + 1.0f);
                ScreenManager.instanc.ChangeScreen(Screens.Return);
            }
        }
      
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLeght()
    {
        Lenght -= 10;
        Wallet -= LenghtCost;
        LenghtCost = costs[-Lenght / 10 - 3];
        PlayerPrefs.SetInt("Lenght",-Lenght);
        PlayerPrefs.SetInt("Wallet", Wallet);
        ScreenManager.instanc.ChangeScreen(Screens.Main);
    }

    public void BuyStrengh()
    {
        Streng++;
        Wallet -= StrengCost;
        StrengCost = costs[Streng - 3];
        PlayerPrefs.SetInt("Streng", Streng);
        PlayerPrefs.SetInt("Wallet", Wallet);
        ScreenManager.instanc.ChangeScreen(Screens.Main);
    }

    public void BuyOfflineEarning()
    {
        OfflineEarning++;
        Wallet -= OfflineEarningCost;
        OfflineEarningCost = costs[OfflineEarning - 3];
        PlayerPrefs.SetInt("Offline", OfflineEarning);
        PlayerPrefs.SetInt("Wallet", Wallet);
        ScreenManager.instanc.ChangeScreen(Screens.Main);
    }

    public void CollectMoneu()
    {
        Wallet += TotalGain;
        PlayerPrefs.SetInt("Wallet", Wallet);
        ScreenManager.instanc.ChangeScreen(Screens.Main);
    }

    public void CollectDoubleMoneu()
    {
        Wallet += TotalGain * 2;
        PlayerPrefs.SetInt("Wallet", Wallet);
        ScreenManager.instanc.ChangeScreen(Screens.Main);
    }
}
