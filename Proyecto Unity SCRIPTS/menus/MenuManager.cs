using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loginMenu;
    public GameObject loadingScreen;
    public GameObject registerMenu;
    
    void Start()
    {
        StartCoroutine(TransitionFromLoadingToMainMenu());
       
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        loginMenu.SetActive(false);
        registerMenu.SetActive(false);
    }
    void Update()
{
    if (Input.GetKeyDown(KeyCode.L))
    {
        ShowLoginMenu();
    }
}


    public void ShowLoginMenu()
    {
         Debug.Log("Iniciando sesión...");
        mainMenu.SetActive(false);
        loginMenu.SetActive(true);
        registerMenu.SetActive(false);
    }

    public void ShowRegisterMenu()
    {
        mainMenu.SetActive(false);
        loginMenu.SetActive(false);
        registerMenu.SetActive(true);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private IEnumerator TransitionFromLoadingToMainMenu()
{
    yield return new WaitForSeconds(3);
    loadingScreen.SetActive(false); 
    ShowMainMenu(); 
}
}
