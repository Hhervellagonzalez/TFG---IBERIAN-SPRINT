using UnityEngine;

public class movObjeto : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float vida = 10;

    void Start()
    {
        Invoke("Destruir", vida);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    public void SetSpeed(float newSpeed)
    {
        velocidad = newSpeed;
    }

    void Destruir()
    {
        Destroy(gameObject);
    }
}
