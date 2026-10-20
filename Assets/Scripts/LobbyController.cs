
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Multiplayer;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    [Header("Room Code")]
    [SerializeField] private TMPro.TMP_Text roomCodeText;

    [Header("Role Buttons")]
    [SerializeField] private Button jim;
    [SerializeField] private Button timofy;
    [SerializeField] private Button lacy;
    [SerializeField] private Button dougington;
    [SerializeField] private Button start;

    [Header("Colors")]
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color takenColor =
        new Color(0.55f, 0.75f, 0.55f);
    [SerializeField] private Color selectedColor =
        new Color(0.85f, 0.85f, 0.35f);

    private Button[] roleButtons;
    private string[] roleNames = { "Jim", "Timofy", "Lacy", "Dougington" };

    private int selectedRole = -1;
    private ISession session;
    private bool savingRole;

    private async void Start()
    {
        string roomCode = PlayerPrefs.GetString("RoomCode", "");
        roomCodeText.text = string.IsNullOrEmpty(roomCode) ? "Room Code: unavailable" : "Room Code: " + roomCode;
        roleButtons = new Button[]
        {
            jim, timofy, lacy, dougington
        };

        for (int i = 0; i < roleButtons.Length; i++)
        {
            int role = i;
            roleButtons[i].onClick.AddListener(
                () => ChooseRole(role));
        }

        start.onClick.AddListener(StartGame);
        start.interactable = false;

        session = MainMenuController.currentSession;

        if (session == null)
        {
            Debug.LogError("No active multiplayer session found.");
            return;
        }

        session.PlayerPropertiesChanged += RefreshLobby;
        RefreshLobby();

        // Restore this player's saved selection if they have one.
        var properties = session.CurrentPlayer.Properties;
        if (properties.TryGetValue("role", out var savedRole))
        {
            selectedRole = Array.IndexOf(roleNames, savedRole.Value);
        }

        RefreshLobby();
        await Task.CompletedTask;
    }

    private void OnDestroy()
    {
        if (session != null)
            session.PlayerPropertiesChanged -= RefreshLobby;
    }

    private async void ChooseRole(int role)
    {
        if (savingRole || session == null) return;

        savingRole = true;

        try
        {
            selectedRole = role;

            session.CurrentPlayer.SetProperty("role", new PlayerProperty(roleNames[role]));

            await session.SaveCurrentPlayerDataAsync();
            RefreshLobby();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            savingRole = false;
        }
        PlayerPrefs.SetString("SelectedRole", roleNames[role]);
        PlayerPrefs.Save();
    }

    private void RefreshLobby()
    {
        if (roleButtons == null || session == null) return;

        var claimedRoles = session.Players
            .Where(p => p.Properties.ContainsKey("role"))
            .Select(p => p.Properties["role"].Value)
            .ToList();

        for (int i = 0; i < roleButtons.Length; i++)
        {
            bool isMine = i == selectedRole;
            bool isTaken = claimedRoles.Contains(roleNames[i]);

            var colors = roleButtons[i].colors;
            colors.normalColor = isMine
                ? selectedColor
                : isTaken ? takenColor : availableColor;
            roleButtons[i].colors = colors;
        }

        start.interactable = session.IsHost; //&& roleNames.All(claimedRoles.Contains);
    }

    private void StartGame()
    {
        if (session == null || !session.IsHost)
            return;

        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null || !networkManager.IsServer)
        {
            Debug.LogError("The host's NetworkManager is not running.");
            return;
        }

        var status = networkManager.SceneManager.LoadScene("Level_1",LoadSceneMode.Single);

        Debug.Log("Scene load status: " + status);
    }
}
