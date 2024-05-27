using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float distanciaRecorrida = 0f;
    public float velocidad = 5.0f;
    public movimientoMapa movimientoMapaScript;
    public TextMeshProUGUI distanciaTexto;
    public TextMeshProUGUI coinsTexto;

    public GameObject defaultCharacterPrefab;
    public GameObject grannyCharacterPrefab;
    public GameObject bigVegasCharacterPrefab;

    public List<Logro> logros = new List<Logro>();

    private bool jugadorVivo;
    private GameObject activeCharacter;
    private bool duplicarMonedas = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
      
        defaultCharacterPrefab.SetActive(false);
        grannyCharacterPrefab.SetActive(false);
        bigVegasCharacterPrefab.SetActive(false);

        
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "default");
        Debug.Log("Selected Character: " + selectedCharacter);

  
        switch (selectedCharacter)
        {
            case "default":
                defaultCharacterPrefab.SetActive(true);
                activeCharacter = defaultCharacterPrefab;
                break;
            case "Granny":
                grannyCharacterPrefab.SetActive(true);
                activeCharacter = grannyCharacterPrefab;
                break;
            case "Big Vegas":
                bigVegasCharacterPrefab.SetActive(true);
                activeCharacter = bigVegasCharacterPrefab;
                duplicarMonedas = true;
                break;
            default:
                defaultCharacterPrefab.SetActive(true);
                activeCharacter = defaultCharacterPrefab;
                break;
        }

        
        if (activeCharacter != null)
        {
            activeCharacter.transform.SetParent(transform);
        }

       
       
        ActualizarCoinsTexto();
    }

    private void Update()
    {
        if (jugadorVivo)
        {
            distanciaRecorrida += velocidad * Time.deltaTime;
            ActualizarDistanciaTexto(distanciaRecorrida);
            VerificarLogros();
        }
    }

    public void IniciarContador()
    {
        distanciaRecorrida = 0f;
        jugadorVivo = true;
    }

    public void DetenerContador()
    {
        if (jugadorVivo)
        {
            jugadorVivo = false;
            ActualizarDistanciaEnBD(distanciaRecorrida);
        }
    }

    private void ActualizarDistanciaEnBD(float distancia)
    {
        if (SessionManager.Instance != null && SessionManager.Instance.CurrentUser != null)
        {
            SessionManager.Instance.ActualizarRecordDistancia(distancia);
        }
    }

    private void ActualizarDistanciaTexto(float distancia)
    {
        if (distanciaTexto != null)
        {
            distanciaTexto.text = distancia.ToString("F2") + " m";
        }
    }

    public void IncrementarContador()
    {
      
    }

    public void AddCoins(int amount)
    {
        if (SessionManager.Instance != null)
        {
            if (duplicarMonedas)
            {
                amount *= 2;
            }
            SessionManager.Instance.AddCoins(amount);
            ActualizarCoinsTexto();
        }
    }

    public void ActualizarCoinsTexto()
    {
        if (SessionManager.Instance != null && SessionManager.Instance.CurrentUser != null && coinsTexto != null)
        {
            coinsTexto.text = SessionManager.Instance.CurrentUser.monedas.ToString();
        }
    }

    public void JugadorMuerto()
    {
        DetenerContador();
        movimientoMapaScript.DetenerMovimiento();
        StartCoroutine(CargarScoreBoardConRetraso(3f));
    }

    private IEnumerator CargarScoreBoardConRetraso(float retraso)
    {
        yield return new WaitForSeconds(retraso);
        ResetGame();
    }

    public void ResetGame()
    {
        
        Destroy(GameObject.Find("Canvas"));
        Destroy(GameObject.Find("mapeado"));
        SceneManager.LoadScene("Lobby");
    }

    private void VerificarLogros()
    {
        foreach (var logro in logros)
        {
            if (!logro.desbloqueado && distanciaRecorrida >= logro.progresoRequerido)
            {
                DesbloquearLogro(logro);
            }
        }
    }

    private void DesbloquearLogro(Logro logro)
    {
        logro.desbloqueado = true;
        SessionManager.Instance.GuardarLogro(logro);
        Debug.Log("Logro desbloqueado: " + logro.nombre);
    }
}
