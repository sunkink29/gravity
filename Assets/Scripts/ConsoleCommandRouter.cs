using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public enum CommandOptions { enable, disable, change }

public class ConsoleCommandRouter : MonoBehaviour
{

    bool cheatsEnabled = false;
    string[] enableDisableCommandList = new string[] { "cheats", "noclip", "interactPower" };
    string[] posibleTargets = new string[] {"player", "instanceId", "name", "rayCast" };
    Func<bool, string[], string>[] enableDisableCommandDelegateList;

    void Start()
    {
        enableDisableCommandDelegateList = new Func<bool, string[], string>[] { enableCheats, enableNoClip, enableInteractPower };
        var repo = ConsoleCommandsRepository.Instance;
        cheatsEnabled = FirstPersonScript.player.cheatsEnabled;
        // repo.RegisterCommand("save", Save);
        repo.RegisterCommand("enable", enableCommand);
        repo.RegisterCommand("disable", disableCommand);
        repo.RegisterCommand("change", changeCommand);
    }
    //
    //	public string Save(params string[] args) {
    //		var filename = args[0];
    //		new LevelSaver().Save(filename);
    //		return "Saved to " + filename;
    //	}

    public string enableCommand(params string[] args) {
        return callCommand(CommandOptions.enable, args);
    }

    public string disableCommand(params string[] args) {
        return callCommand(CommandOptions.disable, args);
    }

    public string changeCommand(params string[] args) {
        return callCommand(CommandOptions.change, args);
    }

    public string callCommand(CommandOptions commandOption, string[] args) {
        int commandIndex = 0;

        string[] commandArgs = new string[args.Length - 1];
        commandArgs = args.Skip(1).ToArray();

        if (commandOption == CommandOptions.change) {
            commandIndex = getCommandIndexFromString(posibleTargets, args[0]);
            if (commandIndex == -1) {
                if (!FirstPersonScript.player.hasProperty(args[0])) {
                    return "Unknown target or player property";
                }
                commandArgs = new string[args.Length - 1];
                commandArgs = args.Skip(1).ToArray();
                return changeProperty(FirstPersonScript.player, args[0], commandArgs);
            }
            string property;

            if (args[0].Equals("player") || args[0].Equals("rayCast")) {
                commandArgs = new string[args.Length - 2];
                commandArgs = args.Skip(2).ToArray();
                property = args[1];
            } else {
                commandArgs = new string[args.Length - 3];
                commandArgs = args.Skip(3).ToArray();
                property = args[2];
            }
            FindPropertys target = null;
            object[] targetObject = getTargetFromString(args[0],args[1]);
            if (targetObject[0] == null) {
                return (string) targetObject[1];
            }
            target = (FindPropertys) targetObject[0];

            if (target != null){
                return changeProperty(target,property,commandArgs);
            }
            
        } else {
            commandIndex = getCommandIndexFromString(enableDisableCommandList, args[0]);
            if (commandIndex == -1) {
                return "invalid command";
            }

            bool enable = false;
            if (commandOption == CommandOptions.enable) {
                enable = true;
            }

            return enableDisableCommandDelegateList[commandIndex](enable, commandArgs);
        }

        return "";
    }

    int getCommandIndexFromString(string[] commands,string command){
        int commandIndex = -1;
        for (int i = 0; i < commands.Length; i++) {
            if (command.Equals(commands[i])) {
                commandIndex = i;
                break;
            }
        }
        return commandIndex;
    }

    object[] getTargetFromString(string targetType, string stringTarget) {
        FindPropertys target = null;
        string error = null;
        int commandIndex = getCommandIndexFromString(posibleTargets, targetType);
        switch (commandIndex) {
                case 0:
                    target = FirstPersonScript.player;
                    break;
                case 1:
#if UNITY_EDITOR
                    target = (FindPropertys)UnityEditor.EditorUtility.InstanceIDToObject(int.Parse(stringTarget));
                    break;
#else
                    error = "Can not Find innstance id inside a standalone build";
                    break;
#endif
                case 2:
                    target = GameObject.Find(stringTarget).GetComponent<FindPropertys>();
                    break;
               case 3:
                    error = "not coded yet";
                    break;
            }
            return new object[] {target,error};
    }

    public string enableCheats(bool enable, string[] args)
    {
        FirstPersonScript.player.toggleCheats(enable);
        cheatsEnabled = FirstPersonScript.player.cheatsEnabled;
        string cheatsStatus;
        if (cheatsEnabled)
        {
            cheatsStatus = "enabled";
        }
        else
        {
            cheatsStatus = "disabled";
        }
        return "cheats " + cheatsStatus;
    }

    public string enableNoClip(bool enable, string[] args)
    {
        if (cheatsEnabled)
        {
            if (args.Length >= 2 && args[1] == "autoRotate")
            {
                FirstPersonScript.player.toggleNoClip(enable, true);
            }
            else
            {
                FirstPersonScript.player.toggleNoClip(enable, false);
            }
        }
        string noClipStatus;
        if (!cheatsEnabled)
        {
            noClipStatus = "Acess Denied";
        }
        if (enable)
        {
            noClipStatus = "noclip enabled";
        }
        else
        {
            noClipStatus = "noclip disabled";
        }
        return noClipStatus;
    }

    public string changeRoomLights(params string[] args)
    {
        if (!cheatsEnabled)
        {
            return "Acess Denied";
        }
        RoomLightsController room;
        for (int i = 0; i < RoomLightsController.AllRooms.Count; i++)
        {
            room = RoomLightsController.AllRooms[i];
            if (room.roomName.ToLower() == (args[0].ToLower()))
            {
                if (args[1].ToLower().Contains("on"))
                {
                    room.changePower(new float[] { GetInstanceID(), 1 });
                    return room.roomName + " room turned on";
                }
                else if (args[1].ToLower().Contains("off"))
                {
                    room.changePower(new float[] { GetInstanceID(), 0 });
                    return room.roomName + " room turned off";
                }
                return "specify whether to turn the room Off or On";
            }
        }
        return "specify the room to change the lights";
    }

    public string enableInteractPower(bool enable, string[] args)
    {
        string returnValue = "";
        int addedArgsLength = 0;
        if (args.Length >= 2)
        {
            addedArgsLength = args.Length - 2;
        }
        string[] powerArgs = new string[2 + addedArgsLength];
        for (int i = 0; i < args.Length; i++) {
            powerArgs[i] = args[i];
        }
        if (args.Length >= 2)
        {
            powerArgs[0] = args[1];
        }
        if (args.Length >= 1)
        {
            powerArgs[1] = args[0];
        }
        if (args.Length == 0 && enable) {
            return "input Power argument";
        }
        if (args.Length <= 1) {
            powerArgs[0] = "0";
        }

        FirstPersonScript.player.enableDisableDebugingInteract(enable, DebugType.Power, powerArgs);
        returnValue = "Interact Power " + (enable ? "enabled" : "disabled") + " with the parameters:";
        for (int i = 0; i < powerArgs.Length; i++) {
            returnValue += " " + powerArgs[i] + ",";
        }
        return returnValue;
    }

    public string changeProperty (FindPropertys target, string property, string[] propertyValue) {
        if (target.hasProperty(property)) {
            target.changeProperty(property, propertyValue);
            return "Changed " + target.getName() + " " + property + " to " + propertyValue;
        }
        return "Could not find "  + property + " in " + target;
    }

    public static float[] convertStringArrayToFloat(string[] input) {
        float[] output = new float[input.Length];
        for (int i = 0; i < input.Length; i++) {
            output[i] = float.Parse(input[i]);
        }
        return output;
    }
}