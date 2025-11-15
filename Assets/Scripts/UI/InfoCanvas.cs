using UnityEngine;
using UnityEngine.UI;

public class InfoCanvas : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text moneyText;
    [SerializeField] private TMPro.TMP_Text timeText;
    [SerializeField] private TMPro.TMP_Text dayText;
    void Start()
    {
        moneyText.text = "Money: " + GameMoneyManager.Instance.currentMoney;
        timeText.text = "Time: " + (int)GameTimeManager.Instance.gameTime;
        dayText.text = "Day: " + GameTimeManager.Instance.day;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Money: " + GameMoneyManager.Instance.currentMoney;
        updateTime();
        dayText.text = "Day: " + GameTimeManager.Instance.day;
    }

    void updateTime()
    {
        float minutesFromStart = GameTimeManager.Instance.gameTime;
        int totalMinutes = (int)minutesFromStart;
     
        int hour = 7 + totalMinutes / 60;
        int minute = totalMinutes % 60;
       
        if (hour > 22)
            hour = 22;
        timeText.text = "Time: " + hour.ToString("00") + ":" + minute.ToString("00");
    }
}
