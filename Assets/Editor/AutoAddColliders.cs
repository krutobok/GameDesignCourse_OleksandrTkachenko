using UnityEngine;
using UnityEditor;

public class AutoAddColliders : EditorWindow
{
    [MenuItem("Tools/Add Mesh Colliders to All Meshes in Scene")]
    public static void AddColliders()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<MeshFilter>() && !obj.GetComponent<MeshCollider>())
            {
                MeshCollider collider = obj.AddComponent<MeshCollider>();
                collider.convex = false; // для кімнати — Convex не потрібен
                count++;
            }
        }

        Debug.Log("Додано Mesh Collider до " + count + " об'єктів.");
    }
}
