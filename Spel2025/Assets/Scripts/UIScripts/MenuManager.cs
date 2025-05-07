using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator transition;

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        transition.SetTrigger("GameStart");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("ExperienceScene");
    }
}
