using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField campoTextoUsuario;
    public TMP_InputField campoTextoContraseña;
    public TextMeshProUGUI textoFeedback;

    private DatabaseReference referenciaBaseDatos;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                referenciaBaseDatos = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Error al inicializar Firebase: " + task.Exception);
            }
        });
    }

    public void IniciarSesion()
    {
        string usuario = campoTextoUsuario.text;
        string contraseña = campoTextoContraseña.text;

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
        {
            textoFeedback.text = "Por favor, introduce un usuario y contraseña válidos.";
            return;
        }

        if (usuario == "usuarioValido" && contraseña == "contraseñaValida")
        {
            SceneManager.LoadScene("JUEGO");
        }
        else
        {
            textoFeedback.text = "Usuario o contraseña incorrectos. Por favor, inténtalo de nuevo.";
        }
    }
}
