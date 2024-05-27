using UnityEngine;

public class Intermitente : MonoBehaviour
{
    public float intervaloEncendido = 0.5f; 
    public float intervaloApagado = 0.5f; 
    private Light luz;
    private bool encendida = true;

    void Start()
    {
        luz = GetComponent<Light>();
        InvokeRepeating("CambiarEstado", intervaloEncendido, intervaloEncendido + intervaloApagado);
    }

    void CambiarEstado()
    {
        encendida = !encendida;
        luz.enabled = encendida;
    }
}