using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class UpgradeManager : MonoBehaviour {

    //Declares a temporary variable to store total savings
    //so the program doesnt have to keep reading the hard disk.
    private int tempTotalSavings;

    //Declares bonuses.
    int speedBonus = 0;
    int brakingBonus = 0;
    int deliveryBonus = 0;

    //Array for how many levels a bonus has been upgraded.
    int[] upgradeLevels = new int[3] { 0, 0, 0 };
    //Various Prices.
    int[] prices = new int[3] { 100, 200, 300 };

    //Referenced Canvas.
    private GameObject canvas;
    //Text showing savings.
    private Text savingsText;


    void Start()
    {
        //Finds the canvas for referenced canvas objects.
        canvas = GameObject.Find("Canvas");

        //Finds the savings text.
        savingsText = canvas.transform.FindChild("SavingsTxt").GetComponent<Text>();

        PlayerPrefs.SetInt("TotalSavings", 1000);

        //Sets savings text to the players current savings.
        savingsText.text = "$" + PlayerPrefs.GetInt("TotalSavings");
        tempTotalSavings = PlayerPrefs.GetInt("TotalSavings");

        Debug.Log("" + upgradeLevels.Length);
    }


    public void Upgrade(int upgrade)
    {
        //Declares a current upgrade level to be defined later.
        int currentUpgradeLevel;

        //Switches on whichever upgrade.
        switch (upgrade)
        {
            //Speed.
            case 0:
                //Sets current upgrade level.
                currentUpgradeLevel = upgradeLevels[0];
                //If the player isnt on their 3rd upgrade and they have enough money.
                if (upgradeLevels[upgrade] < 3 && tempTotalSavings >= prices[currentUpgradeLevel])
                {
                    //Upgrade the speed variable.
                    UpSpeed();
                    //Set the text to calculate money.
                    savingsText.text = ("$" + MakeTransaction(prices[currentUpgradeLevel]));
                }
                break;

            //Braking.
            case 1:
                //Sets current upgrade level.
                currentUpgradeLevel = upgradeLevels[1];
                //If the player isnt on their 3rd upgrade and they have enough money.
                if (upgradeLevels[upgrade] < 3 && tempTotalSavings >= prices[currentUpgradeLevel])
                {
                    //Upgrade braking variable.
                    UpBraking();
                    //Set the text to calculate money.
                    savingsText.text = ("$" + MakeTransaction(prices[currentUpgradeLevel]));
                }
                break;

            //Delivery.
            case 2:
                //Sets current upgrade level.
                currentUpgradeLevel = upgradeLevels[2];
                //If the player isnt on their 3rd upgrade and they have enough money.
                if (upgradeLevels[upgrade] < 3 && tempTotalSavings >= prices[currentUpgradeLevel])
                {
                    //Upgrade delivery variable.
                    UpDelivery();
                    //Set the text to calculate money.
                    savingsText.text = ("$" + MakeTransaction(prices[currentUpgradeLevel]));
                }
                break;
        }
    }

    //Upgrades attributes
    void UpSpeed()
    {
        speedBonus += 10;
        upgradeLevels[0]++;
        PlayerPrefs.SetInt("SpeedBonus", speedBonus);
    }

    void UpBraking()
    {
        brakingBonus += 10;
        upgradeLevels[1]++;
        PlayerPrefs.SetInt("BrakingBonus", brakingBonus);
    }

    void UpDelivery()
    {
        deliveryBonus += 10;
        upgradeLevels[2]++;
        PlayerPrefs.SetInt("DeliveryBonus", deliveryBonus);
    }


    //Calculates the players savings after money is taken out for upgrade.
    int MakeTransaction(int amount)
    {
        //gets a temporary variable for current savings.
        int currentSavings = PlayerPrefs.GetInt("TotalSavings");
        //Sets total savings as current savings minus the passed in amount.
        PlayerPrefs.SetInt("TotalSavings", currentSavings - amount);

        //Sets temporary total savings to the new total savings.
        tempTotalSavings = PlayerPrefs.GetInt("TotalSavings");

        //Returns total savings.
        return PlayerPrefs.GetInt("TotalSavings");
    }
}
