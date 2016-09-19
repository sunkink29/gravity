using UnityEngine;
using System.Collections;

public class ConsoleCommandRouter : MonoBehaviour {

	bool noClipEnabled = false;
    bool cheatsEnabled = false;

	void Start () {
		var repo = ConsoleCommandsRepository.Instance;
        noClipEnabled = FirstPersonScript.player.noClipEnabled;
        cheatsEnabled = FirstPersonScript.player.cheatsEnabled;
//		repo.RegisterCommand("save", Save);
		repo.RegisterCommand ("noclip", NoClip);
        repo.RegisterCommand ("enableCheats", EnableCheats);
		repo.RegisterCommand ("changeRoomLights", ChangeRoomLights);
	}
//
//	public string Save(params string[] args) {
//		var filename = args[0];
//		new LevelSaver().Save(filename);
//		return "Saved to " + filename;
//	}

	public string NoClip(params string[] args) {
        //		FirstPersonScript.player.gravityOnNormals.enableGravity = !noClipEnabled;
        //		FirstPersonScript.player.GetComponent<CapsuleCollider> ().enabled = !noClipEnabled;
        //		FirstPersonScript.player.noClipEnabled = noClipEnabled;
        if (cheatsEnabled)
        {
            if (args.Length >= 1 && args[0] == "autoRotate")
            {
                FirstPersonScript.player.toggleNoClip(true);
            }
            else
            {
                FirstPersonScript.player.toggleNoClip(false);
            }
        }
        noClipEnabled = FirstPersonScript.player.noClipEnabled;
		string noClipStatus;
        if (!cheatsEnabled) {
            noClipStatus = "Acess Denied";
        }
		if (noClipEnabled) {
			noClipStatus = "noclip enabled";
		} else {
			noClipStatus = "noclip disabled";
		}
		return noClipStatus;
	}

    public string EnableCheats(params string[] args)
    {
        FirstPersonScript.player.toggleCheats();
        cheatsEnabled = FirstPersonScript.player.cheatsEnabled;
        string cheatsStatus;
        if (cheatsEnabled)
        {
            cheatsStatus = "enabled";
        } else
        {
            cheatsStatus = "disabled";
        }
        return "cheats " + cheatsStatus;
    }

	public string ChangeRoomLights(params string[] args) {
		if (!cheatsEnabled) {
			return "Acess Denied";
		}
		RoomLightsController room;
		for (int i = 0; i < RoomLightsController.AllRooms.Count; i++) {
			room = RoomLightsController.AllRooms [i];
			if (room.roomName.ToLower() == (args [0].ToLower())) {
				if (args[1].ToLower().Contains("on")) {
					room.powerOn ();
					return room.roomName + " room turned on";
				} else if (args[1].ToLower().Contains("off")) {
					room.powerOff ();
					return room.roomName + " room turned off";
				}
				return "specify whether to turn the room Off or On";
			}
		}
		return "specify the room to change the lights";
	}
}