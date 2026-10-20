using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JimTimer : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI dogTimer;
    [SerializeField] private TextMeshProUGUI jimTimer;

    float jimTimerMax = 15;
    float jimTime = 0;

    float jimMoveTimeMax = 5;
    float jimMoveTime = 0;
    bool isMoveTime = false;
    bool pause = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            jimTime = 0;
            jimMoveTime = 0;
            isMoveTime = false;
            pause = false;
            jimTimer.SetText(jimMoveTime.ToString("0"));
            continueButton.gameObject.SetActive(false);
        });
        continueButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pause) return;
        if (isMoveTime)
        {
            jimMoveTime += Time.deltaTime;
            jimTimer.SetText(jimMoveTime.ToString("0"));
            if(jimMoveTime >= jimMoveTimeMax)
            {
                pause = true;
                continueButton.gameObject.SetActive(true);
            }
            return;
        }
        jimTime += Time.deltaTime;
        dogTimer.SetText(jimTime.ToString("0"));
        if(jimTime >= jimTimerMax)
        {
            isMoveTime = true;
        }
    }
}
