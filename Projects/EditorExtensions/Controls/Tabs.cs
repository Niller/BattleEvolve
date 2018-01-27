using System;
using System.Collections.Generic;
using System.Diagnostics;
using EditorExtensions.Utilities;
using UnityEditor;
using UnityEngine;
using Utilities.Extensions;

namespace EditorExtensions.Controls
{
    public class Tabs
    {
        private const int TabWidth = 100;
        
        private int _selectionIndex;
        private int _tabsCount;

        public Action<int> OnTabRemove;
        public Action<int> OnSelectionChange;
        public Action OnTabAddClick;


        public int SelectionIndex
        {
            get
            {
                return _selectionIndex;
            }
            set
            {
                if (value > _tabsCount - 1)
                {
                    throw new IndexOutOfRangeException("SelectionIndex > TabsCount");
                }
                _selectionIndex = value;
                OnSelectionChange.SafeInvoke(value);
            }
        }

        public Tabs(int count)
        {
            _tabsCount = count;
            SelectionIndex = 0;
        }
        
        public void AddTab()
        {
            SelectionIndex = _tabsCount++;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void RemoveTab(int index)
        {
            _tabsCount--;
            OnTabRemove.SafeInvoke(index);
            SelectionIndex = Mathf.Clamp(SelectionIndex, 0, _tabsCount - 1);
        }
        
        public void DoLayout(Rect rect, List<string> names)
        {
            for (var i = 0; i < names.Count; i++)
            {
                var name = names[i];
                DrawTab(new Rect(rect.x + TabWidth*i, rect.y, TabWidth, rect.height), i, name, names.Count > 1);
            }

            if (GUI.Button(new Rect(rect.x + names.Count  * TabWidth + 10, rect.y, 20, 20), "+"))
            {
                OnTabAddClick.SafeInvoke();
            }
        }

        private void DrawTab(Rect rect, int index, string name, bool showCloseButton)
        {
            GUILayout.BeginArea(new Rect(rect.x + 2, rect.y, rect.width - 2, rect.height - 2));
            var bColor = GUI.backgroundColor;
            if (index == SelectionIndex)
            {
                GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            }
            else
            {
                GUI.backgroundColor = BuiltInResources.GetDefaultSkinColor(EditorGUIUtility.isProSkin);
            }
            
            GUILayout.BeginHorizontal("box");
            GUI.backgroundColor = bColor;
            
            GUILayout.Label(name, GUILayout.Width(TabWidth - 40));
            if (showCloseButton && GUILayout.Button("X", GUILayout.Width(20)))
            {
                RemoveTab(index);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
            
            var evt = Event.current;
            if (evt.type != EventType.MouseUp || !rect.Contains(evt.mousePosition))
            {
                return;
            }

            SelectionIndex = index;
        }
    }
}