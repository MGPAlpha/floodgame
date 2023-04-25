using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartButton() {
        SceneManager.LoadScene("Intro");
    }

    public void LoadMainScene() {
        SelectionManager.takenObjects.Clear();
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton() {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
