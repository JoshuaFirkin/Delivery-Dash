﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMaster : MonoBehaviour {

    private GameObject canvas;

    private int currentTips;
    private Text currentTipsText;
    private Text bonusText;
    private Animation bonusAnim;
    private Animation gameOverAnim;
    private Text tipsThisTimeText;
    private Text totalTipsText;
    private Text timerText;
    private Text housesRemainingTxt;
    private Animation remainingAnim;

    private float timeRemaining;
    private float timeElapsed = 0;
    private bool timerActive = false;
    private House[] houses = new House[8];
    private int activeHouses = 4;
    private int remainingHouses;

	void Start ()
    {
        //Finds all needed assets.
        canvas = GameObject.Find("Canvas");
        currentTipsText = canvas.transform.Find("TipsTxt").GetComponent<Text>();
        bonusText = canvas.transform.Find("BonusTxt").GetComponent<Text>();
        bonusAnim = canvas.transform.Find("BonusTxt").GetComponent<Animation>();
        timerText = canvas.transform.Find("TimerTxt").GetComponent<Text>();
        housesRemainingTxt = canvas.transform.Find("RemainingTxt").GetComponent<Text>();
        remainingAnim = housesRemainingTxt.gameObject.GetComponent<Animation>();
        gameOverAnim = canvas.transform.Find("GameOverPanel").GetComponent<Animation>();
        tipsThisTimeText = canvas.transform.Find("GameOverPanel").transform.Find("TipsThisTimeTxt").GetComponent<Text>();
        totalTipsText = canvas.transform.Find("GameOverPanel").transform.Find("TotalTipsTxt").GetComponent<Text>();

        ActivateHouses();
        remainingHouses = activeHouses;
        housesRemainingTxt.text = (remainingHouses + " Remaining");

        //Sets the tips at the start of the game to 0.
        currentTips = 0;
        //30 seconds on timer.
        StartTimer(30);
    }

    void Update()
    {
        //If the timer is active, call countdown.
        if (timerActive)
        {
            Countdown();
        }
    }

    //Chooses which houses are active.
    void ActivateHouses()
    {
        //Declares an array of house gameobject which it finds with the tag "House".
        GameObject[] housesGO = GameObject.FindGameObjectsWithTag("House");
        //Goes through the previous array and gets all of the house scripts from those game objects,
        //adding them to another array of house scripts.
        for (int i = 0; i < housesGO.Length; i++)
        {
            houses[i] = housesGO[i].GetComponent<House>();
        }

        //Declares an int which will choose a random house.
        int randomHouse = 0;
        //Loops from 0 to however many houses are meant to be active in the scene.
        for (int i = 0; i < activeHouses; i++)
        {
            //Chooses a random int from 0 to length of houses.
            randomHouse = Random.Range(0, houses.Length);
            //Checks if chosen house is already active.
            if (houses[randomHouse].GetActiveState())
            {
                //If the house is already active, re-loop without adding i.
                i--;
                continue;
            }
            //If the house is not already active.
            else
            {
                //make it active and re-loop.
                houses[randomHouse].SetActive();
                continue;
            }
        }
    }

    //Takes one house from remaining houses and checks how many houses are left.
    public void MinusOneHouse()
    {
        //Takes a house away.
        remainingHouses -= 1;
        //Informs the player on how many houses are left.
        housesRemainingTxt.text = (remainingHouses + " Remaining");
        remainingAnim.Play();
        //if there are no houses left active.
        if (remainingHouses <= 0)
        {
            //Stop the timer and end the playthrough.
            StopTimer();
            EndCurrentPlaythrough();
        }
    }

    //Takes the value parameter from the house.
    public void AddTips(int tips)
    {
        //Declares a time bonus.
        int timeBonus = 0;

        //If the delivery is done in less than 30 seconds.
        if (timeElapsed <= 30)
        {
            timeBonus = 20;
        }
        //Less than 60 seconds.
        else if (timeElapsed <= 60)
        {
            timeBonus = 10;
        }
        //Less than 1:30.
        else if (timeElapsed <= 90)
        {
            timeBonus = 3;
        }
        //Any more than that, no bonus.
        else
        {
            timeBonus = 0;
        }

        //NOTE: THE BONUSES ABOVE ARE SUBJECT TO TWEAKING.

        //Adds tips to current tips.
        currentTips += tips + timeBonus;
        Debug.Log("Tips: " + currentTips);

        //Adds it to the UI.
        currentTipsText.text = "Tips $" + currentTips;

        if (timeBonus != 0)
        {
            //Bonus text displays bonus.
            bonusText.text = "Speed Bonus +" + timeBonus + "!";
            //Plays a cool animation.
            bonusAnim.Play();
        }
    }

    //Starts the timer with how ever many seconds are passed in.
    void StartTimer(int secondsOnClock)
    {
        //Adds the seconds to the clock.
        timeRemaining = secondsOnClock;
        //Sets the timer as active so it can be called from update.
        timerActive = true;
    }

    void StopTimer()
    {
        //Deactivates the timer so it can no longer be called from update.
        timerActive = false;
        //Sets time back to 0;
        timeRemaining = 0;
    }

    //Called every frame.
    void Countdown()
    {
        //If the time remaining is still greater than 0.
        if(timeRemaining > 0)
        {
            //take away Time.deltaTime.
            timeRemaining -= Time.deltaTime;
            timeElapsed += Time.deltaTime;
        }
        else
        {
            StopTimer();
            EndCurrentPlaythrough();
        }

        //Add the timer to the UI.
        timerText.text = "" + Mathf.RoundToInt(timeRemaining);

        //Sets the colour of the timer text depending on how much time is remaining.
        if (timeRemaining >= 10)
        {
            timerText.color = new Color32(81, 255, 89, 255);
        }
        else if (timeRemaining >= 5)
        {
            timerText.color = new Color32(255, 243, 81, 255);
        }
        else
        {
            timerText.color = new Color32(255, 81, 81, 255);
        }
    }


    public void AddTime(int seconds)
    {
        timeRemaining += seconds;
    }

    //Runs when timer hits 0.
    void EndCurrentPlaythrough()
    {
        //Finds player controller script and disables input.
        PlayerController playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        playerCtrl.DisableInput = true;

        //Sets current tips to display on the menu
        tipsThisTimeText.text = ("Tips Gained: $" + currentTips);

        //Gets the players current total savings from the key.
        int tempTotalSavings = PlayerPrefs.GetInt("TotalSavings");

        //If the player has no previous savings.
        if (tempTotalSavings == 0)
        {
            //Sets the total savings a default value of current tips.
            PlayerPrefs.SetInt("TotalSavings", currentTips);
        }
        else
        {
            //Sets the total savings to current total savings + current tips.
            PlayerPrefs.SetInt("TotalSavings", tempTotalSavings + currentTips);
        }

        //Display total savings.
        totalTipsText.text = ("Total Savings: $" + PlayerPrefs.GetInt("TotalSavings"));
        //Play the game over animation.
        gameOverAnim.Play();
    }
}