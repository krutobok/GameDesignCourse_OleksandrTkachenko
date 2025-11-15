using UnityEngine;

public class RaycastSelector : MonoBehaviour
{
    public Camera cam; 
    public float rayDistance = 100f;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("canBuy"))
            {                
                CursorController.Instance.SetCursorToHand();
            }
            else
            {
                CursorController.Instance.SetCursorToDefault();
            }
        }
        else
        {
            CursorController.Instance.SetCursorToDefault();
        }

        if (Input.GetMouseButtonDown(1)) // ПКМ
        {
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("canBuy"))
                {
                    Transform parent = hit.collider.transform.parent;
                    if (parent != null)
                    {
                        Debug.Log("Батьківський об'єкт: " + parent.name);
                        OpenBuyPanel(parent);
                    }
                    else
                    {
                        Debug.Log("У об'єкта немає батьківського об'єкта.");
                    }
                }
                else
                {
                    Debug.Log("Попадання в об'єкт: " + hit.collider.gameObject.name + " (але він не має тегу 'canBuy')");
                }
            }
            else
            {
                Debug.Log("Нічого не влучено.");
            }
        }
    }
    public void OpenBuyPanel(Transform objectToBuy)
    {
        BuyPanelManager.Instance.TogglePanelVisibility(objectToBuy);     
        GameTimeManager.Instance.IsGameTimeRunning = false;            
    }
}

