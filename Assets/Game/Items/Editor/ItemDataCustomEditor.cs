using UnityEditor;
using UnityEngine;

namespace InventoryDemo.Items.Editor
{
    [CustomEditor(typeof(PickableItem))]
    public class ItemDataCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            DrawDefaultInspector();
            
            if (GUILayout.Button("Regenerate instance ID"))
            {
                PickableItem data = (PickableItem) target;
                data.Editor_RegenerateID();
            }
        }
    }
}