using UnityEngine;


public class GameObjectActivitySwitch : MonoBehaviour
{
	[Tooltip("The GameObject which should be on at start")]
	public GameObject startEnabled;

	[Tooltip("The GameObject which should be off at start")]
	public GameObject startDisabled;


	public virtual void Start()
	{
		SetNormalState();
	}


	private void SetNormalState()
	{
		startEnabled.SetActive(true);
		startDisabled.SetActive(false);
	}


	protected void SetActivityManually(bool startActive, bool startInactive)
	{
		startDisabled.SetActive(startActive);
		startEnabled.SetActive(startInactive);
	}


	protected void SwitchEnabled()
	{
		startEnabled.SetActive(!startEnabled.activeSelf);
		startDisabled.SetActive(!startEnabled.activeSelf);
	}


	public virtual void OnDisable()
	{
		SetNormalState();
	}
}
