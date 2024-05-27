using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Login : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text mensajeError;

    private DatabaseReference dbReference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void IniciarSesion()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            mensajeError.text = "Todos los campos son obligatorios";
            return;
        }

        StartCoroutine(IniciarSesionCoroutine(username, password));
    }

    private IEnumerator IniciarSesionCoroutine(string username, string password)
    {
        var userReference = dbReference.Child("usuarios").Child(username);
        var getUserTask = userReference.GetValueAsync();
        yield return new WaitUntil(() => getUserTask.IsCompleted);

        if (getUserTask.Exception != null)
        {
            mensajeError.text = "Error al acceder a la base de datos: " + getUserTask.Exception.Message;
        }
        else if (!getUserTask.Result.Exists)
        {
            mensajeError.text = "Nombre de usuario no encontrado.";
        }
        else
        {
            DataSnapshot snapshot = getUserTask.Result;
            string dbPassword = snapshot.Child("password").Value?.ToString();

            if (dbPassword == password)
            {
                int desafiosCompletados = int.Parse(snapshot.Child("desafiosCompletados").Value?.ToString() ?? "0");
                int monedas = int.Parse(snapshot.Child("monedas").Value?.ToString() ?? "0");

                List<string> aspectos = new List<string>();
                if (snapshot.Child("aspectos").Exists)
                {
                    foreach (var aspecto in snapshot.Child("aspectos").Children)
                    {
                        aspectos.Add(aspecto.Value.ToString());
                    }
                }

                Dictionary<string, bool> logrosDict = new Dictionary<string, bool>();
                if (snapshot.Child("logros").Exists)
                {
                    foreach (var logro in snapshot.Child("logros").Children)
                    {
                        logrosDict[logro.Key] = (bool)logro.Value;
                    }
                }

                float recordDistancia = float.Parse(snapshot.Child("recordDistancia").Value?.ToString() ?? "0");

                if (!aspectos.Contains("default"))
                {
                    aspectos.Add("default");
                }

                User loggedInUser = new User(
                    username,
                    password,
                    "",
                    monedas,
                    aspectos,
                    recordDistancia,
                    logrosDict
                );

                SessionManager.Instance.SetCurrentUser(loggedInUser);
                mensajeError.text = "Login exitoso!";
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            }
            else
            {
                mensajeError.text = "Contraseña incorrecta.";
            }
        }
    }
}