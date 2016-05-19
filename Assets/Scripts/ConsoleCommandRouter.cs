using UnityEngine;
using System.Collections;

public class ConsoleCommandRouter : MonoBehaviour {

	bool noClipEnabled = false;

	void Start () {
		var repo = ConsoleCommandsRepository.Instance;
//		repo.RegisterCommand("save", Save);
		repo.RegisterCommand("noclip", NoClip);
	}
//
//	public string Save(params string[] args) {
//		var filename = args[0];
//		new LevelSaver().Save(filename);
//		return "Saved to " + filename;
//	}

	public string NoClip(params string[] args) {
		noClipEnabled = !noClipEnabled;
//		FirstPersonScript.player.gravityOnNormals.enableGravity = !noClipEnabled;
//		FirstPersonScript.player.GetComponent<CapsuleCollider> ().enabled = !noClipEnabled;
//		FirstPersonScript.player.noClipEnabled = noClipEnabled;
		if (args.Length >= 1 && args [0] == "autoRotate") {
			FirstPersonScript.player.toggleNoClip (true);
		} else {
			FirstPersonScript.player.toggleNoClip (false);
		}
		string noClipStatus;
		if (noClipEnabled) {
			noClipStatus = "enabled";
		} else {
			noClipStatus = "disabled";
		}
		return "noclip " + noClipStatus;
	}
}