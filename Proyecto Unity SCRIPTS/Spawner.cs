using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
      public GameObject spherePrefab; 
    public float spawnRate = 2.0f; 
    private float[] lanes = {-8f, -5f, -2f};
    [SerializeField] private float tiempoEntreSpawn = 3f;
    [SerializeField] private GameObject[] prefabsObstaculos;
    [SerializeField] private Transform[] carriles;
    [SerializeField] private Transform mapeado; 
    private bool[] carrilesOcupados;

    private void Start()
    {
        carrilesOcupados = new bool[carriles.Length];
        InvokeRepeating("SpawnObstaculo", 0f, tiempoEntreSpawn);
        InvokeRepeating("SpawnSphere", 0f, spawnRate);
    }

    private void SpawnObstaculo()
    {
        InstanciarObstaculo();

        if (Random.value <= 0.5f)
        {
            InstanciarObstaculo();
        }
    }
     void SpawnSphere()
    {
        int randomLaneIndex = Random.Range(0, lanes.Length); 
        Vector3 spawnPosition = new Vector3(lanes[randomLaneIndex], 1, transform.position.z);
        Instantiate(spherePrefab, spawnPosition, Quaternion.identity, mapeado);
    }

    private void InstanciarObstaculo()
    {
        int indiceCarril = EncontrarCarrilLibre();
        if (indiceCarril == -1) return;

        Transform carrilElegido = carriles[indiceCarril];
        GameObject obstaculoPrefab = prefabsObstaculos[Random.Range(0, prefabsObstaculos.Length)];
        Vector3 posicionSpawn = new Vector3(carrilElegido.position.x, transform.position.y, transform.position.z);
        Quaternion rotacion = Quaternion.Euler(0, 180, 0);

        GameObject obstaculo = Instantiate(obstaculoPrefab, posicionSpawn, rotacion, mapeado);

        carrilesOcupados[indiceCarril] = true;

        StartCoroutine(LiberarCarril(indiceCarril, 5.0f));

        Destroy(obstaculo, 20f);
    }

    private int EncontrarCarrilLibre()
    {
        for (int i = 0; i < carrilesOcupados.Length; i++)
        {
            if (!carrilesOcupados[i]) return i;
        }
        return -1;
    }

    private IEnumerator LiberarCarril(int indiceCarril, float delay)
    {
        yield return new WaitForSeconds(delay);
        carrilesOcupados[indiceCarril] = false;
    }
}
