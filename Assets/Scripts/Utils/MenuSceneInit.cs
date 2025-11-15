using UnityEngine;

public class MenuSceneInit : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMenuMusic();
    }
}
