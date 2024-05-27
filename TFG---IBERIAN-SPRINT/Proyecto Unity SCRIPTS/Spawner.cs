using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spherePrefab;
    public float spawnRate = 2.0f;
    private float[] lanes = { -8f, -5f, -2f };
    [SerializeField] private float tiempoEntreSpawn = 3f;
    [SerializeField] private GameObject[] prefabsObstaculos;
    [SerializeField] private GameObject obstaculoBajoPrefab; // Objeto bajo específico
    [SerializeField] private Transform[] carriles;
    [SerializeField] private Transform mapeado;
    private bool[] carrilesOcupados;
    private float decrementoTiempo = 0.15f; // Decremento del tiempo entre spawns
    private float tiempoMinimoEntreSpawn = 0.1f; // Tiempo mínimo entre spawns
    private float tiempoDecrementoIntervalo = 5f; // Intervalo de tiempo para reducir el tiempo de spawn

    private void Start()
    {
        carrilesOcupados = new bool[carriles.Length];
        InvokeRepeating("SpawnObstaculo", 0f, tiempoEntreSpawn);
        InvokeRepeating("SpawnSphere", 0f, spawnRate);
        StartCoroutine(DisminuirTiempoEntreSpawn());
    }

    private void SpawnObstaculo()
    {
        InstanciarObstaculo();

        if (Random.value <= 0.5f)
        {
            InstanciarObstaculo();
        }

        if (Random.value <= 0.3f)
        {
            InstanciarTresObstaculos();
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

    private void InstanciarTresObstaculos()
    {
        List<int> carrilesLibres = ObtenerCarrilesLibres();
        if (carrilesLibres.Count < 3) return;

        carrilesLibres.Shuffle();
        for (int i = 0; i < 3; i++)
        {
            int indiceCarril = carrilesLibres[i];
            Transform carrilElegido = carriles[indiceCarril];
            GameObject obstaculoPrefab = i == 2 ? obstaculoBajoPrefab : prefabsObstaculos[Random.Range(0, prefabsObstaculos.Length)];
            Vector3 posicionSpawn = new Vector3(carrilElegido.position.x, transform.position.y, transform.position.z);
            Quaternion rotacion = Quaternion.Euler(0, 180, 0);

            GameObject obstaculo = Instantiate(obstaculoPrefab, posicionSpawn, rotacion, mapeado);

            carrilesOcupados[indiceCarril] = true;

            StartCoroutine(LiberarCarril(indiceCarril, 5.0f));

            Destroy(obstaculo, 20f);
        }
    }

    private List<int> ObtenerCarrilesLibres()
    {
        List<int> carrilesLibres = new List<int>();
        for (int i = 0; i < carrilesOcupados.Length; i++)
        {
            if (!carrilesOcupados[i])
            {
                carrilesLibres.Add(i);
            }
        }
        return carrilesLibres;
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

    private IEnumerator DisminuirTiempoEntreSpawn()
    {
        while (tiempoEntreSpawn > tiempoMinimoEntreSpawn)
        {
            yield return new WaitForSeconds(tiempoDecrementoIntervalo);
            tiempoEntreSpawn -= decrementoTiempo;
            CancelInvoke("SpawnObstaculo");
            InvokeRepeating("SpawnObstaculo", tiempoEntreSpawn, tiempoEntreSpawn);
        }
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
