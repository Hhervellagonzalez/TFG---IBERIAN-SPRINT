using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject defaultCharacterLobby;
    public GameObject grannyCharacterLobby;
    public GameObject bigVegasCharacterLobby;

    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "default");

        defaultCharacterLobby.SetActive(false);
        grannyCharacterLobby.SetActive(false);
        bigVegasCharacterLobby.SetActive(false);

        switch (selectedCharacter)
        {
            case "default":
                Debug.Log("Activating Default Character");
                defaultCharacterLobby.SetActive(true);
                break;
            case "Granny":
                Debug.Log("Activating Granny Character");
                grannyCharacterLobby.SetActive(true);
                break;
            case "Big Vegas":
                Debug.Log("Activating Big Vegas Character");
                bigVegasCharacterLobby.SetActive(true);
                break;
            default:
                Debug.Log("Activating Default Character by default");
                defaultCharacterLobby.SetActive(true);
                break;
        }
    }
}