using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //Add pause menu
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

    public static bool isPaused;

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
        else if (pauseMenuUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Pause and resume, check if game is paused. As long as gameOverUI is not active
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverUI.activeInHierarchy)
        {
            if (isPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }

    }

    //Activates the GameOverScreen in Canvas
    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    //Pause the game
    public void pauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    //Resume button
    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    //Restart button
    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isPaused = false; //if restart without before dying
        Debug.Log("Restart");
    }

    //Main menu button
    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
        isPaused = false; //If return to main menu from paused screen
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
