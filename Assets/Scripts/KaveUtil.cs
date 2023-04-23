using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KaveUtil
{
    public static class Util
    {
        //-------------------------------------------------------
        public static Vector2 Rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }
        //-------------------------------------------------------
        public static void DrawTextInSceneView(Vector3 worldPos, string text, Color? colour = null)
        {
            UnityEditor.Handles.BeginGUI();
            {
                if (colour.HasValue) GUI.color = colour.Value;
                SceneView view = UnityEditor.SceneView.currentDrawingSceneView;
                Vector3 viewPos = view.camera.WorldToScreenPoint(worldPos);
                viewPos.y += 24; // Value changed
                Vector2 screenPos = new Vector2(viewPos.x, -viewPos.y + view.position.height);
                GUI.Box(new Rect(screenPos, new Vector2(32, 32)), text);
            }
            UnityEditor.Handles.EndGUI();
        }
    }//class Util
    class CircularQueue<T>
    {
        private T[] element;
        private int front;
        private int rear;
        private int max;
        private int count;

        //-------------------------------------------------------
        public CircularQueue(int size)
        {
            element = new T[size];
            front = 0;
            // rear = -1 // not good
            rear = 0; // use half closed interval
            max = size;
            count = 0;
        }
        //-------------------------------------------------------
        public int GetCount()
        {
            return count;
        }
        //-------------------------------------------------------
        public bool Insert(T item, bool bAutoDelete = true)
        {
            if(count == max)
            {
                if (bAutoDelete == false)
                    return false;

                Delete();
            }

            element[rear] = item;
            rear = (rear + 1) % max;
            count++;
            return true;
        }
        //-------------------------------------------------------
        public bool Delete()
        {
            if (count == 0)
            {
                return false; // empty error
            }

            front = (front + 1) % max;
            count--;
            return true;
        }
        //-------------------------------------------------------
        public bool GetFront(out T e)
        {
            if(count <= 0)
            {
                e = default(T);
                return false;
            }
            e = element[front];
            return true;
        }
        //-------------------------------------------------------
        public bool GetRear(out T e)
        {
            if (count <= 0)
            {
                e = default(T);
                return false;
            }
            e = element[rear];
            return true;
        }
    }// class CircularQueue<T>


}
