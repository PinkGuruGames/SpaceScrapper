// Better Hierarchy Plugin
// Author: Wokarol
// Code is free to use and modify

﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Wokarol.EditorExtensions
{
    [InitializeOnLoad]
    public static class BetterHierarchy
    {
        private const string toggleStyleName = "OL Toggle";
        private const string mixedToggleStyleName = "OL ToggleMixed";

        private static bool includeNotImportant = true;
        private const string includeNotImportantPrefsKey = "{E0EF3D35-59F0-4531-8040-7341E3093C84}";

        // ===============================================================================================

        private static readonly Dictionary<Type, string> iconOverrides = new Dictionary<Type, string>
        {
        };

        private static readonly HashSet<Type> importantList = new HashSet<Type>
        {
            typeof(Camera),
            typeof(Rigidbody2D),
            typeof(Rigidbody),
            typeof(TMPro.TMP_Text),
            typeof(Collider),
            typeof(Collider2D),
            typeof(Renderer),
            typeof(CanvasRenderer)
        };

        private static readonly HashSet<Type> blacklist = new HashSet<Type>
        {
            typeof(Transform),
            typeof(RectTransform)
        };


        // ===============================================================================================

        static BetterHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI = DrawItem;
            includeNotImportant = EditorPrefs.GetBool(includeNotImportantPrefsKey);
        }

        [MenuItem("Tools/BetterHierarchy/Toggle Non-Important")]
        public static void ToggleNonImportant()
        {
            includeNotImportant = !includeNotImportant;
            EditorPrefs.SetBool(includeNotImportantPrefsKey, includeNotImportant);
            EditorApplication.RepaintHierarchyWindow();
        }

        static void DrawItem(int instanceID, Rect rect)
        {

            // Get's object for given item
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go != null)
            {
                bool isHeader = go.name.StartsWith("---");

                bool shouldHaveActivityToggle = !isHeader || go.transform.childCount > 0;

                DrawComponentIcons(rect, go, out int numberOfIconsDraw);

                if (shouldHaveActivityToggle)
                {
                    DrawActivityToggle(rect, go);
                }
                if (isHeader)
                {
                    DrawHeader(rect, go, shouldHaveActivityToggle, numberOfIconsDraw);
                }
            }
        }

        private static void DrawHeader(Rect rect, GameObject go, bool cutLeft, int componentDrawCut)
        {
            // Creating highlight rect and style
            Rect highlightRect = new Rect(rect);
            highlightRect.width -= highlightRect.height;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize -= 1;
            highlightRect.height -= 1;
            highlightRect.y += 1;

            // Drawing background
            string colorHTML = EditorGUIUtility.isProSkin ? "#2D2D2D" : "#AAAAAA";
            ColorUtility.TryParseHtmlString(colorHTML, out Color headerColor);

            var headerRect = new Rect(highlightRect);
            headerRect.y -= 1;
            headerRect.xMin -= 28;
            headerRect.xMax += 28;

            var fullRect = new Rect(headerRect);

            if (PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.NotAPrefab && componentDrawCut == 0)
            {
                headerRect.xMax -= 10;
            }

            if (componentDrawCut > 0)
            {
                headerRect.xMax -= 16;
                headerRect.xMax -= componentDrawCut * 16;
            }

            if (cutLeft)
            {
                headerRect.xMin += 28;
            }

            EditorGUI.DrawRect(headerRect, headerColor);

            // Offseting text
            highlightRect.height -= 2;

            // Drawing label
            EditorGUI.LabelField(highlightRect, go.name.Replace("---", "").ToUpperInvariant(), labelStyle);
        }

        private static void DrawComponentIcons(Rect rect, GameObject go, out int numberOfIconsDrawn)
        {
            Dictionary<Texture, int> usedIcons = new Dictionary<Texture, int>();
            List<(Texture texture, bool important)> iconsToDraw = new List<(Texture icon, bool important)>();

            foreach (var component in go.GetComponents<Component>())
            {
                if (component == null)
                    continue;

                Type type = component.GetType();

                if (blacklist.Contains(type))
                    continue;

                Texture texture = GetIconFor(component, type);
                bool important = CheckTypeRecursive(type, importantList);

                if (!includeNotImportant && !important)
                    continue;

                if (usedIcons.TryGetValue(texture, out int index))
                {
                    var icon = iconsToDraw[index];
                    icon.important |= important;
                    iconsToDraw[index] = icon;
                }
                else
                {
                    iconsToDraw.Add((texture, important));
                    usedIcons.Add(texture, iconsToDraw.Count - 1);
                }
            }

            for (int i = 0; i < iconsToDraw.Count; i++)
            {
                (Texture texture, bool important) = iconsToDraw[i];
                Color tint = important
                    ? new Color(1, 1, 1, 1)
                    : new Color(0.8f, 0.8f, 0.8f, 0.25f);
                GUI.DrawTexture(GetRightRectWithOffset(rect, i), texture, ScaleMode.ScaleToFit, true, 0, tint, 0, 0);
            }

            numberOfIconsDrawn = iconsToDraw.Count;
        }

        private static bool CheckTypeRecursive(Type t, HashSet<Type> set)
        {
            if (set.Contains(t))
                return true;

            if (t.BaseType == null)
                return false;

            return CheckTypeRecursive(t.BaseType, set);
        }

        private static Texture GetIconFor(Component c, Type type)
        {
            return iconOverrides.TryGetValue(type, out string icon)
                ? EditorGUIUtility.IconContent(icon).image
                : EditorGUIUtility.ObjectContent(c, type).image;
        }

        private static void DrawActivityToggle(Rect rect, GameObject go)
        {
            // Get's style of toggle
            bool active = go.activeInHierarchy;

            GUIStyle toggleStyle = active
                ? toggleStyleName
                : mixedToggleStyleName;

            // Sets rect for toggle
            var toggleRect = new Rect(rect);
            toggleRect.width = toggleRect.height;
            toggleRect.x -= 28;

            // Creates toggle
            bool state = GUI.Toggle(toggleRect, go.activeSelf, GUIContent.none, toggleStyle);

            // Sets game's active state to result of toggle
            if (state != go.activeSelf)
            {
                Undo.RecordObject(go, $"{(state ? "Enabled" : "Disabled")}");
                go.SetActive(state);
                Undo.FlushUndoRecordObjects();
            }
        }

        static Rect GetRightRectWithOffset(Rect rect, int offset)
        {
            var newRect = new Rect(rect);
            newRect.width = newRect.height;
            newRect.x = rect.x + rect.width - (rect.height * offset) - 16;
            return newRect;
        }
    }
}
