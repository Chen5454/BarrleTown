using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhases
{
    Day,
    Night,
    talk,
    Vote
}
public class GameManager : MonoBehaviour
{
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
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        GameTimers();



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
    }

    public void SwitchGamePhases()
    {
        switch (gamePhase)
        {
            case GamePhases.Day: //switches to night
                gamePhase = GamePhases.Night;
                timer = nightTime;
                Debug.Log("Switching to Night");
                break;
            case GamePhases.Night://switches to talk
                gamePhase = GamePhases.talk;
                timer = waitForVoteTime;
                Debug.Log("Switching to Talk");
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
        //let players talk in chat, move players physically to the campfire(or vote site) each in their own seat
    }

    public void StartVotePhase()
    {
        //let them vote
    }
    #endregion

}
