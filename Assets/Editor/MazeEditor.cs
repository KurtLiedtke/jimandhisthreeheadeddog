
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class MazeEditor : Editor
{
    private const int Size = 11;
    private const float CellSize = 26f;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty grid = serializedObject.FindProperty("wallGrid");

        // Ensure the array always contains 121 elements.
        if (grid.arraySize != Size * Size)
        {
            grid.arraySize = Size * Size;
        }

        EditorGUILayout.LabelField(
            "Maze Wall Editor",
            EditorStyles.boldLabel
        );

        EditorGUILayout.HelpBox("Click a space between cells to toggle a wall. " +
            "Green squares are Jim's walkable cells. " +
            "Gray squares are intersections.",
            MessageType.Info
        );

        for (int y = Size - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < Size; x++)
            {
                int index = y * Size + x;
                SerializedProperty cell = grid.GetArrayElementAtIndex(index);

                bool isWallPosition = (x + y) % 2 == 1;

                // Color oldColor = GUI.backgroundColor;

                if (isWallPosition)
                {
                    bool hasWall = cell.intValue == 1;

                    GUI.backgroundColor = hasWall ? new Color(0.35f, 0.38f, 0.43f): new Color(0.85f, 0.85f, 0.85f);

                    if (GUILayout.Button(hasWall ? "W" : "",
                        GUILayout.Width(CellSize),
                        GUILayout.Height(CellSize)))
                    {
                        cell.intValue = hasWall ? 0 : 1;
                    }
                }
                else
                {
                    bool isCell = x % 2 == 0 && y % 2 == 0;

                    switch (cell.intValue)
                    {
                        case 0:
                            GUI.backgroundColor = isCell ? new Color(0.65f, 0.85f, 0.72f) : new Color(0.55f, 0.55f, 0.55f);
                        break;
                        case 2:
                            GUI.backgroundColor = new Color(0.65f, 0.1f, 0.1f);
                        break;
                        case 3:
                            GUI.backgroundColor = new Color(0.1f, 0.65f, 0.55f);
                        break;
                    }
                    // GUI.enabled = false;

                    if(GUILayout.Button("",
                        GUILayout.Width(CellSize),
                        GUILayout.Height(CellSize)
                    ))
                    {
                        switch (cell.intValue)
                        {
                            case 0:
                                Debug.Log("case 0");
                                cell.intValue = 2;
                            break;
                            case 2:
                                cell.intValue = 3;
                            break;
                            case 3:
                                cell.intValue = 0;
                            break;
                        }
                    }

                    GUI.enabled = true;
                }

                // GUI.backgroundColor = oldColor;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear All Walls"))
        {
            for (int i = 0; i < grid.arraySize; i++)
            {
                grid.GetArrayElementAtIndex(i).intValue = 0;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}

