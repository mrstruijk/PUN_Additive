using Photon.Pun;
using UnityEngine;
using Sirenix.OdinInspector;


namespace mrstruijk.PUN
{
	public class UserAvatarManager : MonoBehaviourPunCallbacks
	{
		[SerializeField] private bool autoRole = false;

		[Required] [ShowIf("autoRole")]
		public UserRole buildRole;

		[Required] [ShowIf("autoRole")]
		public UserRole editorRole;

		[Required] [HideIf("autoRole")]
		public UserRole role;



		public override void OnEnable()
		{
			SceneManagement.SceneHasBeenLoaded += OnLoadedScene;
		}


		private void Start()
		{
			AutoRole();
		}


		private void AutoRole()
		{
			if (autoRole == false)
			{
				return;
			}

			role = Application.isEditor ? editorRole : buildRole;
		}


		private void OnLoadedScene(string sceneName)
		{
			role.OnEnteredRoom();
		}


		public override void OnDisable()
		{
			SceneManagement.SceneHasBeenLoaded -= OnLoadedScene;
		}
	}
}
