using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class JimInputActions : MonoBehaviour
{
    private InputActions inputActions;

    public event Action<Vector2Int> OnDirectionChosen;
    void Awake()
    {
        inputActions = new InputActions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions.Jim.Enable();

        inputActions.Jim.MoveUp.performed += _ => ChooseDirection(Vector2Int.up);
        inputActions.Jim.MoveDown.performed += _ => ChooseDirection(Vector2Int.down);
        inputActions.Jim.MoveLeft.performed += _ => ChooseDirection(Vector2Int.left);
        inputActions.Jim.MoveRight.performed += _ => ChooseDirection(Vector2Int.right);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseDirection(Vector2Int dir)
    {
        OnDirectionChosen?.Invoke(dir);
    }
}
