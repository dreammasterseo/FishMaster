using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instanc;
    private GameObject CurrenScreen;

    public GameObject EndScreen;
    public GameObject GameScreen;
    public GameObject MainScreen;
    public GameObject ReturnScreen;

    public Button LeghtButton;
    public Button StrengButton;
    public Button OfflineButton;

    public Text GameScreenMoneu;
    public Text LeghtCostText;
    public Text LenghValueText;
    public Text StrenghCostText;
    public Text StrenghValueText;
    public Text OfflineCostText;
    public Text OfflineValueText;
    public Text EndScreenMoneu;
    public Text ReturnScreenMoneu;

    private int GameCount;
    void Awake()
    {
        if (ScreenManager.instanc)
            Destroy(base.gameObject);
        else
            ScreenManager.instanc = this;

        CurrenScreen = MainScreen;
    }

    private void Start()
    {
        UpdateText();
        CheckIdles();
    }

    public void ChangeScreen(Screens screen)
    {
        CurrenScreen.SetActive(false);
        switch(screen)
        {
            case Screens.Main:
                CurrenScreen = MainScreen;
                UpdateText();
                CheckIdles();
                break;

            case Screens.Game:
                CurrenScreen = GameScreen;
                GameCount++;
                break;

            case Screens.End:
                CurrenScreen = EndScreen;
                SetEndScreenMoneu();
                break;

            case Screens.Return:
                CurrenScreen = ReturnScreen;
                SetReturnScreenMoneu();
                break;
                
        }

        CurrenScreen.SetActive(true);
    }

    public void UpdateText()
    {
        GameScreenMoneu.text = "$" + IdleManager.instance.Wallet;
        LeghtCostText.text = "$" + IdleManager.instance.LenghtCost;
        LenghValueText.text = -IdleManager.instance.Lenght + "m";
        StrenghCostText.text = "$" + IdleManager.instance.StrengCost;
        StrenghValueText.text = IdleManager.instance.Streng + "fishes.";
        OfflineCostText.text = "$" + IdleManager.instance.OfflineEarningCost;
        OfflineCostText.text = "$" + IdleManager.instance.OfflineEarning + "/min";
    }

    public void SetEndScreenMoneu()
    {
        EndScreenMoneu.text = "$" + IdleManager.instance.TotalGain;
    }

    public void SetReturnScreenMoneu()
    {
        ReturnScreenMoneu.text = "$" + IdleManager.instance.TotalGain + "gaimed while witing!";
    }

    public void CheckIdles()
    {
        int leghtCost = IdleManager.instance.LenghtCost;
        int streengCost = IdleManager.instance.StrengCost;
        int offlineEarningCost = IdleManager.instance.OfflineEarningCost;
        int wallet = IdleManager.instance.Wallet;

        if (wallet < leghtCost)
            LeghtButton.interactable = false;
        else
            LeghtButton.interactable = true;

        if (wallet < streengCost)
            StrengButton.interactable = false;
        else
            StrengButton.interactable = true;

        if (wallet < offlineEarningCost)
            OfflineButton.interactable = false;
        else
            OfflineButton.interactable = true;
    }
}
