using UnityEngine;



public class Tampering
{
	private readonly Transform transform;
	private readonly Vector3 originalScale;

	public Tampering(Transform transform)
	{
		this.transform = transform;
		originalScale = this.transform.localScale;
	}


	public void TamperScale(Vector3 newScale)
	{
		if (transform.localScale == originalScale)
		{
			transform.localScale = newScale;
		}
		else if (transform.localScale == newScale)
		{
			transform.localScale = originalScale;
		}
	}


	public void TamperPosition(Vector3 newPosition)
	{
		transform.position = newPosition;
	}
}
