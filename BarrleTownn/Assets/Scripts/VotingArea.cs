using System.Collections.Generic;
using UnityEngine;
public class VotingArea : MonoBehaviour
{
	public VotingUI votingUI;
	public Transform[] playerPositions;

	public int[] voteAmount;



	public void MovePlayersToVotingArea(List<VillagerCharacter> _players)
	{
		GameManager.getInstance.ResetVotes();

		for (int i = 0; i < _players.Count; i++)
		{

			if (_players[i].currentHp > 0)
			{

				_players[i].canMove = false;
				//Debug.LogError("moving Character: " + _players[i].name);
				_players[i].rb2D.velocity = new Vector2(0, 0);
				if (_players[i].photonView.IsMine)
					_players[i].gameObject.transform.position = playerPositions[i].position;
			}
		}
	}
	public void ResetVotes()
	{
		voteAmount = new int[votingUI.playerVotingButtons.Length + 1];
	}

	public void VoteToPlayer(int playerIndex, int vote)
	{
		voteAmount[playerIndex] += vote;
	}

	public void PlayersCanMove()
	{
		for (int i = 0; i < GameManager.getInstance.playersList.Count; i++)
		{
			if (GameManager.getInstance.playersList[i].currentHp > 0)
			{
				GameManager.getInstance.playersList[i].canMove = true;
			}
		}

		GameManager.getInstance.ResetVotes();
	}

	public void CheckVotes()
	{
		int highestIndex = 0;
		int highestPoint = 0;
		bool hasTie = false;
		bool skip = false;
		for (int i = 0; i < voteAmount.Length; i++)
		{         
			// -1 < 0 
			if (voteAmount[i] != 0)
			{
				if(voteAmount[i] > voteAmount[highestIndex])
				{
					highestIndex = i;
					highestPoint = voteAmount[highestIndex];
				}
			}
		}

		//ignore the index who has the highest score in the loop, and checks if there is another score with the same amount
		for (int i = 0; i < voteAmount.Length; i++)
		{
			if (highestIndex != -1)
			{
				// i = 0, highestIndex = 2 , 0 != 2
				if (i != highestIndex)
				{
					if (voteAmount[i] == voteAmount[highestIndex])
					{
						hasTie = true;
					}
				}
			}
		}
		if (voteAmount[8] == highestPoint)
			skip = true;


		if (hasTie || highestPoint == 0 || skip)
		{
			Debug.Log("Tie! or No Votes");
		}
		else
		{
			KillVotedPlayer(highestIndex);
		}

	}

	public void KillVotedPlayer(int _indexPlayer)
	{
		GameManager.getInstance.KillVotedPlayer(_indexPlayer);
	}



	public void ShowVotingButtons(bool _show)
	{
		votingUI.SetVotingButtons(_show);
	}

}
