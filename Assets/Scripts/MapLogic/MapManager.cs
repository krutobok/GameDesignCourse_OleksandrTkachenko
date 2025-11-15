using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using Holistic3D.Inventory;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public MapData mapData;
    public string currentLocation;
    [SerializeField] private MapLocationButton[] locationButtons;

    public bool IsVisible
    {
        get
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            if (canvasGroup.alpha == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ”никаЇмо дублю
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    private void Start()
    {
        foreach (var btn in locationButtons)
        {
            var capturedName = btn.LocationName;
            var capturedMode = btn.defaultTravelMode;

            btn.GetComponent<Button>().onClick.AddListener(() => {
                    TravelTo(capturedName, capturedMode);
                    TogglePanelVisibility();
                }
            );
        }
        LockLocation(currentLocation);
    }

    public void SetPanelVisibility(bool isVisible)
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.blocksRaycasts = isVisible;
        canvasGroup.interactable = isVisible;
        GameTimeManager.Instance.IsGameTimeRunning = !isVisible;
    }
    public void TogglePanelVisibility()
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup.alpha == 1)
        {
            GameTimeManager.Instance.IsGameTimeRunning = true;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        else
        {
            GameTimeManager.Instance.IsGameTimeRunning = false;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

    }


    public void TravelTo(string targetLocation, string travelMode)
    {
        var currentNode = mapData.nodes.FirstOrDefault(n => n.locationName == currentLocation);
        if (currentNode == null) return;

        var edge = currentNode.connections.FirstOrDefault(c => c.targetLocation == targetLocation);
        if (edge == null) return;

        int travelTime = travelMode switch
        {
            "Walk" => edge.walkTime,
            "Bus" => edge.busTime,
            "Car" => edge.carTime,
            _ => 0
        };

        

        var targetNode = mapData.nodes.FirstOrDefault(n => n.locationName == targetLocation);
        if (targetNode != null)
        {
            GameTimeManager.Instance.AddTime(travelTime);
            InventoryPanelManager.Instance.saveInventory();
            SaveGameData.saveGameData();
            SecondHandSpawner.Instance.SaveToJson();
            UnlockLocation(currentLocation);
            LockLocation(targetLocation);
            currentLocation = targetLocation;
            SceneManager.LoadScene(targetNode.sceneName);
        }
    }
    public void LockLocation(string locationName)
    {
        var btn = System.Array.Find(locationButtons, b => b.LocationName == locationName);
        if (btn != null)
            btn.SetInteractable(false);
    }

    public void UnlockLocation(string locationName)
    {
        var btn = System.Array.Find(locationButtons, b => b.LocationName == locationName);
        if (btn != null)
            btn.SetInteractable(true);
    }
}

