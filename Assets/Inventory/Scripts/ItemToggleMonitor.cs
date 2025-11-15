using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Holistic3D.Inventory
{
    public class ItemToggleMonitor : MonoBehaviour
    {
        public ToggleGroup toggleGroup;
        private Toggle lastSelectedToggle;
        void Start()
        {
            foreach(var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                toggle.onValueChanged.AddListener(isOn =>
                {
                    if (isOn)
                    {
                        HandleToggleChanged(toggle);
                    }
                });
                if (toggle.isOn) InventoryPanelManager.Instance.FilterItemsByType(toggle.GetComponent<ToggleItemType>().toggleItemType);
            }
        }
        
        private void HandleToggleChanged(Toggle selecteToggle)
        {
            if(selecteToggle != lastSelectedToggle)
            {
                lastSelectedToggle = selecteToggle;
                ItemType selectedType = lastSelectedToggle.GetComponent<ToggleItemType>().toggleItemType;
                InventoryPanelManager.Instance.FilterItemsByType(selectedType);
            }
        }

    }
}

