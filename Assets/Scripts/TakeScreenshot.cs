using System.Collections;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
	[SerializeField] private GameObject[] hideElements;
	private Sprite screenShot;

	private IEnumerator Screenshot()
	{
		Debug.Log("Wait till end of frame");
		yield return new WaitForEndOfFrame();
		Debug.Log("Taking screenshot");

		Texture2D texture = new Texture2D(Screen.width, Screen.width, TextureFormat.RGB24, false);
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();

		string fileName = "cabecolha_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
		NativeGallery.SaveImageToGallery(texture, "Cabeçolha", fileName);

		foreach (GameObject element in hideElements) element.SetActive(true);
	}

	public void Snap()
	{
		foreach (GameObject element in hideElements) element.SetActive(false);
		StartCoroutine(Screenshot());
	}
}
