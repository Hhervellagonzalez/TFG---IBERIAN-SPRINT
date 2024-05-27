using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class Registro : MonoBehaviour
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

    public void RegistrarUsuario()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            mensajeError.text = "Todos los campos son obligatorios";
            mensajeError.color = Color.red;
            return;
        }

        if (!EsUsernameValido(username))
        {
            mensajeError.text = "El nombre de usuario debe tener entre 5 y 20 caracteres y no contener símbolos.";
            mensajeError.color = Color.red;
            return;
        }

        if (!EsPasswordValido(password))
        {
            mensajeError.text = "La contraseña debe tener entre 5 y 20 caracteres y no contener símbolos.";
            mensajeError.color = Color.red;
            return;
        }

        StartCoroutine(RegistrarCoroutine(username, password));
    }

    private bool EsUsernameValido(string username)
    {
        return Regex.IsMatch(username, "^[a-zA-Z0-9]{5,20}$");
    }

    private bool EsPasswordValido(string password)
    {
        return Regex.IsMatch(password, "^[a-zA-Z0-9]{5,20}$");
    }

    private IEnumerator RegistrarCoroutine(string username, string password)
    {
        var userReference = dbReference.Child("usuarios").Child(username);
        var getUserTask = userReference.GetValueAsync();
        yield return new WaitUntil(() => getUserTask.IsCompleted);

        if (getUserTask.Exception != null)
        {
            mensajeError.text = "Error al acceder a la base de datos: " + getUserTask.Exception.Message;
            mensajeError.color = Color.red;
        }
        else if (getUserTask.Result.Exists)
        {
            mensajeError.text = "El nombre de usuario ya existe.";
            mensajeError.color = Color.red;
        }
        else
        {
            List<string> aspectos = new List<string> { "default" };
            Dictionary<string, bool> logros = new Dictionary<string, bool>();

            User newUser = new User(username, password, "", 0, aspectos, 0f, logros);
            string json = JsonUtility.ToJson(newUser);
            var dbTask = userReference.SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => dbTask.IsCompleted);

            if (dbTask.Exception != null)
            {
                mensajeError.text = "Error al registrar: " + dbTask.Exception.Message;
                mensajeError.color = Color.red;
            }
            else
            {
                mensajeError.text = "Registro exitoso!";
                mensajeError.color = Color.green;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
            }
        }
    }
}