using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class UpgradeManager : MonoBehaviour {



    void Start()
    {
        
    }

    public void UpgradeSpeed()
    {

    }


    int MakeTransaction(int amount)
    {
        int currentSavings = PlayerPrefs.GetInt("TotalSavings");
        PlayerPrefs.SetInt("TotalSavings", currentSavings - amount);

        return PlayerPrefs.GetInt("TotalSavings");
    }
}
