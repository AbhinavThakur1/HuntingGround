using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function loads the next scene (like your Game Scene)
    public void StartGame()
    {
        // Replace "GameScene" with your actual scene name
        SceneManager.LoadScene("Game Level");
    }

    // This function quits the game (works in build, not in Editor)
    public void QuitGame()
    {
        Debug.Log("Quit Game!"); // For testing inside Unity Editor
        Application.Quit();
    }
}

