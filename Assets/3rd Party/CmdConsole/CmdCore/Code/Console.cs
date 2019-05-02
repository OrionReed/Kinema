using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace CmdConsole
{
    public class Console : MonoBehaviour
    {
        public static Console instance { get; private set; }
        [SerializeField] private CmdStylePalette stylePalette;
        [SerializeField] private KeyCode autocomplete;
        [SerializeField] private KeyCode cycleOptionsUp;
        [SerializeField] private KeyCode cycleOptionsDown;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Text autocompleteText;
        [SerializeField] private TMP_Text logText;

        private List<ICommand> matchingCommands = new List<ICommand>();
        private List<IParam> matchingParameters = new List<IParam>();
        private string commandSearch = "";
        private string parameterSearch = "";
        private int targetCommand = 0;
        private int targetParameter = 0;
        private List<string> args = new List<string>();

        private ConsoleLog log;

        private void Start()
        {
            instance = this;
            if (inputField == null) Debug.LogError("CmdConsole is missing an input field.");
            else if (autocompleteText == null) Debug.LogError("CmdConsole is missing SuggestionText.");
            else if (logText == null) Debug.LogError("CmdConsole has no log.");
            else
            {
                log = new ConsoleLog(logText, stylePalette);
                log.ClearLog();
                inputField.onValueChanged.AddListener(OnTextInputChanged);
                inputField.onSubmit.AddListener(OnProcessCommand);
                ConsoleCommands.RegisterCommands();
                ConsoleCommands.SortCommands();

                log.LogSystemMessage(ConsoleCommands.ValidateCommands());
            }
        }
        private void Update()
        {
            if (Input.GetKeyUp(autocomplete))
                inputField.MoveTextEnd(false);
            if (Input.GetKeyDown(cycleOptionsUp))
                CycleCommand(1); CycleParam(1);
            if (Input.GetKeyDown(cycleOptionsDown))
                CycleCommand(-1); CycleParam(-1);
            if (Input.GetKeyDown(autocomplete))
                if (autocompleteText.text != "")
                    inputField.text = autocompleteText.text;
        }

        private void OnTextInputChanged(string input)
        {
            List<string> splitInput = Regex.Split(input, @"\W+").ToList();
            if (splitInput.Count > 0)
            {
                commandSearch = splitInput[0];
                if (splitInput.Count > 1)
                {
                    parameterSearch = splitInput[1];
                    if (splitInput.Count > 2)
                    {
                        splitInput.RemoveRange(0, 2);
                        args = splitInput;
                    }
                }
                else
                {
                    parameterSearch = "";
                }
            }
            else
            {
                commandSearch = "";
                parameterSearch = "";
            }
            ClampTargets();
            OnInputChanged();
        }

        private void OnInputChanged()
        {
            if (string.IsNullOrWhiteSpace(commandSearch) == false)
            {
                matchingCommands = SearchCommands();
                if (string.IsNullOrWhiteSpace(parameterSearch) == false && matchingParameters.Any())
                {
                    matchingParameters = SearchParameters();
                }
                else
                {
                    matchingParameters.Clear();
                }
            }
            else
            {
                matchingCommands.Clear();
                matchingParameters.Clear();
            }
            ClampTargets();
            UpdateSuggestion();
        }


        private void CycleCommand(int increment)
        {
            targetCommand += increment;
            ClampTargets();
            OnInputChanged();
        }
        private void CycleParam(int increment)
        {
            targetParameter += increment;
            ClampTargets();
            OnInputChanged();
        }

        private void ClampTargets()
        {
            targetCommand = Mathf.Clamp(targetCommand, 0, matchingCommands.Count - 1);
            targetParameter = Mathf.Clamp(targetParameter, 0, matchingParameters.Count - 1);
        }

        private void UpdateSuggestion()
        {
            if (matchingCommands.Any())
            {
                autocompleteText.text = matchingCommands[targetCommand].Name;
                if (matchingParameters.Any())
                {
                    autocompleteText.text += " " + matchingParameters[targetParameter].Name;
                }
            }
            else
            {
                autocompleteText.text = "";
            }
        }

        private List<ICommand> SearchCommands()
        {
            List<ICommand> matchingCommands = new List<ICommand>(ConsoleCommands.Commands);
            for (int i = matchingCommands.Count - 1; i >= 0; i--)
                if (matchingCommands[i].Name.StartsWith(commandSearch) == false)
                    matchingCommands.RemoveAt(i);

            return matchingCommands;
        }
        private List<IParam> SearchParameters()
        {
            List<IParam> matchingParameters = new List<IParam>(matchingCommands[targetCommand].Parameters);
            for (int i = matchingCommands[targetCommand].Parameters.Count - 1; i >= 0; i--)
                if (matchingCommands[targetCommand].Parameters[i].Name.StartsWith(parameterSearch) == false)
                    matchingParameters.RemoveAt(i);

            return matchingParameters;
        }
        private void OnProcessCommand(string rawInput)
        {
            if (matchingCommands.Any())
            {
                if (parameterSearch.Any())
                {
                    if (args.Any())
                    {
                        log.LogMessage(matchingCommands[targetCommand].ProcessArgs(parameterSearch, args));
                    }
                    else
                    {
                        log.LogMessage(matchingCommands[targetCommand].ProcessArgs(parameterSearch, null));
                    }
                }
                else
                {
                    log.LogMessage(matchingCommands[targetCommand].ProcessDefault());
                }
                inputField.text = "";
            }
            else if (commandSearch.Any())
            {
                log.LogMessage(new CMLine(CMLineType.Warning)
                    {
                        {commandSearch, CMStyle.Emphasis},
                        {" is not a valid command", CMStyle.Default}
                    }
                );
            }
        }
    }
}