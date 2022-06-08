using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
	[SerializeField] private GameObject UIElements;

	public void Back()
	{
		SceneManager.LoadScene("TakePicture");
	}

	public void SavePicture()
	{
		UIElements.SetActive(false);
		ScreenCapture.CaptureScreenshot("Screenshot.png");
		UIElements.SetActive(true);
	}
}
