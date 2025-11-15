using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Перевіряємо, чи це гравець (треба, щоб у гравця був тег "Player")
        if (other.CompareTag("Player"))
        {
            MapManager.Instance.TogglePanelVisibility();
        }
    }
}

