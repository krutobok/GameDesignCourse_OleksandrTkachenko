using UnityEngine;

public class InventoryIconGenerator : MonoBehaviour
{
    public static Sprite GenerateIcon(ItemInstance itemInstance)
    {
        if (itemInstance == null || itemInstance.template == null || itemInstance.template.itemPrefab == null)
        {
            Debug.LogWarning("ItemInstance або itemPrefab порожній.");
            return null;
        }

        // Створюємо тимчасову сцену
        GameObject tempParent = new GameObject("TempIconRoot");
        GameObject itemObj = Instantiate(itemInstance.template.itemPrefab, new Vector3(0, 0, 1f),                       
        Quaternion.Euler(0, 45, 0), tempParent.transform);

        // Знаходимо дочірній об'єкт з тегом "item"
        GameObject itemPart = null;
        foreach (Transform child in itemObj.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("item"))
            {
                itemPart = child.gameObject;
                break;
            }
        }

        if (itemPart == null)
        {
            Debug.LogWarning("Не знайдено дочірній об'єкт з тегом 'item'. Використовується весь префаб.");
            itemPart = itemObj;
        }

        // Застосовуємо колір
        Renderer renderer = itemPart.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material = itemInstance.color;
        }

        // Створюємо камеру
        GameObject camObj = new GameObject("TempCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = Color.clear;
        cam.orthographic = true;
        cam.orthographicSize = 0.5f; // 👈 Можеш налаштувати для кращого зображення
        cam.transform.position = itemPart.transform.position + new Vector3(0, 0, -2f);
        cam.transform.LookAt(itemPart.transform);

        // Створюємо RenderTexture
        RenderTexture rt = new RenderTexture(256, 256, 16);
        cam.targetTexture = rt;
        cam.Render();

        // Зчитуємо вміст RenderTexture
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        // Очищення
        RenderTexture.active = null;
        cam.targetTexture = null;
        Object.DestroyImmediate(tempParent);
        Object.DestroyImmediate(camObj);
        Object.DestroyImmediate(rt);

        // Створюємо спрайт
        Sprite icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return icon;
    }
}
