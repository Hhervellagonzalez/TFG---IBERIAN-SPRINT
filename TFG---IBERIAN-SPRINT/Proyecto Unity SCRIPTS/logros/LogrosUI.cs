using UnityEngine;

public class LogrosUI : MonoBehaviour
{
    public GameObject logrosPanel; 

    private void Start()
    {
        logrosPanel.SetActive(false); 
    }

    public void MostrarLogros()
    {
        logrosPanel.SetActive(true); 
    }

    public void OcultarLogros()
    {
        logrosPanel.SetActive(false); 
    }
}