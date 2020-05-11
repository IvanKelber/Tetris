using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    const int MENU_SCENE = 0;
    const int GAME_SCENE = 1;

    public static void PlayGame()
    {
        Debug.Log("Playing the game...");
        SceneManager.LoadScene(GAME_SCENE);
    }

    public static void Menu()
    {
        SceneManager.LoadScene(MENU_SCENE);
    }

    public static void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
