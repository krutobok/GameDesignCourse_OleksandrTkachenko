using UnityEngine;

public class SaveGameData
{
    public static void saveGameData()
    {
        GameData wrapper = new GameData();
        wrapper.money = GameMoneyManager.Instance.currentMoney;
        wrapper.day = GameTimeManager.Instance.day;
        wrapper.gameTime = (int)GameTimeManager.Instance.gameTime;

        string gameData = JsonUtility.ToJson(wrapper, true);
        Debug.Log(gameData);

        string filePath = Application.persistentDataPath + "/gameData.json";
        System.IO.File.WriteAllText(filePath, gameData);
    }
}
