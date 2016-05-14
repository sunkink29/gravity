using UnityEngine;
using System.Collections;

public interface Powerable {

	void powerOn ();
	void powerOn (PowerProvider reference);

	void powerOff ();
	void powerOff (PowerProvider reference);
}

public interface Interactible {

	void interact ();
}

public interface PowerProvider {

	void sendReference (Powerable reference);
}
