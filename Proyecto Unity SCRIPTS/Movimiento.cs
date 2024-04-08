using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public Transform contenedorCarriles;
    public Transform[] posicionesCarriles;
    public int indiceCarrilActual = 1;
    public float velocidadHorizontalJugador = 10f;
    public float fuerzaSalto = 10f;
    public float gravedadExtra = 2.5f; // Ajusta este valor para controlar cuánto más rápido cae el personaje
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        posicionesCarriles = new Transform[contenedorCarriles.childCount];
        for (int i = 0; i < contenedorCarriles.childCount; i++)
        {
            posicionesCarriles[i] = contenedorCarriles.GetChild(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && indiceCarrilActual > 0)
        {
            MoverCarril(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && indiceCarrilActual < posicionesCarriles.Length - 1)
        {
            MoverCarril(1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && EstaEnElSuelo())
        {
            Salto();
        }
    }

    void FixedUpdate()
    {
        Vector3 objetivoPosicion = new Vector3(posicionesCarriles[indiceCarrilActual].position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, objetivoPosicion, velocidadHorizontalJugador * Time.fixedDeltaTime);

        if (!EstaEnElSuelo())
        {
            AplicarGravedadExtra();
        }
    }

    void MoverCarril(int direccion)
    {
        indiceCarrilActual += direccion;
    }

    void Salto()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }

    void AplicarGravedadExtra()
    {
        
        rb.AddForce(Vector3.down * gravedadExtra);
    }

    bool EstaEnElSuelo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            return true;
        }
        return false;
    }
}
