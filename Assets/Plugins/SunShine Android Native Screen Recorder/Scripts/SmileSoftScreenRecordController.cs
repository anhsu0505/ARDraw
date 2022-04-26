using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SmileSoftScreenRecordController : MonoBehaviour
{
	public static SmileSoftScreenRecordController instance;

	private string _fileProvider = "com.Junru.dilmerv";

	private AndroidJavaObject screenRecorder;


	private void Awake()
	{
		if (instance == null)
			instance = this;

		Setup();
	}

	void Setup()
	{
		if (IsAndroidPlatform())
		{
			screenRecorder = new AndroidJavaObject("com.SmileSoft.unityplugin.ScreenCapture.ScreenRecordFragment");

			screenRecorder?.Call("SetUp");
		}
	}

	public void StartRecording()
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("StartRecording");
	}

	public string StopRecording()
	{
		if (IsAndroidPlatform())
		{
			string recordedPath = screenRecorder?.Call<string>("StopRecording");
			Debug.Log("Unity>> record path : " + recordedPath);
			return recordedPath;
		}

		return null;
	}

	public void SetVideoStoringDestination(string destination)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetVideoStoringDestination", destination);
	}
	public void SetStoredFolderName(string folderName)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetVideoStoredFolderName", folderName);
	}

	public void SetVideoName(string videoName)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetVideoName", videoName);
	}

	public void SetGalleryAddingCapabilities(bool canAddintoGallery)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetGalleryAddingCapabilities", canAddintoGallery);
	}

	public void SetAudioCapabilities(bool canRecordAudio)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetAudioCapabilities", canRecordAudio);
	}
	public void SetFPS(int fps)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetVideoFps", fps);
	}

	public void SetBitRate(int bitRate)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetBitrate", bitRate);
	}

	public void SetVideoSize(int width, int height)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetVideoSize", width, height);
	}

	public void SetVideoEncoder(int encoder)
	{
		if (IsAndroidPlatform())
			screenRecorder?.Call("SetVideoEncoder", encoder);
	}

	public void PreviewVideo(string videoPath)
	{
		if (IsAndroidPlatform() && (videoPath != null && File.Exists(videoPath)) )
			screenRecorder?.Call("PreviewVideo", videoPath);
	}

	public void ShareVideo(string filePath,string message, string title)
	{
		if (IsAndroidPlatform() &&  (filePath != null && File.Exists(filePath)))
			screenRecorder?.Call("ShareVideo", filePath,message,title,_fileProvider);
	}

	public enum VideoEncoder
	{
		DEFAULT = 0, H263 = 1, H264 = 2, MPEG_4_SP = 3, VP8 = 4, HEVC = 5
	}

	private bool IsAndroidPlatform()
	{
		bool result = false;

#if UNITY_ANDROID && !UNITY_EDITOR
		result = true;
#endif

		return result;
	}


}
