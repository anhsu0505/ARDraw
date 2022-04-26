using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mic : MonoBehaviour {

    //Singletone
    public static Mic me { get; private set; }    
    private AudioClip myAudioClip;

    //Time for record cant be endless in Unity3D
    //so we combine several short parts with max duration equal TIME_FOR_TEMP_RECORD
    //in one big .wav file
    private const int TIME_FOR_TEMP_RECORD = 60;

    //Default recording frequency. Wav quality setting.
    private const int FREQUENCY = 11025;  

    //Private VAR to allow recording
    private bool isCommandToRecord;

    //This variable contains path to last saved record.
    public string strLastSavedPath;

    //This variable contains TRUE if recording in progress and FALSE if not.
    public bool isRecordind {
        get { return Microphone.IsRecording(null); } 
    }

    //AudioClip container to combine AudioParts after recording
    private List <AudioClip> pausedClip =  new List <AudioClip>();

    //Recording frequency. If the microphone has a different one, then the appropriate one will be selected from the list
    private int SAMLE_RATE = FREQUENCY;

     // Use this for initialization
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if (me != null && me != this) {
            // First we check if there are any other instances conflicting
            Destroy(gameObject); // If that is the case, we destroy other instances
        } else {
            me = this; // Here we save our singleton instance        
        }

        if (Microphone.devices.Length <= 0) {
            Debug.Log("You have NO input devices to Record Sound!");           
        } else {
            Debug.Log("You have " + Microphone.devices.Length + " input devices:");
            for (int i = 0; i < Microphone.devices.Length; ++i) {
                Debug.Log(i+" : "+Microphone.devices[i]);
            }
        }

        //List of available frequencies
        int[] sampleRateList = { 8000, 11025, 16000, 22050, 32000, 44100, 48000, 96000, 192000 };

        //Here we get the maximum and minimum available frequency
        int minRate, maxRate;
        Microphone.GetDeviceCaps(null, out minRate, out maxRate);

        //Setting the frequency most suitable for the microphone based on our list
        foreach (var rate in sampleRateList)
        {
            if (rate >= minRate && rate <= maxRate)
            {
                SAMLE_RATE = rate;
            }
        }

        Debug.Log("Recording frequency: " + SAMLE_RATE);
    }


    public void RecordStart() {
         isCommandToRecord = true;
    }

    public void RecordUnPause() {
         isCommandToRecord = true;
    }

    void FixedUpdate() {
        if (!isCommandToRecord || Microphone.IsRecording(null) ) return;
        //Stream record start here
        myAudioClip = Microphone.Start(null, false, TIME_FOR_TEMP_RECORD, SAMLE_RATE);
        //this line execute only onve
        pausedClip.Add(myAudioClip);      
    }
 

    public AudioClip RecordStopAndSave(string nameEnd) {
        isCommandToRecord = false;
        Microphone.End(null);
        string filename = string.Format("{0}_{1}", SceneManager.GetActiveScene().name,nameEnd);
        pausedClip[pausedClip.Count-1] = SavWav.TrimSilence(pausedClip[pausedClip.Count-1], 0);
        AudioClip newClip ;

        if (pausedClip.Count > 1) {
            newClip = Combine(pausedClip);
        } else {
            newClip = pausedClip[0];
        }
        
        if (newClip == null) {
            Debug.Log("Too short probably");
            return null;
        }
        strLastSavedPath = Path.Combine(Application.persistentDataPath, filename);
        SavWav.Save(filename,newClip );
        pausedClip = new List <AudioClip>();
        return newClip ;
    }

    public void RecordPause() {
        isCommandToRecord = false;
        Microphone.End(null);
        pausedClip[pausedClip.Count-1] = SavWav.TrimSilence(pausedClip[pausedClip.Count-1], 0);
    }

    public IEnumerator LoadAndPlay(string filename, AudioSource aus) {
        string fullFileNames = "file:///" + Path.Combine(Application.persistentDataPath, filename);
        WWW www = new WWW(fullFileNames);
        yield return www;
        AudioClip tempClip = www.GetAudioClip(false);
        tempClip.name = filename;
        aus.clip = tempClip;
        aus.Play();
    }

    public void Rename(string oldName, string newName) {
        File.Move(oldName, newName);
   }

    private AudioClip Combine(List <AudioClip> clips) {
        if (clips == null || clips.Count == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Count; ++i) {
            if (clips[i] == null)
                continue;
            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Count; ++i) {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        AudioClip result = AudioClip.Create("Combine", length, clips[0].channels, clips[0].frequency, false);
        result.SetData(data, 0);
        return result;
    }
    
    public FileInfo[] LoadFileNames() {
        DirectoryInfo directoryInfo = new DirectoryInfo (Application.persistentDataPath);
        return  directoryInfo.GetFiles ("*.wav", SearchOption.AllDirectories);        
    }
    
}
