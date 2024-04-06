using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public Transform contenedorCarriles;
    public Transform[] posicionesCarriles;
    public int indiceCarrilActual = 1;
    public float velocidadHorizontalJugador = 10f;
    public float fuerzaSalto = 10f;
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
    }

    void MoverCarril(int direccion)
    {
        indiceCarrilActual += direccion;
    }

    void Salto()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
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
