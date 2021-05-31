using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector2 votingArea;
	bool isAtVotingPhase;
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
		}
	}

}
