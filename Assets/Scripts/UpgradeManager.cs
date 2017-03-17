using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class UpgradeManager : MonoBehaviour {

    //Declares a temporary variable to store total savings
    //so the program doesnt have to keep reading the hard disk.
    private int tempTotalSavings;

    //Declares bonuses.
    float speedBonus = 1200;
    float brakingBonus = 1.2f;
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

        //Sets savings text to the players current savings.
        savingsText.text = "$" + PlayerPrefs.GetInt("TotalSavings");
        tempTotalSavings = PlayerPrefs.GetInt("TotalSavings");

        //Finds all three progress bar images to show upgrades.
        bars[0] = canvas.transform.Find("SpeedBar").transform.Find("SpeedBar").GetComponent<Image>();
        bars[1] = canvas.transform.Find("BrakeBar").transform.Find("BrakeBar").GetComponent<Image>();
        bars[2] = canvas.transform.Find("DeliveryBar").transform.Find("DeliveryBar").GetComponent<Image>();

        //Gets the upgrade levels from the hard drive.
        upgradeLevels[0] = PlayerPrefs.GetInt("SpeedLevel");
        upgradeLevels[1] = PlayerPrefs.GetInt("BrakingLevel");
        upgradeLevels[2] = PlayerPrefs.GetInt("DeliveryLevel");

        //Loads the progress bars.
        LoadBars();
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

            //Resets the cars upgrades.
            case 3:
                ResetBonuses();
                break;
        }

        //Saves upgrades.
        SaveUpgradeLevels();
        //Loads the progress bars.
        LoadBars();
    }


    //Loads the fill amout of progress bars for each upgrade.
    void LoadBars()
    {
        //Loops through all bars.
        for (int i = 0; i < bars.Length; i++)
        {
            //Checks the upgrade level of that particular bar and sets the fill amount to equal the level.
            switch (upgradeLevels[i])
            {
                case 0:
                    bars[i].fillAmount = 0.0f;
                    break;

                case 1:
                    bars[i].fillAmount = 0.33f;
                    break;

                case 2:
                    bars[i].fillAmount = 0.66f;
                    break;

                case 3:
                    bars[i].fillAmount = 1.0f;
                    break;
            }
        }
    }



    //Upgrades attributes
    void UpSpeed()
    {
        //Increments player speed by 300.
        speedBonus += 300;
        //Adds one to the 'speed bonus' upgrade level.
        upgradeLevels[0]++;
        //Saves the speed bonus.
        PlayerPrefs.SetFloat("SpeedBonus", speedBonus);
    }


    void UpBraking()
    {
        //Decrements speed dampening by 0.2.
        brakingBonus -= 0.2f;
        //Adds a level to the upgrade level.
        upgradeLevels[1]++;
        //Saves the bonus.
        PlayerPrefs.SetFloat("BrakingBonus", brakingBonus);
    }



    void UpDelivery()
    {
        //Increments fire rate by 1.4f.
        deliveryBonus += 1.4f;
        //Adds a level to the upgrade level.
        upgradeLevels[2]++;
        //Saves the value of fire rate.
        PlayerPrefs.SetFloat("DeliveryBonus", deliveryBonus);
    }


    //Resets all upgrades.
    void ResetBonuses()
    {
        //Sets all of the values back to their original states.
        PlayerPrefs.SetFloat("SpeedBonus", 1200);
        PlayerPrefs.SetFloat("BrakingBonus", 1);
        PlayerPrefs.SetFloat("DeliveryBonus", 1.5f);

        //Loops through the array of upgrade levels.
        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            //Sets them all back to 0.
            upgradeLevels[i] = 0;
        }
        //Save the upgrade levels.
        SaveUpgradeLevels();
        //Reloads the current scene to reload bars.
        SceneManager.LoadScene(2);
    }

    //Saves the upgrade levels of each bonus.
    void SaveUpgradeLevels()
    {
        //Saves a float for each upgrade level in the array.
        PlayerPrefs.SetInt("SpeedLevel", upgradeLevels[0]);
        PlayerPrefs.SetInt("BrakingLevel", upgradeLevels[1]);
        PlayerPrefs.SetInt("DeliveryLevel", upgradeLevels[2]);
    }

    //Calculates the players savings after money is taken out for upgrade.
    int MakeTransaction(int amount)
    {
        //Find the animation attached to the savingsText.
        Animation transAnim = savingsText.gameObject.GetComponent<Animation>();

        //If the animation is NOT already playing.
        if (!transAnim.isPlaying)
        {
            //Play the animation.
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
