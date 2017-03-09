using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class UpgradeManager : MonoBehaviour {

    int speedBonus = 0;
    int brakingBonus = 0;
    int deliveryBonus = 0;

    int[] upgradeLevels = new int[3] { 0, 0, 0 };
    int[] prices = new int[3] { 100, 200, 300 };

    private Text savingsText;


    void Start()
    {
        savingsText = GameObject.Find("Canvas").transform.FindChild("SavingsTxt").GetComponent<Text>();
        savingsText.text = "$" + PlayerPrefs.GetInt("TotalSavings");
    }


    public void Upgrade(char upgrade)
    {
        switch (upgrade)
        {
            case 's':
                if (upgradeLevels[0] < 3)
                {
                    UpSpeed();
                    savingsText.text = ("$" + MakeTransaction(prices[upgradeLevels[0]]));
                }
                break;


            case 'b':
                if (upgradeLevels[1] < 3)
                {
                    UpBraking();
                    MakeTransaction(prices[upgradeLevels[1]]);
                }
                break;


            case 'd':
                if (upgradeLevels[2] < 3)
                {
                    UpDelivery();
                    MakeTransaction(prices[upgradeLevels[2]]);
                }
                break;
        }
    }


    void UpSpeed()
    {
        speedBonus += 10;
        PlayerPrefs.SetInt("SpeedBonus", speedBonus);
    }

    void UpBraking()
    {
        brakingBonus += 10;
        PlayerPrefs.SetInt("BrakingBonus", brakingBonus);
    }

    void UpDelivery()
    {
        deliveryBonus += 10;
        PlayerPrefs.SetInt("DeliveryBonus", deliveryBonus);
    }



    int MakeTransaction(int amount)
    {
        int currentSavings = PlayerPrefs.GetInt("TotalSavings");
        PlayerPrefs.SetInt("TotalSavings", currentSavings - amount);

        return PlayerPrefs.GetInt("TotalSavings");
    }
}
