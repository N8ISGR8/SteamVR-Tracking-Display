﻿using UnityEngine;
using System.Net.Sockets;
using System;
using System.IO;
using UnityEngine.UI;

public class ReadRunPosition : MonoBehaviour
{
    //network
    internal bool socketReady = false;
    TcpClient socket;
    NetworkStream stream;
    StreamWriter writer;
    StreamReader reader;
    public string host = "127.0.0.1";
    public int port = 32779;

    //random
    public Toggle isOn;
    public static ReadRunPosition instance;
    public GameObject restartDashUI;
    public GameObject regularUI;
    private bool dashSetup = false;
    public Button runButton;
    private bool uiOpen;

    //vr tracking
    public bool readPositions = false;
    private bool setup = false;
    private GameObject hmd;
    private GameObject lCon;
    private GameObject rCon;


    public void OpenDash()
    {
        if (!dashSetup)
        {
            dashSetup = true;
            restartDashUI.SetActive(false);
            regularUI.SetActive(true);
        }
        Debug.Log("OPEN UI");
        uiOpen = true;
    }

    public void CloseDash()
    {
        uiOpen = false;
    }

    private void Awake()
    {
        instance = this;
    }

    public void RunNoClick()
    {
        Debug.Log("RUN CLICK");
        Run();
    }

    public void RunClick()
    {
        if (uiOpen)
        {
            Run();
        }
    }

    public void Run()
    {
        Debug.Log("RUN");
        Unity_SteamVR_Handler handler = FindObjectOfType<Unity_SteamVR_Handler>();
        hmd = handler.hmdObject;
        lCon = handler.leftTrackerObj;
        rCon = handler.rightTrackerObj;
        readPositions = !readPositions;
        isOn.isOn = readPositions;
    }

    public void Setup()
    {
        setup = true;
        Unity_SteamVR_Handler handler = FindObjectOfType<Unity_SteamVR_Handler>();
        hmd = handler.hmdObject;
        lCon = handler.leftTrackerObj;
        rCon = handler.rightTrackerObj;
        SetPosition.instance.Run();
        SetupSocket();
    }

    private void Update()
    {
        if (readPositions)
        {
            Debug.Log("read position");
            if(!setup)
            {
                Setup();
            }
            string objs = GetVRObj(hmd).ToString() + "/" + GetVRObj(lCon).ToString() + "/" + GetVRObj(rCon).ToString();
            WriteSocket(objs);
        }
    }

    private OpenVRObj GetVRObj(GameObject go)
    {
        OpenVRObj obj = new OpenVRObj();
        obj.device = go.name;
        obj.posX = go.transform.position.x;
        obj.posY = go.transform.position.y;
        obj.posZ = go.transform.position.z;
        obj.rotX = go.transform.rotation.eulerAngles.x;
        obj.rotY = go.transform.rotation.eulerAngles.y;
        obj.rotZ = go.transform.rotation.eulerAngles.z;
        return obj;
    }
    
    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    public void SetupSocket()
    {
        socket = new TcpClient(host, port);
        stream = socket.GetStream();
        writer = new StreamWriter(stream);
        socketReady = true;
    }

    public void WriteSocket(string objs)
    {
        if (!socketReady)
            return;
        writer.Write(objs);
        writer.Flush();
    }
    public void CloseSocket()
    {
        if (!socketReady)
            return;
        socket.Close();
        socketReady = false;
    }

}
