using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject iniciarSesionPanel;
    public GameObject registrarsePanel;
    public GameObject menuPrincipalPanel;

    public string url = "http://localhost:8501/";
    public string politica_privacidad = "http://localhost:8501/";

  
    public void OpenURL()
    {
        Application.OpenURL(url);
    }

      public void politicaprivacidad()
    {
        Application.OpenURL(politica_privacidad);
    }

    public void MostrarIniciarSesion()
    {
        DesactivarTodosLosPaneles();
        iniciarSesionPanel.SetActive(true);
    }

    public void MostrarRegistrarse()
    {
        DesactivarTodosLosPaneles();
        registrarsePanel.SetActive(true);
    }

   

    public void MostrarMenuPrincipal()
    {   
        DesactivarTodosLosPaneles();
        menuPrincipalPanel.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }

    private void DesactivarTodosLosPaneles()
    {
        iniciarSesionPanel.SetActive(false);
        registrarsePanel.SetActive(false);
        menuPrincipalPanel.SetActive(false);
    }
}
