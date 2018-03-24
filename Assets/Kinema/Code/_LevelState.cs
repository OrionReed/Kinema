using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System;

public class _LevelState : MonoBehaviour
{
    public static event Action OnIntro = delegate { };
    public static event Action OnPlay = delegate { };
    public static event Action OnWin = delegate { };
    public static event Action OnLose = delegate { };
    public static event Action OnDead = delegate { };
    public static event Action OnWatchReplay = delegate { };
    public static States currentState { get; private set; }
    public enum States
    {
        Init,
        Intro,
        Play,
        Win,
        Lose,
        Dead,
        WatchReplay
    }

    private StateMachine<States> stateMachine;

    // Handy Methods to change state, call events and log change at once.
    private void StateIntro() { stateMachine.ChangeState(States.Intro); OnIntro(); Debug.Log("State: Intro"); }
    private void StatePlay() { stateMachine.ChangeState(States.Play); OnPlay(); Debug.Log("State: Play"); }
    private void StateWin() { stateMachine.ChangeState(States.Win); OnWin(); Debug.Log("State: Win"); }
    private void StateLose() { stateMachine.ChangeState(States.Lose); OnLose(); Debug.Log("State: Lose"); }
    private void StateDead() { stateMachine.ChangeState(States.Dead); OnDead(); Debug.Log("State: Dead"); }
    private void StateWatchReplay() { stateMachine.ChangeState(States.WatchReplay); OnWatchReplay(); Debug.Log("State: WatchReplay"); }

    private void Awake() { stateMachine = StateMachine<States>.Initialize(this, States.Init); }
    private void Start()
    {
        _Input.OnKeyRestartScene += StatePlay;
        FindObjectOfType<Player_Health>().OnPlayerDeath += StateDead;

        StateIntro();
    }
    private void Update() { currentState = stateMachine.State; }

    // STATES

    private void Init_Enter() { Debug.Log("Initializing State Machine"); }
    private IEnumerator Intro_Enter()
    {

        Debug.Log("Starting in 2");
        yield return new WaitForSeconds(1);

        Debug.Log("Starting in 1");
        yield return new WaitForSeconds(1);

        StatePlay();
    }
    private void Play_Enter()
    {
    }
    private void Win_Enter()
    {
    }
    private void Lose_Enter()
    {
    }
    private void Dead_Enter()
    {
    }
    private void WatchReplay_Enter()
    {
    }

}
