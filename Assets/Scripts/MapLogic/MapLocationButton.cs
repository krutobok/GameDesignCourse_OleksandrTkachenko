using UnityEngine;
using UnityEngine.UI;

public class MapLocationButton : MonoBehaviour
{
    [SerializeField] public string locationName;
    [SerializeField] public string defaultTravelMode = "Walk";
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    public string LocationName => locationName;

    private void Reset()
    {
        button = GetComponent<Button>();
        iconImage = GetComponent<Image>();
    }

    public void Init(string locationName, Sprite icon)
    {
        this.locationName = locationName;
        if (iconImage != null)
            iconImage.sprite = icon;
    }

    public void OnClick()
    {
        MapManager.Instance.TravelTo(locationName, defaultTravelMode);
    }

    public void SetInteractable(bool state)
    {
        if (button != null)
            button.interactable = state;
    }
}

