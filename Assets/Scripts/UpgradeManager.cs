using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class UpgradeManager : MonoBehaviour {

    //Declares a temporary variable to store total savings
    //so the program doesnt have to keep reading the hard disk.
    private int tempTotalSavings;

    //Declares bonuses.
    int speedBonus = 1200;
    int brakingBonus = 1;
    float deliveryBonus = 1.5f;

    //Array for how many levels a bonus has been upgraded.
    int[] upgradeLevels = new int[3] { 0, 0, 0 };
    //Various Prices.
    int[] prices = new int[3] { 100, 200, 300 };

    //Referenced Canvas.
    private GameObject canvas;
    //Text showing savings.
    private Text savingsText;

    private Image[] bars = new Image[3];


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

        bars[0] = canvas.transform.Find("SpeedBar").transform.Find("SpeedBar").GetComponent<Image>();
        bars[1] = canvas.transform.Find("BrakeBar").transform.Find("BrakeBar").GetComponent<Image>();
        bars[2] = canvas.transform.Find("DeliveryBar").transform.Find("DeliveryBar").GetComponent<Image>();
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
                    if (speedBonus < 2100)
                    {
                        //Upgrade the speed variable.
                        UpSpeed();
                        //Set the text to calculate money.
                        savingsText.text = ("$" + MakeTransaction(prices[currentUpgradeLevel]));
                    }
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
                    if (deliveryBonus < 4.7f)
                    {
                        //Upgrade delivery variable.
                        UpDelivery();
                        //Set the text to calculate money.
                        savingsText.text = ("$" + MakeTransaction(prices[currentUpgradeLevel]));
                    }
                }
                break;

            case 3:
                ResetBonuses();
                break;
        }
    }

    //Upgrades attributes
    void UpSpeed()
    {
        speedBonus += 300;
        upgradeLevels[0]++;
        PlayerPrefs.SetInt("SpeedBonus", speedBonus);

        switch (upgradeLevels[0])
        {
            case 0:
                bars[0].fillAmount = 0.0f;
                break;

            case 1:
                bars[0].fillAmount = 0.33f;
                break;

            case 2:
                bars[0].fillAmount = 0.66f;
                break;

            case 3:
                bars[0].fillAmount = 1.0f;
                break;
        }
    }

    void UpBraking()
    {
        brakingBonus++;
        upgradeLevels[1]++;
        PlayerPrefs.SetInt("BrakingBonus", brakingBonus);

        switch (upgradeLevels[1])
        {
            case 0:
                bars[1].fillAmount = 0.0f;
                break;

            case 1:
                bars[1].fillAmount = 0.33f;
                break;

            case 2:
                bars[1].fillAmount = 0.66f;
                break;

            case 3:
                bars[1].fillAmount = 1.0f;
                break;
        }
    }

    void UpDelivery()
    {
        deliveryBonus += 1.4f;
        upgradeLevels[2]++;
        PlayerPrefs.SetFloat("DeliveryBonus", deliveryBonus);

        switch (upgradeLevels[2])
        {
            case 0:
                bars[2].fillAmount = 0.0f;
                break;

            case 1:
                bars[2].fillAmount = 0.33f;
                break;

            case 2:
                bars[2].fillAmount = 0.66f;
                break;

            case 3:
                bars[2].fillAmount = 1.0f;
                break;
        }
    }


    void ResetBonuses()
    {
        PlayerPrefs.SetInt("SpeedBonus", 1200);
        PlayerPrefs.SetInt("BrakingBonus", 1);
        PlayerPrefs.SetFloat("DeliveryBonus", 1.5f);

        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            upgradeLevels[i] = 0;
        }

        for (int j = 0; j < bars.Length; j++)
        {
            bars[j].fillAmount = 0;
        }
    }


    //Calculates the players savings after money is taken out for upgrade.
    int MakeTransaction(int amount)
    {
        Animation transAnim = savingsText.gameObject.GetComponent<Animation>();

        if (!transAnim.isPlaying)
        {
            transAnim.Play();
        }

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
