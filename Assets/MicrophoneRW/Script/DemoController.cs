using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoController : MonoBehaviour {

    private float timeRecording;
    private AudioSource _aus;
    public Slider TimeLine;
    public Text txtRecFile;
    public Text txtRecTime;
    public Text txtClipTime;
    public Dropdown ddWafFiles;
    public InputField ifRename;
    // Flag to know if we are draging the Timeline handle
    private bool isTimeLineOnDrag = false;

    private AudioClip clipToVoice;

    void Start() {
        _aus = GetComponent <AudioSource>();
        RefreshFiles();
    }


    public void PressRecStart() {
        timeRecording = 0;
        Mic.me.RecordStart();
    }

    public void PressRecPause() {
        txtRecTime.text = "REC paused: " +  Mathf.Floor(timeRecording / 60).ToString("00") + ":" +
                           Mathf.RoundToInt(timeRecording % 60).ToString("00");
        Mic.me.RecordPause();
    }

    public void PressRecUnPasue() {
        Mic.me.RecordUnPause();
    }

    public void PressRecStop() {
        // You can change name of file here. But path to file in Mic.cs
        string clipName = Regex.Replace(DateTime.Now.ToLongTimeString(), "[^0-9]", "");
        clipToVoice = Mic.me.RecordStopAndSave(clipName);

        //Path to last saved file stored in "strLastSavedPath"
        txtRecFile.text = string.Format("Record saved: {0}.wav ", Mic.me.strLastSavedPath);

        txtRecTime.text = "REC stopped: " + Mathf.Floor(timeRecording / 60).ToString("00") + ":" +
                           Mathf.RoundToInt(timeRecording % 60).ToString("00");
        RefreshFiles();
    }


    public void PressClipPlay() {
        _aus.clip = clipToVoice;
        _aus.Play();
    }

    public void PressClipStop() {
        _aus.Stop();
    }

    public void PressClipPause() {
        _aus.Pause();
    }

    public void PressClipUnPause() {
        _aus.UnPause();
    }

    void Update() {
    // isRecordind - public property. Helps to check rec status.
        if (Mic.me.isRecordind) {
            timeRecording += Time.deltaTime;
            txtRecTime.text = "REC in progress: " + Mathf.Floor(timeRecording / 60).ToString("00") + ":" +
                               Mathf.RoundToInt(timeRecording % 60).ToString("00");
            //  Debug.Log(timeRecording);
        }

        if (!_aus.isPlaying) return;

        if (isTimeLineOnDrag) {
            _aus.timeSamples = (int) (_aus.clip.samples * TimeLine.value);
        } else {
            if (_aus.clip) TimeLine.value = (float) _aus.timeSamples / (float) _aus.clip.samples;
            txtClipTime.text = Mathf.Floor(_aus.time / 60).ToString("00") + ":" +  Mathf.RoundToInt(_aus.time % 60).ToString("00");
          
        }
    }


    public void PressRename() {
        string fileold = Path.Combine(Application.persistentDataPath, ddWafFiles.captionText.text);
        string filenew = Path.Combine(Application.persistentDataPath, ifRename.text + ".wav");
        Mic.me.Rename(fileold, filenew);
        RefreshFiles();
        SetDropdownByText(ifRename.text);
    }

    public void PressLoad() {
        string fileLoad = Path.Combine(Application.persistentDataPath, ddWafFiles.captionText.text);
        StartCoroutine(Mic.me.LoadAndPlay(fileLoad, _aus));
    }

    public void PressDelete() {
        string fileDel = Path.Combine(Application.persistentDataPath, ddWafFiles.captionText.text);
        File.Delete(fileDel);
        RefreshFiles();
    }

    // Called by the event trigger when the drag begin
    public void TimeLineOnBeginDrag() {
        isTimeLineOnDrag = true;
        _aus.Pause();
    }

    // Called at the end of the drag of the TimeLine
    public void TimeLineOnEndDrag() {
        _aus.Play();
        isTimeLineOnDrag = false;
    }


    void RefreshFiles() {
        FileInfo[] fileInfo = Mic.me.LoadFileNames();
        ddWafFiles.options.Clear();
        foreach (FileInfo file in fileInfo) {
            //Debug.Log(file.Name);
            Dropdown.OptionData optionData = new Dropdown.OptionData(file.Name);
            ddWafFiles.options.Add(optionData);
        }
        ddWafFiles.value = 0;
        ddWafFiles.Select();
        ddWafFiles.RefreshShownValue();
    }


    void SetDropdownByText(string txt) {
        int result = ddWafFiles.options.FindIndex(val => val.text == txt);
        ddWafFiles.value = result;
    }

}
