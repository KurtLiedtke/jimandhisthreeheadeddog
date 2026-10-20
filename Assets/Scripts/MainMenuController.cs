
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button confirmJoinButton;
    [SerializeField] private Button backButton;

    [Header("Join Panel")]
    [SerializeField] private GameObject joinPanel;
    [SerializeField] private TMP_InputField roomCodeInput;

    [Header("Status")]
    [SerializeField] private TMP_Text statusText;

    public static ISession currentSession {get; private set;}
    private bool isBusy;

    private async void Start()
    {
        joinPanel.SetActive(false);

        try
        {
            await InitializeServices();
            statusText.text = "Create or Join to Start";
        }
        catch (Exception e)
        {
            statusText.text = "Initialization failed: " + e.Message;
            Debug.LogException(e);
        }
    }

    private void Awake()
    {
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(ShowJoinPanel);
        confirmJoinButton.onClick.AddListener(JoinGame);
        backButton.onClick.AddListener(HideJoinPanel);
    }

    private async Task InitializeServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void ShowJoinPanel()
    {
        joinPanel.SetActive(true);
        statusText.text = "Enter your room code.";
    }

    private void HideJoinPanel()
    {
        joinPanel.SetActive(false);
        statusText.text = "Ready to play!";
    }

    private async void HostGame()
    {
        if (isBusy) return;
        isBusy = true;
        SetButtons(false);
        statusText.text = "Creating room...";

        try
        {
            await InitializeServices();

            var options = new SessionOptions
            {
                MaxPlayers = 4
            }.WithRelayNetwork();

            var session = await MultiplayerService.Instance.CreateSessionAsync(options);
            currentSession = session;
            Debug.Log("Room code: " + session.Code);
            PlayerPrefs.SetString("RoomCode", session.Code);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Lobby");
        }
        catch (Exception e)
        {
            statusText.text = "Couldn't create room: " + e.Message;
            Debug.LogException(e);
            isBusy = false;
            SetButtons(true);
        }
    }

    private async void JoinGame()
    {
        if (isBusy) return;

        string code = roomCodeInput.text.Trim();

        if (string.IsNullOrEmpty(code))
        {
            statusText.text = "Please enter a room code.";
            return;
        }

        isBusy = true;
        SetButtons(false);
        statusText.text = "Joining room...";

        try
        {
            await InitializeServices();

            currentSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(code);

            PlayerPrefs.SetString("RoomCode", code);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Lobby");
        }
        catch (Exception e)
        {
            statusText.text = "Couldn't join room: " + e.Message;
            Debug.LogException(e);
            isBusy = false;
            SetButtons(true);
        }
    }

    private void SetButtons(bool interactable)
    {
        hostButton.interactable = interactable;
        joinButton.interactable = interactable;
        confirmJoinButton.interactable = interactable;
        backButton.interactable = interactable;
    }
}
