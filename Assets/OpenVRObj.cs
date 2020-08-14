using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenVRObj
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;
    public string device;

    public override string ToString()
    {
        string s = posX + "|" + posY + "|" + posZ + "|";
        s += rotX + "|" + rotY + "|" + rotZ;

        return s;
    }
}
