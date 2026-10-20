
using UnityEngine;

public class RoleCameraSetup : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera jimCamera;
    [SerializeField] private Camera timofyCamera;
    [SerializeField] private Camera lacyCamera;
    [SerializeField] private Camera dougingtonCamera;
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        string role = PlayerPrefs.GetString("SelectedRole", "");
        canvas.SetActive(false);
        // Disable all four cameras first.
        jimCamera.enabled = false;
        timofyCamera.enabled = false;
        lacyCamera.enabled = false;
        dougingtonCamera.enabled = false;

        // Enable the camera for this player's role.
        switch (role)
        {
            case "Jim":
                canvas.SetActive(true);
                jimCamera.enabled = true;
            break;
            case "Timofy":
                timofyCamera.enabled = true;
            break;
            case "Lacy":
                lacyCamera.enabled = true;
            break;
            case "Dougington":
                dougingtonCamera.enabled = true;
            break;

            default:
                Debug.LogError("No valid player role saved: " + role);
            break;
        }
    }
}
