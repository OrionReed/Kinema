using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class _DebugConsole : MonoBehaviour
{
    public List<DebugCommand> Commands;
}
[Serializable]
public struct DebugCommand
{
    [SerializeField]
    private string commandString;
    [SerializeField]
    private UnityEvent commandEvent;
}
