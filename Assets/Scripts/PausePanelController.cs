using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PausePanelController : MonoBehaviour, Interactible {

    FirstPersonScript player;
    public ScreenManager screenManager;
    public Animator defaltScreen;
    public TeleportScreenController teleportController;
    public GameObject teleportPoint;
    public bool defaltPanel = false;
    public static PausePanelController[] panels = new PausePanelController[10];

    void Awake()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] == null)
            {
                panels[i] = this;
                break;
            }
        }
    }

    // Use this for initialization
    void Start () {
        player = FirstPersonScript.player;
        if (defaltPanel)
        {
            interact();
        }
	}

    public void interact ()
    {
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(teleportPoint.transform.position, -teleportPoint.transform.up, out raycastHit);
        //player.gravityOnNormals.rayCastGround();
        if (hit && raycastHit.normal == player.changeGravity.objectGravity.currentDirection)
        {
            player.pauseGame();
            if (player.gamePaused)
            {
                teleport(this);
                screenManager.OpenPanel(defaltScreen);
            }
            else
            {
                screenManager.CloseCurrent();
            }
        }
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void teleport(PausePanelController nextPanel)
    {
		player.GetComponent<Rigidbody> ().velocity = new Vector3(0,0);
        teleportController.teleport(nextPanel);
    }
}


