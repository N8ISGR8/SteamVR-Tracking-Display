using UnityEngine;
using System.Threading;

public class SetPosition : MonoBehaviour
{
    public static SetPosition instance;
    public Transform trackingParent;
    public GameObject lConPrefab;
    public GameObject rConPrefab;
    public GameObject hmdPrefab;
    private GameObject lCon;
    private GameObject rCon;
    private GameObject hmd;
    private string device;
    private float[] lPoses;
    private float[] rPoses;
    private float[] hPoses;

    private void Start()
    {
        lCon = Instantiate(lConPrefab, trackingParent);
        rCon = Instantiate(rConPrefab, trackingParent);
        hmd = Instantiate(hmdPrefab, trackingParent);
        lPoses = new float[6];
        rPoses = new float[6];
        hPoses = new float[6];
        instance = this;
        Thread t = new Thread(new ThreadStart(ReadPosition.Start));
        t.Start();
    }

    private void Update()
    {
        lCon.transform.localPosition = new Vector3(lPoses[0], lPoses[1], lPoses[2]);
        rCon.transform.localPosition = new Vector3(rPoses[0], rPoses[1], rPoses[2]);
        hmd.transform.localPosition = new Vector3(hPoses[0], hPoses[1], hPoses[2]);
        lCon.transform.localRotation = Quaternion.Euler(lPoses[3], lPoses[4], lPoses[5]);
        rCon.transform.localRotation = Quaternion.Euler(rPoses[3], rPoses[4], rPoses[5]);
        hmd.transform.localRotation = Quaternion.Euler(hPoses[3], hPoses[4], hPoses[5]);
    }

    public void Deserialize(string s)
    {
        string[] devices = s.Split('/');
        string[] tokensH = devices[0].Split('|');
        string[] tokensL = devices[1].Split('|');
        string[] tokensR = devices[2].Split('|');
        for (int i = 0; i < tokensH.Length && i < 6; i++)
        {
            hPoses[i] = float.Parse(tokensH[i]);
        }
        for (int i = 0; i < tokensL.Length && i < 6; i++)
        {
            lPoses[i] = float.Parse(tokensL[i]);
        }

        for (int i = 0; i < tokensR.Length && i < 6; i++)
        {
            rPoses[i] = float.Parse(tokensR[i]);
        }
    }
}
