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
    public static States CurrentState { get; private set; }
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

    // Handy Methods to change state & call events at once.
    private void StateIntro() { stateMachine.ChangeState(States.Intro); OnIntro(); }
    private void StatePlay() { stateMachine.ChangeState(States.Play); OnPlay(); }
    private void StateWin() { stateMachine.ChangeState(States.Win); OnWin(); }
    private void StateLose() { stateMachine.ChangeState(States.Lose); OnLose(); }
    private void StateDead() { stateMachine.ChangeState(States.Dead); OnDead(); }
    private void StateWatchReplay() { stateMachine.ChangeState(States.WatchReplay); OnWatchReplay(); }

    private void Awake() { stateMachine = StateMachine<States>.Initialize(this, States.Init); }
    private void Start()
    {
        _Input.OnKeyResetScene += StatePlay;
        FindObjectOfType<PlayerHealth_Death>().OnPlayerDeath += StateDead;

        StateIntro();
    }
    private void Update() { CurrentState = stateMachine.State; }

    // STATES

    private void Init_Enter() { }
    private IEnumerator Intro_Enter()
    {
        Debug.Log("Starting...");
        yield return new WaitForSeconds(0);
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
