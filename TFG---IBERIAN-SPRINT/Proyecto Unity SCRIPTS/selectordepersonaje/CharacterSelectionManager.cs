using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    public Button selectButton1;
    public Button selectButton2;
    public Button selectButton3;

    public TMP_Text statusText1;
    public TMP_Text statusText2;
    public TMP_Text statusText3;

    private string selectedCharacter;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            VerificarEstadoBotones();
        });
    }

    public void SeleccionarPersonaje1()
    {
        StartCoroutine(SeleccionarPersonaje("default", statusText1));
    }

    public void SeleccionarPersonaje2()
    {
        StartCoroutine(SeleccionarPersonaje("Granny", statusText2));
    }

    public void SeleccionarPersonaje3()
    {
        StartCoroutine(SeleccionarPersonaje("Big Vegas", statusText3));
    }

    private IEnumerator SeleccionarPersonaje(string character, TMP_Text statusText)
    {
        if (SessionManager.Instance.CurrentUser == null)
        {
            Debug.LogError("CurrentUser is null.");
            yield break;
        }

        string username = SessionManager.Instance.CurrentUser.username;
        var userReference = dbReference.Child("usuarios").Child(username);
        var getUserTask = userReference.GetValueAsync();
        yield return new WaitUntil(() => getUserTask.IsCompleted);

        if (getUserTask.Exception != null)
        {
            Debug.LogError("Error accessing the database: " + getUserTask.Exception.Message);
            yield break;
        }

        DataSnapshot snapshot = getUserTask.Result;

        if (!snapshot.Exists || snapshot.Child("aspectos") == null)
        {
            Debug.LogError("Snapshot or specific fields do not exist.");
            yield break;
        }

        var aspects = new List<string>();
        foreach (var aspect in snapshot.Child("aspectos").Children)
        {
            aspects.Add(aspect.Value.ToString());
        }

        if (aspects.Contains(character))
        {
            selectedCharacter = character;
            statusText.text = "Character selected!";
            SaveSelectedCharacter();
        }
        else
        {
            statusText.text = "Character not purchased.";
            Debug.Log("Character not purchased.");
        }
    }

    private void VerificarEstadoBotones()
    {
        if (SessionManager.Instance.CurrentUser == null)
        {
            Debug.LogError("CurrentUser is null.");
            return;
        }

        string username = SessionManager.Instance.CurrentUser.username;
        var userReference = dbReference.Child("usuarios").Child(username);
        userReference.GetValueAsync().ContinueWith(task => {
            if (task.Exception != null)
            {
                Debug.LogError("Error accessing the database: " + task.Exception.Message);
                return;
            }

            DataSnapshot snapshot = task.Result;
            var aspects = new List<string>();
            if (snapshot.Exists && snapshot.Child("aspectos") != null)
            {
                foreach (var aspect in snapshot.Child("aspectos").Children)
                {
                    aspects.Add(aspect.Value.ToString());
                }

               
                if (aspects.Contains("default"))
                {
                    selectButton1.gameObject.SetActive(true);
                }
                else
                {
                    selectButton1.gameObject.SetActive(false);
                    statusText1.text = "Character not purchased.";
                }

                if (aspects.Contains("Granny"))
                {
                    selectButton2.gameObject.SetActive(true);
                }
                else
                {
                    selectButton2.gameObject.SetActive(false);
                    statusText2.text = "Character not purchased.";
                }

                if (aspects.Contains("Big Vegas"))
                {
                    selectButton3.gameObject.SetActive(true);
                }
                else
                {
                    selectButton3.gameObject.SetActive(false);
                    statusText3.text = "Character not purchased.";
                }
            }
            else
            {
                Debug.LogError("Snapshot or aspects field does not exist.");
            }
        });
    }

    private void SaveSelectedCharacter()
    {
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
