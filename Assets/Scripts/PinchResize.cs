using UnityEngine;

public class PinchResize : MonoBehaviour
{
	[SerializeField] private float scalingAmmount = 30f;

	private float previousDistance = 0f, currentDistance = 0f, scalingFactor;

	private void Update()
	{
		Touch[] touches = Input.touches;
		if (touches.Length == 2)
		{
			Touch primaryTouch = touches[0];
			Touch secondaryTouch = touches[1];
			scalingFactor = GetScalingFactor();
			Debug.Log(scalingFactor);

			currentDistance = Vector2.Distance(primaryTouch.position, secondaryTouch.position);

			if (currentDistance > previousDistance) // Aumentando
			{
				transform.localScale = transform.localScale * scalingFactor;
			}
			else if (currentDistance < previousDistance) // Diminuindo
			{
				transform.localScale = transform.localScale * (1 / scalingFactor);
			}
			previousDistance = currentDistance;
		}
	}

	private float GetScalingFactor()
	{
		float sf = Mathf.Clamp(scalingAmmount / transform.localScale.magnitude, 5, scalingAmmount);
		return 1f + (sf / 100f);
	}
}
