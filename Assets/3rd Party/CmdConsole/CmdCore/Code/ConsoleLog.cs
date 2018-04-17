using UnityEngine;
using System.Linq;
using TMPro;
using System.Collections.Generic;
using System.Text;

namespace CmdConsole
{
    public class ConsoleLog
    {
        public static ConsoleLog instance;
        private TMP_Text log;
        private CmdStylePalette style;
        private List<CmdMessage> internalMessageLog = new List<CmdMessage>();
        private List<CmdMessage> internalSystemMessageLog = new List<CmdMessage>();

        public ConsoleLog(TMP_Text logText, CmdStylePalette stylePalette)
        {
            instance = this;
            log = logText;
            style = stylePalette;
        }
        public void ClearLog(bool clearInternalLog = false)
        {
            log.text = "";
            if (clearInternalLog)
                internalMessageLog.Clear();
        }

        public void LogSystemMessage(CmdMessage message)
        {
            if (message != null && message.Lines.Any())
            {
                internalSystemMessageLog.Add(message);
                for (int i = 0; i < message.Lines.Count; i++)
                {
                    log.text +=
                        "\n" +
                        ColorString("[CmdConsole] ", style.System) +
                        FormatLine(message.Lines[i]);
                }
            }
        }

        public void LogMessage(CMLine message) => LogMessage(new CmdMessage(message));

        public void LogMessage(CmdMessage consoleMessage)
        {
            if (consoleMessage != null && consoleMessage.Lines.Any())
            {
                internalMessageLog.Add(consoleMessage);
                for (int i = 0; i < consoleMessage.Lines.Count; i++)
                {
                    string prefix = "";
                    switch (consoleMessage.Lines[i].Type)
                    {
                        case CMLineType.Default:
                            if (string.IsNullOrWhiteSpace(style.PrefixDefault) == false)
                                prefix = style.PrefixDefault.Trim() + " ";
                            break;
                        case CMLineType.Warning:
                            if (string.IsNullOrWhiteSpace(style.PrefixWarning) == false)
                                prefix = style.PrefixWarning.Trim() + " ";
                            break;
                    }
                    log.text +=
                        "\n" +
                        ColorString(prefix, style.System) +
                        FormatLine(consoleMessage.Lines[i]);
                }
            }
        }

        private string FormatLine(CMLine line)
        {
            StringBuilder formatString = new StringBuilder();
            foreach (Pair<string, CMStyle> segment in line)
            {
                switch (segment.Value2)
                {
                    case CMStyle.Default:
                        formatString.Append(ColorString(segment.Value1, style.Default));
                        break;
                    case CMStyle.Emphasis:
                        formatString.Append(ColorString(segment.Value1, style.Emphasis));
                        break;
                    case CMStyle.Warning:
                        formatString.Append(ColorString(segment.Value1, style.Warning));
                        break;
                    case CMStyle.Confirm:
                        formatString.Append(ColorString(segment.Value1, style.Confirm));
                        break;
                    case CMStyle.Object:
                        formatString.Append(ColorString(segment.Value1, style.Object));
                        break;
                    case CMStyle.System:
                        formatString.Append(ColorString(segment.Value1, style.System));
                        break;
                }
            }
            return formatString.ToString();
        }
        private string ColorString(string stringToColor, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + stringToColor + "</color>";
        }
    }
}
