using UnityEngine;
using System.Collections;

public interface Powerable {

	void powerOn ();
	void powerOn (PowerProvider reference);

	void powerOff ();
	void powerOff (PowerProvider reference);

	void changePower (float[] powerArgs);
}

public interface Interactible {

	void interact ();
}

public interface PowerProvider {

	void sendReference (Powerable reference);
}

public interface Debugable {

    void debug();
}
