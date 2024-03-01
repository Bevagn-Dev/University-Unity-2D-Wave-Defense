using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // References to the quit game object
    public GameObject quitObj;

    // Method to handle starting the game
    public void HandlePlay(int sceneIndex)
    {
        // Load the scene with the specified index
        SceneManager.LoadScene(sceneIndex); 
    }

    // Method to handle displaying the quit confirmation menu
    public void HandleQuit()
    {
        // Activate the quit menu
        quitObj.SetActive(true); 
    }

    // Method to handle confirming quitting the application
    public void HandleConfirm()
    {
        // Quit the application
        Application.Quit(); 
    }

    // Method to handle denying quitting the application
    public void HandleDeny()
    {
        // Deactivate the quit menu
        quitObj.SetActive(false); 
    }
}
