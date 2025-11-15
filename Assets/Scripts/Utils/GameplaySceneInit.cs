using UnityEngine;

public class GameplaySceneInit : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayGameplayMusic();
    }
}


