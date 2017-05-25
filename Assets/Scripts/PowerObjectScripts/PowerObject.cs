using UnityEngine;
using System.Collections;

public class PowerObject : MonoBehaviour{

	public bool enableOnStart = false;
	public PowerProviderPowerObject powerProvider;
	public virtual void Start () {
		if (powerProvider != null) {
			powerProvider.sendReference (this);
		}
		if (enableOnStart) {
			changePower(new float[] {GetInstanceID(),1});
		}
	}

	public virtual void changePower(float[] powerArgs) {
		
	}
}