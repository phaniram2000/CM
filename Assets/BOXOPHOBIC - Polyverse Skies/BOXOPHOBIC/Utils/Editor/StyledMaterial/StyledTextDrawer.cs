﻿// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using System;

namespace Boxophobic.StyledGUI
{
    public class StyledTextDrawer : MaterialPropertyDrawer
    {
        public string text = "";
        public string alignment = "Center";
        public string font = "Normal";
        public string disabled = "";
        public float size = 11;
        public float top = 0;
        public float down = 0;

        public StyledTextDrawer(string text)
        {
            this.text = text;
        }

        public StyledTextDrawer(string text, string alignment, string font, string disabled, float size)
        {
            this.text = text;
            this.alignment = alignment;
            this.font = font;
            this.disabled = disabled;
            this.size = size;
        }

        public StyledTextDrawer(string text, string alignment, string font, string disabled, float size, float top,
            float down)
        {
            this.text = text;
            this.alignment = alignment;
            this.font = font;
            this.disabled = disabled;
            this.size = size;
            this.top = top;
            this.down = down;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor materialEditor)
        {
            //Material material = materialEditor.target as Material;

            GUIStyle styleLabel = new GUIStyle(EditorStyles.label)
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };

            GUILayout.Space(top);

            if (alignment == "Center")
            {
                styleLabel.alignment = TextAnchor.MiddleCenter;
            }
            else if (alignment == "Left")
            {
                styleLabel.alignment = TextAnchor.MiddleCenter;
            }
            else if (alignment == "Left")
            {
                styleLabel.alignment = TextAnchor.MiddleCenter;
            }

            if (font == "Bold")
            {
                styleLabel.fontStyle = FontStyle.Bold;
            }
            else
            {
                styleLabel.fontStyle = FontStyle.Normal;
            }

            styleLabel.fontSize = (int)size;

            if (disabled == "Disabled")
            {
                GUI.enabled = false;
            }

            GUILayout.Label(text, styleLabel);

            GUI.enabled = true;

            GUILayout.Space(down);
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            return -2;
        }
    }
}