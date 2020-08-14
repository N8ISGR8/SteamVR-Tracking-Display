using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Toggle autoStartTracking;
    private int entries = 1;
    private string[] currentData;
    public bool autoStart;

    private void Start()
    {
        LoadData();
        autoStartTracking.isOn = autoStart;
        if (autoStart)
        {
            ReadRunPosition.instance.Setup();
            ReadRunPosition.instance.RunNoClick();
        }
    }

    public void SaveData()
    {
#if UNITY_EDITOR
        string path = "Assets/SaveData/Config.txt";
#else
        DirectoryInfo di = new DirectoryInfo(Application.dataPath);
        string path = di.Parent.ToString() + "/UserData/Config.txt";
#endif
        string[] lines = File.ReadAllLines(path);
        if(lines.Length < entries)
        {
            string[] temp = lines;
            lines = new string[entries];
            for(int i = 0; i < entries; i++)
            {
                if(i < temp.Length)
                {
                    lines[i] = temp[i];
                }
            }
        }
        lines[0] = "Autostart Tracking: " + autoStartTracking.isOn;
        File.WriteAllLines(path,lines);
    }

    public void LoadData()
    {
#if UNITY_EDITOR
        string path = "Assets/SaveData/Config.txt";
#else
        DirectoryInfo di = new DirectoryInfo(Application.dataPath);
        string path = di.Parent.ToString() + "/UserData/Config.txt";
#endif
        string[] lines = File.ReadAllLines(path);
        if (lines.Length >= entries)
        {
            string[] tokens = lines[0].Split(':');
            autoStart = bool.Parse(tokens[1]);
        } else
        {
            SaveData();
        }
    }

}
