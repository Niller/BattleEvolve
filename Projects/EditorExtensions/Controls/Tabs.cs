using System;
using System.Collections.Generic;

namespace EditorExtensions.Controls
{
    public class Tabs
    {
        public Action<int> OnTabRemove;
        public Action<int> OnSelectionChange;

        private int _tabsCount;

        public Tabs(int count)
        {
            _tabsCount = count;
        }
        
        public void AddTab()
        {
            
        }
        
        public void DoLayout(List<string> names)
        {
            
        }

        private void DrawTab(string name)
        {
            
        }
    }
}