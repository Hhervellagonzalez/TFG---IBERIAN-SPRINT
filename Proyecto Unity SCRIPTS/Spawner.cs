using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float tiempoEntreSpawn = 3f;
    [SerializeField] private GameObject[] prefabsObstaculos;
    [SerializeField] private Transform[] carriles;
    [SerializeField] private float velocidadInicialObstaculos = 8f;
    [SerializeField] private float incrementoVelocidad = 0.1f;
    private float velocidadActualObstaculos;

    private void Start()
    {
        velocidadActualObstaculos = velocidadInicialObstaculos;
        InvokeRepeating("SpawnObstaculo", 0f, tiempoEntreSpawn);
        InvokeRepeating("AumentarVelocidad", 10f, 10f); // Aumenta la velocidad cada 10 segundos, ajusta esto como prefieras.
    }

    private void SpawnObstaculo()
    {
        InstanciarObstaculo();

        // Genera un segundo obstáculo con una probabilidad del 50%
        if (Random.value <= 0.5f)
        {
            InstanciarObstaculo();
        }
    }

    private void InstanciarObstaculo()
    {
        int indiceCarril = Random.Range(0, carriles.Length);
        Transform carrilElegido = carriles[indiceCarril];
        GameObject obstaculoPrefab = prefabsObstaculos[Random.Range(0, prefabsObstaculos.Length)];
        Vector3 posicionSpawn = new Vector3(carrilElegido.position.x, transform.position.y, transform.position.z);
        Quaternion rotacion = Quaternion.Euler(0, 180, 0);

        GameObject obstaculo = Instantiate(obstaculoPrefab, posicionSpawn, rotacion);
        obstaculo.GetComponent<movObjeto>().SetSpeed(velocidadActualObstaculos);
    }

    private void AumentarVelocidad()
    {
        velocidadActualObstaculos += incrementoVelocidad;
    }
}
