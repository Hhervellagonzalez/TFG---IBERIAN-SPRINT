using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardManager : MonoBehaviour
{
    public GameObject topRowPrefab; 
    public GameObject regularRowPrefab; 
    public Transform rowParent; 

    private DatabaseReference dbReference;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                GetTopScores();
            }
            else
            {
                Debug.LogError("No se pudieron resolver todas las dependencias de Firebase: " + task.Result);
            }
        });
    }

    void GetTopScores()
    {
        dbReference.Child("usuarios").OrderByChild("recordDistancia").LimitToLast(25).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al recuperar datos: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<User> topUsers = new List<User>();

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    User user = JsonUtility.FromJson<User>(userSnapshot.GetRawJsonValue());
                    topUsers.Add(user);
                }

                
                topUsers.Sort((x, y) => y.recordDistancia.CompareTo(x.recordDistancia));

                
                DisplayScores(topUsers);
            }
        });
    }

    void DisplayScores(List<User> topUsers)
    {
       
        foreach (Transform child in rowParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < topUsers.Count; i++)
        {
            GameObject row;
            if (i < 3)
            {
                
                row = Instantiate(topRowPrefab, rowParent);
            }
            else
            {
                
                row = Instantiate(regularRowPrefab, rowParent);
            }

            
            Button[] buttons = row.GetComponentsInChildren<Button>();
            if (buttons.Length >= 3)
            {
                Debug.Log($"Asignando datos para el usuario {topUsers[i].username} en la posición {i + 1}");
                buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString(); 
                buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = topUsers[i].recordDistancia.ToString() + "m"; 
                buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = topUsers[i].username;
            }
            else
            {
                Debug.LogError("No se encontraron suficientes componentes Button en el prefab.");
            }


            row.transform.SetParent(rowParent, false);
            row.transform.SetAsLastSibling(); 
        }
    }
}