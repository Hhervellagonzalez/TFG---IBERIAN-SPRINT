using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private DatabaseReference dbReference;

    public Button button1;
    public Button button2;
    public TMP_Text statusText1;  
    public TMP_Text statusText2;  

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            VerificarEstadoBotones();
        });
    }

    public void ComprarPersonaje1()
    {
        StartCoroutine(ComprarPersonaje("Granny", 5000, button1, statusText1));
    }

    public void ComprarPersonaje2()
    {
        StartCoroutine(ComprarPersonaje("Big Vegas", 1000, button2, statusText2));
    }

    private IEnumerator ComprarPersonaje(string character, int cost, Button button, TMP_Text statusText)
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

        if (!snapshot.Exists || snapshot.Child("monedas") == null || snapshot.Child("aspectos") == null)
        {
            Debug.LogError("Snapshot or specific fields do not exist.");
            yield break;
        }

        int coins = int.Parse(snapshot.Child("monedas").Value.ToString());

        if (coins >= cost)
        {
            coins -= cost;
            var aspects = new List<string>();
            foreach (var aspect in snapshot.Child("aspectos").Children)
            {
                aspects.Add(aspect.Value.ToString());
            }
            aspects.Add(character);

           
            userReference.Child("monedas").SetValueAsync(coins);
            userReference.Child("aspectos").SetValueAsync(aspects);

          
            SessionManager.Instance.CurrentUser.monedas = coins;
            SessionManager.Instance.CurrentUser.aspectos = aspects;

           
            button.gameObject.SetActive(false);
            statusText.text = "Character purchased!";
        }
        else
        {
            statusText.text = "Not enough coins.";
            Debug.Log("Not enough coins.");
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

                if (aspects.Contains("Granny"))
                {
                    button1.gameObject.SetActive(false);
                    statusText1.text = "Character purchased!";
                }

                if (aspects.Contains("Big Vegas"))
                {
                    button2.gameObject.SetActive(false);
                    statusText2.text = "Character purchased!";
                }
            }
            else
            {
                Debug.LogError("Snapshot or aspects field does not exist.");
            }
        });
    }
}