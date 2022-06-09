using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TakePicture : MonoBehaviour
{
	private bool cameraAvailable;
	private bool evaluating = false;
	private Texture defaultBackground;
	private WebCamTexture activeCamera = null;

	[SerializeField] private RawImage background;
	[SerializeField] private AspectRatioFitter fit;
	[SerializeField] private Button switchCameraButton;

	[SerializeField] private GameObject cameraElements;
	[SerializeField] private GameObject evaluationElements;

	[SerializeField] private Material bubbleFaceMaterial;

	private int flip = 1;

	private void Start()
	{
		defaultBackground = background.texture;
		cameraElements.SetActive(true);
		evaluationElements.SetActive(false);

		if (WebCamTexture.devices.Length == 0)
		{
			Debug.Log("No cameras?");
			cameraAvailable = false;
			switchCameraButton.interactable = false;
			Debug.Log("Deactivating switch camera button");
			return;
		}

		foreach (WebCamDevice webCam in WebCamTexture.devices)
		{
			Debug.Log($"Webcam: {webCam.name} is front facing? {webCam.isFrontFacing}");
			if (webCam.isFrontFacing) activeCamera = new WebCamTexture(webCam.name, Screen.width, Screen.height);
		}

		if (activeCamera == null)
		{
			Debug.Log($"Webcam is null. Setting to {WebCamTexture.devices[0].name}");
			activeCamera = new WebCamTexture(WebCamTexture.devices[0].name, Screen.width, Screen.height);
		}

		cameraAvailable = true;
		activeCamera.Play();
		background.texture = activeCamera;
	}

	private void Update()
	{
		if (cameraAvailable && !evaluating)
		{
			fit.aspectRatio = (float)activeCamera.width / (float)activeCamera.height;
			background.rectTransform.localScale = new Vector3(1f, (activeCamera.videoVerticallyMirrored ? -1f : 1f) * flip, 1f);
			background.rectTransform.localEulerAngles = new Vector3(0f, 0f, -activeCamera.videoRotationAngle);
		}
	}

	private IEnumerator CycleCams()
	{
		WebCamDevice[] d = WebCamTexture.devices;
		if (d.Length <= 1)
		{
			Debug.Log("only one.");
			if (d.Length == 1) Debug.Log("Name is " + d[0].name);
			yield break;
		}

		Debug.Log("0 " + d[0].name);
		Debug.Log("1 " + d[1].name);

		if (activeCamera.deviceName == d[0].name)
		{
			activeCamera.Stop();
			yield return new WaitForSeconds(.1f);
			activeCamera.deviceName = d[1].name;
			yield return new WaitForSeconds(.1f);
			activeCamera.Play();

			Debug.Log("\"" + activeCamera.deviceName + "\"");
			SetFlipMode(d[1]);
			yield break;
		}
		else if (activeCamera.deviceName == d[1].name)
		{
			activeCamera.Stop();
			yield return new WaitForSeconds(.1f);
			activeCamera.deviceName = d[0].name;
			yield return new WaitForSeconds(.1f);
			activeCamera.Play();

			Debug.Log("\"" + activeCamera.deviceName + "\"");
			SetFlipMode(d[0]);
			yield break;
		}
	}

	private void SetFlipMode(WebCamDevice device)
	{
		if (device.isFrontFacing) flip = -1;
		else flip = 1;
	}

	public void SwitchCameras()
	{
		StartCoroutine(CycleCams());
	}

	public void Snap()
	{
		activeCamera.Pause();
		cameraElements.SetActive(false);
		evaluationElements.SetActive(true);
		evaluating = true;
	}

	public void Reject()
	{
		activeCamera.Play();
		cameraElements.SetActive(true);
		evaluationElements.SetActive(false);
		evaluating = false;
	}

	public void Accept()
	{
		bubbleFaceMaterial.mainTexture = background.texture;
		SceneManager.LoadScene("MainScene");
	}
}
