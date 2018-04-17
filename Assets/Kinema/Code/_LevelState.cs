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
    public static LevelState CurrentState { get; private set; }

    private StateMachine<LevelState> stateMachine;

    // Handy Methods to change state & call events at once.
    private void StateIntro() { stateMachine.ChangeState(LevelState.Intro); OnIntro(); }
    private void StatePlay() { stateMachine.ChangeState(LevelState.Play); OnPlay(); }
    private void StateWin() { stateMachine.ChangeState(LevelState.Win); OnWin(); }
    private void StateLose() { stateMachine.ChangeState(LevelState.Lose); OnLose(); }
    private void StateDead() { stateMachine.ChangeState(LevelState.Dead); OnDead(); }
    private void StateWatchReplay() { stateMachine.ChangeState(LevelState.WatchReplay); OnWatchReplay(); }

    private void Awake() { stateMachine = StateMachine<LevelState>.Initialize(this, LevelState.Init); }
    private void Start()
    {
        _Input.OnKeyResetScene += StatePlay;
        FindObjectOfType<PlayerHealth_Death>().OnPlayerDeath += StateDead;

        StateIntro();
    }
    private void Update() { CurrentState = stateMachine.State; }

    // LevelState

    private void Init_Enter() { }
    private IEnumerator Intro_Enter()
    {
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

public enum LevelState
{
    Init,
    Intro,
    Play,
    Win,
    Lose,
    Dead,
    WatchReplay
}