using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //Add pause menu
    public GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //If game over ui is active -> enable cursor else diable
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Activates the GameOverScreen in Canvas
    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    //Restart button
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
    }

    //Main menu button
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Main Menu");
    }

    //Quit button
    public void quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    //Add pause menu functions

}
