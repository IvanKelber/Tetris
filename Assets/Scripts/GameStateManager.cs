using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    const int MENU_SCENE = 0;
    const int GAME_SCENE = 1;
    const int LOSE_SCENE = 2;

    public static int final_score = 0;
    public static int lines = 0;

    public static void PlayGame()
    {
        Debug.Log("Playing the game...");
        SceneManager.LoadScene(GAME_SCENE);
    }

    public static void Menu()
    {
        SceneManager.LoadScene(MENU_SCENE);
    }

    public static void Lose()
    {
        SceneManager.LoadScene(LOSE_SCENE);
    }

    public static void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
