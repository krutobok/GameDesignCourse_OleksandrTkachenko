using UnityEngine;
using UnityEditor;

namespace Holistic3D.Inventory
{
    [CustomEditor(typeof(InventorySystem))]
    public class InventorySystemEditor : Editor
    {
        private Color lineColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        private float rowHeight = 20f;

        public override void OnInspectorGUI()
        {
            InventorySystem inventorySystem = (InventorySystem)target;
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxWeight"));

            if(inventorySystem.slots == null || inventorySystem.slots.Count == 0)
            {
                EditorGUILayout.LabelField("Inventory is empty.");
            }
            else
            {
                GUILayout.BeginVertical();
                DrawRowHeader();
                DrawHorizontalLine();
                for (int i = 0; i < inventorySystem.slots.Count; i++)
                {
                    DrawRow(inventorySystem.slots[i], i);
                    DrawHorizontalLine();
                }
                GUILayout.EndVertical();
            }

             
            
            serializedObject.ApplyModifiedProperties();
        }
        private void DrawRowHeader()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Item Name", EditorStyles.boldLabel, GUILayout.Width(200));
            EditorGUILayout.LabelField("Weight", EditorStyles.boldLabel, GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }
        private void DrawRow(InventorySlot slot, int index)
        {
            GUILayout.BeginHorizontal();
            string itemName = slot.item != null ? slot.item.itemName : "Empty Slot";
            EditorGUILayout.LabelField(itemName, GUILayout.Width(200));

            int weight = slot.item != null ? slot.Weight : 0;
            EditorGUILayout.LabelField(weight.ToString(), GUILayout.Width(200));

            GUILayout.EndHorizontal();
        }
        private void DrawHorizontalLine()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, lineColor);
        }
    }


}

