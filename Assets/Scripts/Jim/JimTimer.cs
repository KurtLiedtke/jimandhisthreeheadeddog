using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JimTimer : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI dogTimer;
    [SerializeField] private TextMeshProUGUI jimTimer;
    [SerializeField] private JimInputActions jimInput;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private TextMeshProUGUI displayText;


    float jimTimerMax = 15;
    float jimTime = 0;

    float jimMoveTimeMax = 5;
    float jimMoveTime = 0;
    bool isMoveTime = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jimInput.Disable();
        continueButton.onClick.AddListener(() =>
        {

            jimInput.Disable();
            jimTime = 0;
            jimMoveTime = 0;
            isMoveTime = false;
            pauseManager.isPaused = false;
            jimTimer.SetText(jimMoveTime.ToString("0"));
            continueButton.gameObject.SetActive(false);
        });
        continueButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.isPaused) return;

        if (isMoveTime)
        {
            jimMoveTime += Time.deltaTime;
            jimTimer.SetText(jimMoveTime.ToString("0"));
            if(jimMoveTime >= jimMoveTimeMax)
            {
                jimInput.Disable();
                pauseManager.isPaused = true;
                continueButton.gameObject.SetActive(true);
                displayText.SetText("Jim you have to mave\nJim draw a Punishment");
            }
            return;
        }

        jimTime += Time.deltaTime;
        dogTimer.SetText(jimTime.ToString("0"));
        if(jimTime >= jimTimerMax)
        {
            isMoveTime = true;
            jimInput.Enable();
        }
    }

    public void ShowButton()
    {
        continueButton.gameObject.SetActive(true);
    }

    public void ResetTimer()
    {
        jimInput.Disable();
        jimTime = 0;
        jimMoveTime = 0;
        isMoveTime = false;
        pauseManager.isPaused = false;
        jimTimer.SetText(jimMoveTime.ToString("0"));
        continueButton.gameObject.SetActive(false);
    }

    public void Enable()
    {
        jimInput.Enable();
    }

    public void Disable()
    {
        jimInput.Disable();
    }
}
