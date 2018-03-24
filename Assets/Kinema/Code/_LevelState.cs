using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System;

public class _LevelState : MonoBehaviour
{
    public static event Action OnInit = delegate { };
    public static event Action OnIntro = delegate { };
    public static event Action OnPlay = delegate { };
    public static event Action OnWin = delegate { };
    public static event Action OnLose = delegate { };
    public static event Action OnDead = delegate { };
    public static event Action OnWatchReplay = delegate { };

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
    public static States currentState { get; private set; }

    private void StateIntro() { stateMachine.ChangeState(States.Intro); OnIntro(); Debug.Log("State: Intro"); }
    private void StatePlay() { stateMachine.ChangeState(States.Play); OnPlay(); Debug.Log("State: Play"); }
    private void StateWin() { stateMachine.ChangeState(States.Win); OnWin(); Debug.Log("State: Win"); }
    private void StateLose() { stateMachine.ChangeState(States.Lose); OnLose(); Debug.Log("State: Lose"); }
    private void StateDead() { stateMachine.ChangeState(States.Dead); OnDead(); Debug.Log("State: Dead"); }
    private void StateWatchReplay() { stateMachine.ChangeState(States.WatchReplay); OnWatchReplay(); Debug.Log("State: WatchReplay"); }

    private StateMachine<States> stateMachine;
    private void Awake() { stateMachine = StateMachine<States>.Initialize(this, States.Init); }
    private void Start()
    {
        _Input.OnKeyRestartScene += StatePlay;
        FindObjectOfType<Node_Health>().OnPlayerDeath += StateDead;
        StateIntro();
    }
    private void Update() { currentState = stateMachine.State; }

    // STATES

    private void Init_Enter()
    {
        Debug.Log("Initializing State Machine");
        OnInit();
    }
    private IEnumerator Intro_Enter()
    {

        Debug.Log("Game Starting in 2");
        yield return new WaitForSeconds(1);

        Debug.Log("Game Starting in 1");
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
