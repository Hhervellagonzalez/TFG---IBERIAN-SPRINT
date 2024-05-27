using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Collections.Generic;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    public User CurrentUser { get; private set; }

    private DatabaseReference dbReference;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
        CargarLogros();
    }

    public void AddCoins(int amount)
    {
        if (CurrentUser != null)
        {
            CurrentUser.monedas += amount;
            UpdateCoinsInDatabase(CurrentUser.username, CurrentUser.monedas);
        }
    }

    private void UpdateCoinsInDatabase(string username, int monedas)
    {
        dbReference.Child("usuarios").Child(username).Child("monedas").SetValueAsync(monedas);
    }

    public void ActualizarRecordDistancia(float distancia)
    {
        if (CurrentUser != null && distancia > CurrentUser.recordDistancia)
        {
            int distanciaRedondeada = Mathf.FloorToInt(distancia);
            CurrentUser.recordDistancia = distanciaRedondeada;
            dbReference.Child("usuarios").Child(CurrentUser.username).Child("recordDistancia").SetValueAsync(distanciaRedondeada);
        }
    }

    public void GuardarLogro(Logro logro)
    {
        if (CurrentUser != null)
        {
            dbReference.Child("usuarios").Child(CurrentUser.username).Child("logros").Child(logro.nombre).SetValueAsync(logro.desbloqueado);
        }
    }

    private void CargarLogros()
    {
        if (CurrentUser != null)
        {
            dbReference.Child("usuarios").Child(CurrentUser.username).Child("logros").GetValueAsync().ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot logroSnapshot in snapshot.Children)
                    {
                        string nombreLogro = logroSnapshot.Key;
                        bool desbloqueado = (bool)logroSnapshot.Value;

                        Logro logro = GameManager.Instance.logros.Find(l => l.nombre == nombreLogro);
                        if (logro != null)
                        {
                            logro.desbloqueado = desbloqueado;
                        }
                    }
                }
            });
        }
    }

    private void Start()
    {
        if (CurrentUser != null)
        {
            string username = CurrentUser.username;
            var userReference = dbReference.Child("usuarios").Child(username);
            userReference.GetValueAsync().ContinueWith(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        int monedas = int.Parse(snapshot.Child("monedas").Value.ToString());
                        CurrentUser.monedas = monedas;
                        GameManager.Instance.ActualizarCoinsTexto();
                    }
                }
            });
        }
    }
}
