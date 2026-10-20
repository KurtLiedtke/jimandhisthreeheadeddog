using System;
using UnityEngine;

public class JimMovement : MonoBehaviour
{
    [SerializeField] private JimInputActions jimInput;
    [SerializeField] private GridManager grid;
    [SerializeField] private Vector2Int startPos;
    [SerializeField] private float jimSpeed = 1.4f;
    private Vector2Int jimPos;
    public EventHandler OnJimMove;
    public bool hitWall = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jimInput.OnDirectionChosen += HandleDirectionChosen;
        jimPos = Vector2Int.zero;
        transform.position = (Vector3Int) startPos;
    }

    private void HandleDirectionChosen(Vector2Int dir)
    {
        if(!grid.IsWallBetween(jimPos, dir))
        {
            hitWall = false;
            transform.position += new Vector3(jimSpeed * dir.x, jimSpeed * dir.y); 
            jimPos = new Vector2Int(jimPos.x + 2 * dir.x, jimPos.y + 2 * dir.y);
            Debug.Log(jimPos);
            OnJimMove?.Invoke(this, EventArgs.Empty);

        }
        else {
            hitWall = true;
            OnJimMove?.Invoke(this, EventArgs.Empty);
        }

    }

    public Vector2Int GetJimPos()
    {
        return jimPos;
    }
}
