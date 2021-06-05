using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotingUI : MonoBehaviour
{
	public Button[] playerVotingButtons;
	public TextMeshProUGUI[] voteAmount;
	public bool[] isVotedFor;
	public VotingArea voting;

	[SerializeField]int votingIndex = -1;
	private void Awake()
	{




	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.B))
		//{
		//	SetVotingButtons(true);
		//}
		//if (Input.GetKeyDown(KeyCode.N))
		//{
		//	SetVotingButtons(false);
		//}
	}

	public void SetVotingButtons(bool _show)
	{
		if (GameManager.getInstance != null)
		{
			votingIndex = -1;
			if (_show)
			{
				for (int i = 0; i < playerVotingButtons.Length; i++)
				{
					if (i < GameManager.getInstance.playersList.Count)
						if (GameManager.getInstance.playersList[i].currentHp > 0)
							if (!GameManager.getInstance.playersList[i].photonView.IsMine)
								playerVotingButtons[i].gameObject.SetActive(_show);
				}
				isVotedFor = new bool[playerVotingButtons.Length];
				ChangeButtonText();
			}
			else
			{
				for (int i = 0; i < playerVotingButtons.Length; i++)
				{
					playerVotingButtons[i].gameObject.SetActive(_show);
				}
			}
		}
	}

	public void ChangeButtonText()
	{
		for (int i = 0; i < playerVotingButtons.Length; i++)
		{
			if (isVotedFor[i])
			{
				playerVotingButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Voted";
			}
			else
			{
				playerVotingButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Vote";
			}
		}
	}
	

	public void VoteButton(int buttonIndex)
	{
		isVotedFor = new bool[playerVotingButtons.Length];
		if (votingIndex != buttonIndex)
		{
			if (votingIndex != -1)
				RemoveVoteFromPlayer(votingIndex);

			votingIndex = buttonIndex;
			isVotedFor[votingIndex] = true;
			VoteToPlayer(votingIndex);
			ChangeButtonText();
		}else if(votingIndex == buttonIndex)
		{
			if (votingIndex != -1)
				RemoveVoteFromPlayer(votingIndex);

			isVotedFor[votingIndex] = false;
			ChangeButtonText();


			votingIndex = -1;
		}
	}
	public void VoteToPlayer(int index)
	{
		GameManager.getInstance.GetPlayerVotes(index, 1);
		//voting.VoteToPlayer(index, 1);
	}
	public void RemoveVoteFromPlayer(int index)
	{
		GameManager.getInstance.GetPlayerVotes(index, -1);
		//voting.VoteToPlayer(index, -1);
	}





}
