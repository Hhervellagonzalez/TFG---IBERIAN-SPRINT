using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class personaje : MonoBehaviour {
    public float anchoDelCarril = 3.0f; 
    private int carrilActual = 1; 
    private bool estaVivo = true;

    private Vector3 posicionObjetivo; 
    private bool estaSaltando = false;
    private float alturaDelSalto = 10f; 
    private float gravedad = 32f; 
    private Vector3 velocidadVertical = Vector3.zero;
    private Animator animator; 

   void Start()
{
    posicionObjetivo = transform.position;
    animator = GetComponent<Animator>();
    estaVivo = true;
}

    void Update()
    {
        ManejarCambioDeCarril();
        MoverAPosicionObjetivo();
        ManejarSalto();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

   void ManejarCambioDeCarril()
{
    if (!estaVivo) return;  

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
    if (!estaVivo) return; 

    if (Input.GetKeyDown(KeyCode.W) && !estaSaltando)
    {
        estaSaltando = true;
        velocidadVertical.y = Mathf.Sqrt(alturaDelSalto * 2 * gravedad); 
        animator.SetTrigger("saltar");  
    }
    if (estaSaltando)
    {
        velocidadVertical.y -= gravedad * Time.deltaTime; 
        transform.position += velocidadVertical * Time.deltaTime;
        if (transform.position.y < posicionObjetivo.y)
        {
            transform.position = new Vector3(transform.position.x, posicionObjetivo.y, transform.position.z);
            estaSaltando = false;
            velocidadVertical = Vector3.zero; 
        }
    }
}

void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("Obstaculo"))
    {
        Morir();
    }
}

void Morir()
{
    estaVivo = false;  // Establece el estado del personaje a muerto.
    animator.SetTrigger("chocar");  // Activa la animación de muerte.
    GameManager.Instance.JugadorMuerto();  // Notifica al GameManager que el jugador ha muerto.
}

    void MoverAPosicionObjetivo()
    {
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, 10f * Time.deltaTime);
    }
}

