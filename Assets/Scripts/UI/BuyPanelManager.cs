using UnityEngine;
using UnityEngine.UI;

public class BuyPanelManager : MonoBehaviour
{    
    public static BuyPanelManager Instance { get; private set; }
    [SerializeField] private Button buyButton;
    [SerializeField] private Button exitButton;
    public string currentItemId = null;
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
        Debug.Log("Try to createObject");
        if (Instance != null)
        {
            Destroy(gameObject);
            Debug.Log("Not new");
        }
        else
        {
            Instance = this;
            Debug.Log("new");
        }
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        exitButton.onClick.AddListener(() => OnExitButtonClick());
    }

    public void SetPanelVisibility(bool isVisible)
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.blocksRaycasts = isVisible;
        canvasGroup.interactable = isVisible;
    }
    public void TogglePanelVisibility(Transform? objectToBuy = null)
    {
        CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup.alpha == 1)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            currentItemId = objectToBuy.GetComponent<SecondHandItem>().itemData.item.uniqueId;
        }     
        ConfigBuyButton(objectToBuy);
    }
    public void ConfigBuyButton(Transform? objectToBuy = null)
    {
        if (objectToBuy != null)
        {
            SecondHandItem secondHandItem = objectToBuy.gameObject.GetComponent<SecondHandItem>();
            int cost = secondHandItem.itemData.item.price;
            buyButton.GetComponentInChildren<TMPro.TMP_Text>().text = "buy - " + cost;
            buyButton.onClick.RemoveAllListeners();
            if (GameMoneyManager.Instance.canBuy(cost))
            {

                buyButton.onClick.AddListener(() => OnBuyButtonClick(objectToBuy));

                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }
        else
        {
            buyButton.onClick.RemoveAllListeners();
        }
           
    }
    private void OnExitButtonClick()
    {
        GameTimeManager.Instance.IsGameTimeRunning = true;
        currentItemId = null;
        TogglePanelVisibility();
    }
    private void OnBuyButtonClick(Transform? objectToBuy = null)
    {
        if (objectToBuy != null)        
        {
        SecondHandItem secondHandItem = objectToBuy.gameObject.GetComponent<SecondHandItem>();
            if (GameMoneyManager.Instance.canBuy(secondHandItem.itemData.item.price))
            {                
                GameTimeManager.Instance.IsGameTimeRunning = true;
                bool result = GameMoneyManager.Instance.Buy(secondHandItem.itemData.item);
                if (result)
                {
                    Destroy(objectToBuy.GetComponent<SecondHandItem>());
                    Destroy(objectToBuy.gameObject);
                    currentItemId = null;
                }
                TogglePanelVisibility();                                
                              
            }
        }       
     
    }
}
