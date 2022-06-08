using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
	[SerializeField] private GameObject UIElements;
	[SerializeField] private GameObject planeFinder;
	[SerializeField] private GameObject groundPlaneStage;
	[SerializeField] private GameObject imageTarget;
	[SerializeField] private Image markerToggle;

	private GameObject activeBubble;
	private GameObject planeBubble;
	private GameObject markerBubble;

	private Color32 activeColor = new Color32(159, 250, 143, 255);
	private Color32 inactiveColor = new Color32(255, 255, 255, 255);
	private bool markerMode = false;

	private void Start()
	{
		planeBubble = groundPlaneStage.transform.GetChild(0).gameObject;
		markerBubble = imageTarget.transform.GetChild(0).gameObject;

		activeBubble = planeBubble;
		planeFinder.SetActive(true);
		groundPlaneStage.SetActive(true);
		imageTarget.SetActive(false);
	}

	public void Back()
	{
		SceneManager.LoadScene("TakePicture");
	}

	public void RotateY(float angle)
	{
		activeBubble.transform.Rotate(Vector3.up, angle, Space.World);
	}

	public void RotateX(float angle)
	{
		activeBubble.transform.Rotate(Vector3.left, angle, Space.World);
	}

	public void RotateZ(float angle)
	{
		activeBubble.transform.Rotate(Vector3.back, angle, Space.World);
	}

	public void ToggleMarkerMode()
	{
		markerMode = !markerMode;

		if (markerMode)
		{
			markerToggle.color = activeColor;

			activeBubble = markerBubble;
			planeFinder.SetActive(false);
			groundPlaneStage.SetActive(false);
			imageTarget.SetActive(true);
		}
		else
		{
			markerToggle.color = inactiveColor;

			activeBubble = planeBubble;
			planeFinder.SetActive(true);
			groundPlaneStage.SetActive(true);
			imageTarget.SetActive(false);
		}
	}

	public void SavePicture()
	{
		UIElements.SetActive(false);
		ScreenCapture.CaptureScreenshot("Screenshot.png");
		UIElements.SetActive(true);
	}
}
