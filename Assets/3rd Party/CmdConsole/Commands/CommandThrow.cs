using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CmdConsole;
public class CommandThrow : BaseCommand
{
    public CommandThrow()
    {
        Name = "throw";
        Description = "Throws the player in a direction";
        DefaultType = typeof(bool);
    }

    public override CmdMessage ProcessDefault()
    {
        return new CmdMessage();
    }

    public override CmdMessage ProcessArgs(string parameter, List<string> args)
    {
        Player_Movement player = GameObject.FindObjectOfType<Player_Movement>();
        if (parameter.CanChangeType(typeof(float)))
        {
            float force = parameter.ChangeType<float>();
            if (args?.Count > 0)
            {
                Vector3 direction = new Vector3(
                    args[0].ChangeType<float>(),
                    args[1].ChangeType<float>(),
                    args[2].ChangeType<float>());
                player.Throw(force, direction);
            }
            else
            {
                player.Throw(force);
            }
        }
        return new CmdMessage();
    }
}
