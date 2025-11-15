using System;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;
    private float nextTriggerTime = 0f;
    public float hourseDuration = 60f;
    public float gameTime { get; private set; } = 0f; // ігровий час у секундах
    public int day { get; private set; } = 1;

    private bool _isGameTimeRunning = true;

    public bool IsGameTimeRunning
    {
        get => _isGameTimeRunning;
        set
        {
            _isGameTimeRunning = value;            
            // Коли час зупинено — показати курсор, і навпаки
            if (CursorController.Instance != null)
            {               
                CursorController.Instance.SetCursorActive(!_isGameTimeRunning);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadFromJson();
        if (GameTimeManager.Instance.gameTime <= hourseDuration)
        {
            nextTriggerTime = hourseDuration * 2;
        }
        else
        {
            nextTriggerTime = (int)Math.Ceiling(GameTimeManager.Instance.gameTime / hourseDuration) * hourseDuration;
        }
        Debug.Log(nextTriggerTime);
        
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (IsGameTimeRunning)
        {
            gameTime += Time.deltaTime;

            // Наприклад, кожні 300 секунд — новий день
            if (gameTime >= 900f)
            {
                day++;
                gameTime = 0f;
                Debug.Log("Новий день: " + day);
                SaleManager.Instance.ProcessSales(day);
            }
        }
    }
    public bool isTimeToItemDisappear()
    {
        if (gameTime >= nextTriggerTime)
        {
            Debug.Log("Item disappear" + gameTime);
            if (gameTime >= hourseDuration * 12)
            {
                nextTriggerTime += hourseDuration * 4;
            }
            else
            {
                nextTriggerTime += hourseDuration;
            }           
           
            return true;
        }
        return false;
    }
    public void AddTime(int timeToAdd)
    {
        gameTime += timeToAdd;
    }

    private void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/gameData.json";
        if (System.IO.File.Exists(filePath))
        {
            string gameData = System.IO.File.ReadAllText(filePath);
            Debug.Log(gameData);

            GameData wrapper = JsonUtility.FromJson<GameData>(gameData);
            gameTime = wrapper.gameTime;
            day = wrapper.day;
        }
    }
}
