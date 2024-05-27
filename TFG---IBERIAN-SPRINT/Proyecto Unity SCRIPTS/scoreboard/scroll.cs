using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollToTop : MonoBehaviour
{
    public ScrollRect scrollRect;

    void Start()
    {
          Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
     
    }

    public void Salir()
    {
        SceneManager.LoadScene("Lobby");
    }
}
