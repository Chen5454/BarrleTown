using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

#region Elor's work
public enum GamePhases
{
    Day,
    Night,
    talk,
    Vote
}
public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] FieldOfView fov;
    [Header("Phases")]
    public List<GameObject> playersList;
    public VotePhase votePhase;
    bool isGameActive = false;
    [Header("Current Phase")]
    [SerializeField]
    GamePhases gamePhase;

    [Header("Timers")]
    [SerializeField]
    float dayTime;
    [SerializeField]
    float nightTime;
    [SerializeField]
    float voteTime;
    [SerializeField]
    float waitForVoteTime; // this will have to be lower then vote time, how long players will have to wait untill they can vote someone

    [SerializeField]
    private float timer;
    [SerializeField]
    bool canVote;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Game Manger is now On");
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            GameTimers();
        }
       



    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 3)
        {
            InitGame();

        }
    }

    public void GameTimers()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            SwitchGamePhases();
        }
    }

    public void InitGame()
    {
        timer = dayTime;
        gamePhase = GamePhases.Day;
        isGameActive = true;
    }

    public void SwitchGamePhases()
    {
        switch (gamePhase)
        {
            case GamePhases.Day: //switches to night
                gamePhase = GamePhases.Night;
                timer = nightTime;
                Debug.Log("Switching to Night");
                fov.SetNightFOV(true);
                break;
            case GamePhases.Night://switches to talk
                gamePhase = GamePhases.talk;
                timer = waitForVoteTime;
                StartTalkPhase();
                Debug.Log("Switching to Talk");
                fov.SetDayFOV();
                break;
            case GamePhases.talk://switches to Vote

                gamePhase = GamePhases.Vote;
                timer = voteTime;
                canVote = true;
                Debug.Log("Players can vote");
                break;
            case GamePhases.Vote: //switches to Day
                canVote = false;
                gamePhase = GamePhases.Day;
                timer = dayTime;
                Debug.Log("Switching to Day");
                break;
            default:
                break;
        }
    }

    #region Day_Region
    
    public void StartDayPhase()
    {
        //players see normal, reset barrel/shop, ETC
    }

    #endregion

    #region Night_Region

    public void StartNightPhase()
    {
        //werewolf transforms, villagers visions get thinner, ETC
    }

    #endregion

    #region Talk_And_Vote_Region

    public void StartTalkPhase()
    {
        votePhase.SetPlayersAtTheirVotingSpots(playersList);
        //disable players movement


        //let players talk in chat, move players physically to the campfire(or vote site) each in their own seat
    }

    public void StartVotePhase()
    {
        //let them vote
    }
    #endregion

    #region Afik's Work

    #region Photon Callbacks


    /// Called when the local player left the room. We need to load the launcher scene.
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }



    #endregion

    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion

    #region Private Methods


    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Lobby");
    }


    #endregion









    #endregion



}
[Serializable]
public class VotePhase
{
    [SerializeField]
    private Transform[] playerVoteSpots;

    public void SetPlayersAtTheirVotingSpots(List<GameObject> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = playerVoteSpots[i].position;
            //player will not have the ability to move
        }
    }
}

[Serializable]
public class NightPhase
{

}

[Serializable]
public class DayPhase
{

}
# endregion


