using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recorder;

public class UIManager : MonoBehaviour
{
    public RecordManager recordManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartVid()
    {
        recordManager.StartRecord();
    }

    public void SaveVid()
    {
        recordManager.StopRecord();
    }
}
