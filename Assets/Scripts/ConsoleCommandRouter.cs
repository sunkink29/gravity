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
		repo.RegisterCommand("noclip", NoClip);
        repo.RegisterCommand("enableCheats", EnableCheats);
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
}