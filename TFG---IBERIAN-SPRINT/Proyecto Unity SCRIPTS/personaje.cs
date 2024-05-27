using System.Collections;
using UnityEngine;

public class personaje : MonoBehaviour
{
    public float anchoDelCarril = 3.0f;
    private int carrilActual = 1;
    private bool estaVivo = true;
    private bool tieneVidaExtra = false;

    private Vector3 posicionObjetivo;
    private bool estaSaltando = false;
    private float alturaDelSalto = 10f;
    private float gravedad = 32f;
    private float velocidadVertical = 0f;
    private Animator animator;

    void Start()
    {
        ResetEstadoInicial();
    }

    void Update()
    {
        if (estaVivo)
        {
            ManejarCambioDeCarril();
            MoverAPosicionObjetivo();
            ManejarSalto();
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void ResetEstadoInicial()
    {
        posicionObjetivo = transform.position;
        estaVivo = true;
        animator = GetComponent<Animator>();
        GameManager.Instance.IniciarContador();

        if (PlayerPrefs.GetString("SelectedCharacter", "default") == "Granny")
        {
            tieneVidaExtra = true;
        }
    }

    void ManejarCambioDeCarril()
    {
        if (Input.GetKeyDown(KeyCode.A) && carrilActual > 0)
        {
            carrilActual--;
            posicionObjetivo.x -= anchoDelCarril;
        }
        else if (Input.GetKeyDown(KeyCode.D) && carrilActual < 2)
        {
            carrilActual++;
            posicionObjetivo.x += anchoDelCarril;
        }
    }

    void ManejarSalto()
    {
        if (Input.GetKeyDown(KeyCode.W) && !estaSaltando)
        {
            estaSaltando = true;
            velocidadVertical = Mathf.Sqrt(alturaDelSalto * 2 * gravedad);
            animator.SetTrigger("saltar");
        }
        if (estaSaltando)
        {
            velocidadVertical -= gravedad * Time.deltaTime;
            Vector3 movimientoVertical = new Vector3(0, velocidadVertical * Time.deltaTime, 0);
            transform.position += movimientoVertical;
            if (transform.position.y < posicionObjetivo.y)
            {
                transform.position = new Vector3(transform.position.x, posicionObjetivo.y, transform.position.z);
                estaSaltando = false;
                velocidadVertical = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstaculo"))
        {
            if (tieneVidaExtra)
            {
                tieneVidaExtra = false;
                Destroy(other.gameObject);
                Debug.Log("Vida extra utilizada.");
            }
            else
            {
                Morir();
            }
        }
    }

    void Morir()
    {
        estaVivo = false;
        animator.SetTrigger("chocar");
        Debug.Log("Jugador choca con obstáculo.");
        GameManager.Instance.JugadorMuerto();
    }

    void MoverAPosicionObjetivo()
    {
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, 10f * Time.deltaTime);
    }
}



