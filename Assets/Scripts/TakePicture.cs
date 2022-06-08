using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TakePicture : MonoBehaviour
{
	private bool cameraAvailable;
	private bool evaluating = false;
	private int activeCameraNumber;
	private Texture defaultBackground;

	private WebCamTexture backCamera;
	private WebCamTexture frontCamera;
	private WebCamTexture activeCamera;

	[SerializeField] private RawImage background;
	[SerializeField] private AspectRatioFitter fit;
	[SerializeField] private Button switchCameraButton;

	[SerializeField] private GameObject cameraElements;
	[SerializeField] private GameObject evaluationElements;

	[SerializeField] private Material bubbleFaceMaterial;

	private void Start()
	{
		defaultBackground = background.texture;
		cameraElements.SetActive(true);
		evaluationElements.SetActive(false);

		if (WebCamTexture.devices.Length == 0)
		{
			Debug.Log("No cameras?");
			cameraAvailable = false;
			return;
		}

		foreach (WebCamDevice webCam in WebCamTexture.devices)
		{
			if (webCam.isFrontFacing) frontCamera = new WebCamTexture(webCam.name, Screen.width, Screen.height);
			else backCamera = new WebCamTexture(webCam.name, Screen.width, Screen.height);
		}

		if (backCamera == null || frontCamera == null)
		{
			Debug.Log("Deactivating switch camera button");
			switchCameraButton.interactable = false;
			if (backCamera == null && frontCamera == null) return;
		}

		if (backCamera != null)
		{
			activeCamera = backCamera;
			activeCameraNumber = 1;
		}
		else
		{
			activeCamera = frontCamera;
			activeCameraNumber = 2;
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
			background.rectTransform.localScale = new Vector3(1f, activeCamera.videoVerticallyMirrored ? -1f : 1f, 1f);
			background.rectTransform.localEulerAngles = new Vector3(0f, 0f, -activeCamera.videoRotationAngle);
		}
	}

	public void SwitchCameras()
	{
		if (activeCameraNumber == 1)
		{
			activeCamera = frontCamera;
			activeCameraNumber = 2;
		}
		else
		{
			activeCamera = backCamera;
			activeCameraNumber = 1;
		}
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
