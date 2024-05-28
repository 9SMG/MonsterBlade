using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SMG.Utility
{
    [CustomEditor(typeof(MonsterBlade.UI.DoModal), editorForChildClasses: true)]
    public class ClassTooltipDrawer : Editor
    {
        string tooltip;

        private void OnEnable()
        {
            var attributes = target.GetType().GetCustomAttributes(inherit: false);
            foreach (var attr in attributes)
            {
                if (attr is ClassTooltipAttribute tooltip)
                {
                    this.tooltip = tooltip.description;
                    break;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(this.tooltip);
            base.OnInspectorGUI();
        }
    }

}

