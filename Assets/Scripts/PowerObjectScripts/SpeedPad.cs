using UnityEngine;
using System.Collections;

public class SpeedPad : PowerObject {
    
    bool playerInTrigger = false;
    bool powered = false;
    LightStripControllerPowerObject powerLight;
    float defaltSpeed = 0;
    public float maxSpeed = 20;
    FirstPersonScript player;

    public override void Start() {
        powerLight = GetComponent<LightStripControllerPowerObject>();
        base.Start();
        player = FirstPersonScript.player;
        defaltSpeed = player.speed;
    }

    public override void changePower(float[] powerArgs) {
        powerArgs[0] = GetInstanceID();
        powerLight.changePower(powerArgs);
        if(powerArgs.Length >= 2 && powerArgs[1] >= 1){
            powered = true;
        } else {
            powered = false;
        }
    }
    
    void OnTriggerEnter(Collider collider) {
        playerInTrigger = true;
        if (powered){
            player.speed = maxSpeed;
        }
    }

    void OnTriggerExit(Collider collider) {
        playerInTrigger = false;
        bool playerCollideingWithSpeedPad = false;
        Collider[] playerCollisions = Physics.OverlapSphere(player.transform.position,player.GetComponent<CapsuleCollider>().height/2 + 0.1f);
        for (int i = 0; i < playerCollisions.Length; i++) {
            SpeedPad speedPad = playerCollisions[i].GetComponent<SpeedPad>();
            if (speedPad != null && speedPad != this) {
                playerCollideingWithSpeedPad = true;
            }
        }
        if (!playerCollideingWithSpeedPad && powered) {
            player.speed = defaltSpeed;
        }
    }
}