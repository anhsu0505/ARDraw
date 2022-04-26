using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ExampleScreenRecorder : MonoBehaviour
{
	[SerializeField] private string folderName;
	[SerializeField] private bool isAudioRecording = true;
	[SerializeField] private int bitrate;
	[SerializeField] private int fps;
	[SerializeField] private SmileSoftScreenRecordController.VideoEncoder videoEncoder = SmileSoftScreenRecordController.VideoEncoder.H264;


	[SerializeField] private GameObject afterVideoCompletePanel;
	[SerializeField] private Text savedPathText;
	[SerializeField] private Button previewButton;
	[SerializeField] private Button ShareButton;

	private string _recordedFilePath;

	void Start()
	{
		HideAfterVideoCompletePanel();
		SetUp();
	}

	void SetUp()
	{
		// If want to store video in persistant data Path (Private Path) then use following line
		//SmileSoftScreenRecordController.instance.SetVideoStoringDestination(Application.persistentDataPath);
		//Do not want to show stored videos in gallery ,then uncomment following line.
		//SmileSoftScreenRecordController.instance.SetGalleryAddingCapabilities(false);

		SmileSoftScreenRecordController.instance.SetStoredFolderName(folderName);
		SmileSoftScreenRecordController.instance.SetBitRate(bitrate);
		SmileSoftScreenRecordController.instance.SetFPS(fps);
		SmileSoftScreenRecordController.instance.SetAudioCapabilities(isAudioRecording);

		SmileSoftScreenRecordController.instance.SetVideoEncoder((int)videoEncoder);

	}


	public void StartRecording()
	{
		SetFileName();
		SmileSoftScreenRecordController.instance.StartRecording();
	}
	public void StopRecording()
	{
		_recordedFilePath = null;
		_recordedFilePath = SmileSoftScreenRecordController.instance.StopRecording();
		ShowVideoCompletatoonDialog();
	}

	private void SetFileName()
	{
		System.DateTime now = System.DateTime.Now;
		string date = now.ToShortDateString().Replace('/', '_')
					+ now.ToLongTimeString().Replace(':', '_');
		string fileName = "Record_" + date;

		SmileSoftScreenRecordController.instance.SetVideoName(fileName);
	}

	private void ShowVideoCompletatoonDialog()
	{
		afterVideoCompletePanel.SetActive(true);
		if (_recordedFilePath != null && File.Exists(_recordedFilePath))
		{
			previewButton.interactable = true;
			ShareButton.interactable = true;
			savedPathText.text = "Video saved successfully at : " + _recordedFilePath;
		}
		else
		{
			previewButton.interactable = false;
			ShareButton.interactable = false;
			savedPathText.text = "Error occured. Can not record video";
		}

	}

	public void  PreviewVideo()
	{
		SmileSoftScreenRecordController.instance.PreviewVideo(_recordedFilePath);
	}

	public void ShareVideo()
	{
		SmileSoftScreenRecordController.instance.ShareVideo(_recordedFilePath, "Greetings From SmileSoft", "Sunshine Native Share");
	}

	public void HideAfterVideoCompletePanel()
	{
		afterVideoCompletePanel.SetActive(false);
	}

}
