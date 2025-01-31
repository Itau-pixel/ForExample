using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Save : MonoBehaviour
{
    public int score_SpeedTap, score_FocusStrike, score_FlashReflex;

    public bool onboardingStart;

    [Serializable]
    class SaveData
    {
        public int _score_SpeedTap, _score_FocusStrike, _score_FlashReflex;

        public bool _onboardingStart;
    }
    public void RestartSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/SavesData.txt");
        SaveData data = new SaveData();

        score_SpeedTap = 0; score_FocusStrike = 0; score_FlashReflex = 0; onboardingStart = true;
        score_SpeedTap = data._score_SpeedTap;
        score_FocusStrike = data._score_FocusStrike;
        score_FlashReflex = data._score_FlashReflex;

        onboardingStart = data._onboardingStart;

        bf.Serialize(file, data);
        file.Close();
    }
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/SavesData.txt");
        SaveData data = new SaveData();

        data._score_SpeedTap = score_SpeedTap;
        data._score_FocusStrike = score_FocusStrike;
        data._score_FlashReflex = score_FlashReflex;

        data._onboardingStart = onboardingStart;

        bf.Serialize(file, data);
        file.Close();
    }
    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SavesData.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SavesData.txt", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);

            score_SpeedTap = data._score_SpeedTap;
            score_FocusStrike = data._score_FocusStrike;
            score_FlashReflex = data._score_FlashReflex;

            onboardingStart = data._onboardingStart;

            file.Close();
        }
        else
            RestartSave();
    }
}
