using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMaster : MonoBehaviour {

    private GameObject canvas;

    private int currentTips;
    private Text currentTipsText;

    private float timeRemaining;
    private bool timerActive = false;
    private Text timerText;

    private Animation gameOverAnim;
    private Text tipsThisTimeText;
    private Text totalTipsText;

	void Start ()
    {
        //Finds all needed assets.
        canvas = GameObject.Find("Canvas");
        currentTipsText = canvas.transform.FindChild("TipsTxt").GetComponent<Text>();
        if (currentTipsText == null)
        {
            Debug.Log("Can't Find Text.");
        }

        timerText = canvas.transform.FindChild("TimerTxt").GetComponent<Text>();

        gameOverAnim = canvas.transform.FindChild("GameOverPanel").GetComponent<Animation>();
        tipsThisTimeText = canvas.transform.FindChild("GameOverPanel").transform.FindChild("TipsThisTimeTxt").GetComponent<Text>();
        totalTipsText = canvas.transform.FindChild("GameOverPanel").transform.FindChild("TotalTipsTxt").GetComponent<Text>();

        //Sets the tips at the start of the game to 0.
        currentTips = 0;

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

    //Takes the value parameter from the house.
    public void AddTips(int tips)
    {
        //Adds tips to current tips.
        currentTips += tips;
        Debug.Log("Tips: " + currentTips);
        //Adds it to the UI.
        currentTipsText.text = "Tips $" + currentTips;
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
            Debug.Log(timeRemaining);
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
