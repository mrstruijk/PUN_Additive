using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;


namespace mrstruijk.PUN
{
	[RequireComponent(typeof(TMP_InputField))]
	public class UserNameInputSetter : MonoBehaviour
	{
		private const string userNamePrefKey = "UserName";


		private void Start ()
		{
			SetDefaultName();
		}


		private void SetDefaultName()
		{
			string defaultName = string.Empty;
			var _inputField = GetComponent<InputField>();

			if (_inputField != null)
			{
				if (PlayerPrefs.HasKey(userNamePrefKey))
				{
					defaultName = PlayerPrefs.GetString(userNamePrefKey);
					_inputField.text = defaultName;
				}
			}

			PhotonNetwork.NickName = defaultName;
		}


		/// <summary>
		/// Sets the name of the player, and saves it in the PlayerPrefs for future sessions.
		/// Will be called dynamically fom UI
		/// </summary>
		/// <param name="name">The name of the Player</param>
		public void SetUserName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				Debug.Log("User name cannot be empty");
				return;
			}
			PhotonNetwork.NickName = name;

			PlayerPrefs.SetString(userNamePrefKey, name);
		}
	}
}
