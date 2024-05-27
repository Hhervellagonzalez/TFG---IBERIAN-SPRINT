
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugar : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("JUEGO");
    }
    public void Tienda()
    {
        SceneManager.LoadScene("Tienda");
    }
     public void Lobby()
    {
        SceneManager.LoadScene("Lobby");
    }
     public void Selector()
    {
        SceneManager.LoadScene("SelectorPersonaje");
    }
}



