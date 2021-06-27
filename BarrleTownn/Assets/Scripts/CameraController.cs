using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector2 votingArea;
	bool isAtVotingPhase;

	public Animator firePlaceAnim;
	public Animator cameraAnim;
	public VotingUI votingUI;
	public Camera cam;
	public Transform firePlace;
	public NotificationSystem notifictaion;
	[SerializeField] GameTimeUI gameTimeUI;
	// Update is called once per frame
	void Update()
	{
		if (GameManager.getInstance != null)
			if (GameManager.getInstance.player != null && !isAtVotingPhase)
			{
				Vector3 cameraPos = new Vector3(GameManager.getInstance.player.transform.position.x, GameManager.getInstance.player.transform.position.y, -10f);

				transform.position = cameraPos;
			}
	}

	public void setCameraToGamePhase(bool _VotingPhase)
	{
		isAtVotingPhase = _VotingPhase;
		if (isAtVotingPhase)
		{
			Vector3 cameraPos = new Vector3(votingArea.x, votingArea.y, -10f);
			transform.position = cameraPos;
			cameraAnim.enabled = true;
		}
		else
		{
			cameraAnim.enabled = false;
			cam.orthographicSize = 5;
		}
	}


	public void AnimActivateFire()
	{
		firePlaceAnim.SetBool("IsOnFire", true);
	}

	public void AnimDeActivateFire()
	{
		firePlaceAnim.SetBool("IsOnFire", false);
	}
	public void SetCameraToKillVoted()
	{
		cameraAnim.SetBool("ShowVote", true);
	}

	public void AnimdisableUI()
	{
		if (votingUI == null)
			votingUI = FindObjectOfType<VotingUI>();

		votingUI.SetVotingAmountActive(false);
		votingUI.SetVotingButtons(false);

		AnimNotificationVoted();

	}
	public void AnimShowVoteAmount()
	{
		if (votingUI == null)
			votingUI = FindObjectOfType<VotingUI>();

		votingUI.SetVotingAmountActive(true);
		//votingUI.voting.CheckVotes();
		gameTimeUI.isTimerShown = false;
	}

	public void NoOneWasVoted()
	{
		cameraAnim.SetBool("NoOneVoted", true);
	}
	public void putVotedOnFire()
	{
		GameManager.getInstance.PutVotedPlayerOnFire(firePlace.position, GameManager.getInstance.killedPlayer);
	}

	public void ChangePhaseToDay()
	{
		GameManager.getInstance.CheckWinCondition();
		GameManager.getInstance.ChangeToDay();
	}
	public void AnimKillVotedPlayer()
	{
		votingUI.voting.KillVotedPlayer(GameManager.getInstance.killedPlayer);
		AnimWasWerewolfOrNot();
	}




	#region Notification
	public void AnimNotificationNoOneVoted()
	{
		string text = "No one was voted";
		notifictaion.ShowText(text);
	}
	public void AnimNotificationVoted()
	{
		if (GameManager.getInstance.killedPlayer == -1)
			return;
		string text = GameManager.getInstance.playersList[GameManager.getInstance.killedPlayer].playerName + " was voted to burn";
		notifictaion.ShowText(text);
	}
	public void AnimWasWerewolfOrNot()
	{
		if (GameManager.getInstance.playersList[GameManager.getInstance.killedPlayer].isWerewolf)
		{
			string text = GameManager.getInstance.playersList[GameManager.getInstance.killedPlayer].playerName + " were the werewolf";
			notifictaion.ShowText(text);
		}
		else
		{
			string text = GameManager.getInstance.playersList[GameManager.getInstance.killedPlayer].playerName + " were not the werewolf";
			notifictaion.ShowText(text);
		}
		
	}

	#endregion

}
