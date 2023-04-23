using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    static void DrawTextInSceneView(Vector3 worldPos, string text, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 pos = screenPos;
        pos.y = -screenPos.y + view.position.height - 36;
        GUI.Box(new Rect(pos, new Vector2(100, 32)), "hello");
        UnityEditor.Handles.EndGUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        // draw state
        //
        string text = string.Format("{0}", 11);
        DrawTextInSceneView(transform.position, text, Color.white);
    }
}
