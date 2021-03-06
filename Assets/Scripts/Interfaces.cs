﻿using UnityEngine;
using System.Collections;

public interface Powerable {

	void changePower (float[] powerArgs);
	GameObject getGameObject();
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

public interface FindPropertys {
	
	bool hasProperty(string property);
	void changeProperty(string property, string[] propertyValue);
	string getName();
}
