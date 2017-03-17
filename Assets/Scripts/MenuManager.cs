using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //Added the using scenemanagement namespace because this is how you have to do it now.
using System.Collections;

public class MenuManager : MonoBehaviour {

    bool controlsShowing = false;

    //Load a level with the passed in integer.
    public void LoadLevel(int levelToLoad)
    {
        //Loads the passed in level in single mode (Meaning there will be no other scenes in the background).
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
    }

    //Exits the application.
    public void ExitGame()
    {
        Application.Quit();
    }

    //Loads the controls menu.
    public void LoadControls(bool unload)
    {
        //Finds the preset animation for the controls.
        Animation controlsAnim = GameObject.Find("Canvas").transform.Find("ControlsPanel").GetComponent<Animation>();

        //Plays the controls animation which heightens the alpha of the canvas group.
        controlsAnim.Play();
    }
}
