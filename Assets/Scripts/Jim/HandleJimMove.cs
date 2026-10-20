using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleJimMove : MonoBehaviour
{
    [SerializeField] private JimMovement jimMovement;
    [SerializeField] private JimTimer jimTimer;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private GridManager grid;
    [SerializeField] private TextMeshProUGUI displayText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jimMovement.OnJimMove += Jim_OnJimMovement;
        
    }

    private void Jim_OnJimMovement(object sender, System.EventArgs e)
    {
        if (!jimMovement.hitWall)
        {
            //find spot he is on and do something
            int x = grid.TileType(jimMovement.GetJimPos().x, jimMovement.GetJimPos().y);
            //if nothing reset round timer
            if (x == 0)
            {
                Debug.Log("Stepped on normal tile : Reset");
                displayText.SetText("Jim is good");
                jimTimer.ResetTimer();
            }
            //if trap, pause game, show coninue button, continue button should reset round
            if (x == 2)
            {
                displayText.SetText("Jim trapped\nEverybody draw a Punishment");
                PauseForReset();
            }
            //if objective collect the objective, reset round
            if (x == 3)
            {
                displayText.SetText("Wow Jim found an Objective\n");
                grid.SetTile(jimMovement.GetJimPos().x, jimMovement.GetJimPos().y,0);
                jimTimer.ResetTimer();
            }
        }
        else
        {
            displayText.SetText("Jim hit wall\nJim draw a Punishment");
            PauseForReset();
            jimMovement.hitWall = false;
            //jim hit a wall, pause game, show continue button
        }
    }

    private void PauseForReset()
    {
        pauseManager.isPaused = true;
        jimTimer.Disable();
        jimTimer.ShowButton();
    }

    
}
