using Photon.Pun;
using UnityEngine;


namespace mrstruijk.PUN
{
	public class UserAnimatorManager : MonoBehaviourPun
	{
		[SerializeField] private float directionDampTime = 0.25f;

		private Animator animator;
		private float hor;
		private float vert;

		private static readonly int Speed = Animator.StringToHash("Speed");
		private static readonly int Jump1 = Animator.StringToHash("Jump");
		private static readonly int Direction = Animator.StringToHash("Direction");

		private XRIDefaultInputActions input;


		private void Awake()
		{
			input = new XRIDefaultInputActions();
		}


		private void OnEnable()
		{
			input.Enable();

			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			{
				return;
			}
			input.Player.Jump.performed += context => Jump();
			input.Player.Move.performed += context => SetAnimator(context.ReadValue<Vector2>());
		}



		private void Start()
		{
			animator = GetComponent<Animator>();

			if (!animator)
			{
				Debug.LogError("User Animator is missing! Help.");
			}
		}


		private void Jump()
		{
			var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			if (!stateInfo.IsName("Base Layer.Run"))
			{
				return;
			}

			animator.SetTrigger(Jump1);
		}



		private void SetAnimator(Vector2 direction)
		{
			if (direction.y < 0)
			{
				vert = 0;
			}
			else
			{
				vert = direction.y;
			}

			hor = direction.x;

			animator.SetFloat(Speed, Mathf.Abs(hor) + Mathf.Abs(vert)); // We've squared both inputs. Why? So that it's always a positive absolute value as well as adding some easing. Nice subtle trick right here. You could use Mathf.Abs() too, that would work fine.
			animator.SetFloat(Direction, hor, directionDampTime, Time.deltaTime); // Damping time: it's how long it will take to reach the desired value
		}


		private void OnDisable()
		{
			input.Disable();
		}
	}
}

