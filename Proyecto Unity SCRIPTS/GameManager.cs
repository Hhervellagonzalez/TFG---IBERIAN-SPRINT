using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int contador = 0;
     public movimientoMapa movimientoMapaScript;
    public TextMeshProUGUI contadorTexto; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementarContador()
    {
        contador++;
        contadorTexto.text = "Recogidos: " + contador;  
    }
     public void JugadorMuerto()
    {
        movimientoMapaScript.DetenerMovimiento(); 
    }
}
