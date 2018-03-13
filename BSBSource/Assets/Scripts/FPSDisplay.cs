﻿using UnityEngine;
using System.Collections;
using Assets;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        FPS();
        Stamina();
    }

    void FPS()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 50);
        style.alignment = TextAnchor.LowerRight;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    void Stamina()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 50);
        style.alignment = TextAnchor.LowerCenter;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        string text = GameStats.Stamina.ToString("0.00");
        GUI.Label(rect, text, style);
    }
}