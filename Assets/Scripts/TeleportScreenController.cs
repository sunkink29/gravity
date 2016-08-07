using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeleportScreenController : MonoBehaviour {

    FirstPersonScript player;
    public PausePanelController pausePanalController;
    public ScreenManager screenManager;
    public Animator teleportScreen;
    public string teleportName;
    static PausePanelController[] panels;
    public GameObject ButtonTemplete;
    public Text CurrentTeleport;
    public int topButtonYposition = 92;
    public int spaceBetweenButtons = 2;

    // Use this for initialization
    void Start()
    {
        player = FirstPersonScript.player;
        panels = PausePanelController.panels;
        int buttonOffset = 0;
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
            {
                PausePanelController panelController = panels[i];
                if (panelController.teleportController == this)
                {
                    CurrentTeleport.text = "Current Location: " + panelController.teleportController.teleportName;
                    buttonOffset ++;
                }
                else
                {
                    GameObject teleportButtonGameObject = (GameObject)Instantiate(ButtonTemplete);
                    Button teleportButton = teleportButtonGameObject.GetComponent<Button>();
                    RectTransform teleportButtonRectTransform = teleportButton.GetComponent<RectTransform>();
                    Text teleportButtonText = teleportButtonGameObject.GetComponentInChildren<Text>();
                    teleportButton.name = panels[i].teleportController.teleportName;
                    teleportButtonGameObject.SetActive(true);
                    teleportButtonGameObject.transform.SetParent(teleportScreen.gameObject.transform, false);
                    teleportButton.onClick.AddListener(delegate { teleport(panelController); });
                    teleportButtonText.text = panels[i].teleportController.teleportName;
                    teleportButtonGameObject.transform.localPosition = new Vector3(0, topButtonYposition - ((i - buttonOffset) * spaceBetweenButtons + (i - buttonOffset) * teleportButtonRectTransform.rect.height), 0);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void teleport(PausePanelController nextPanel)
    {
        if (player == null)
        {
            player = FirstPersonScript.player;
        }
        nextPanel.screenManager.OpenPanel(nextPanel.teleportController.teleportScreen);
        player.gameObject.transform.position = nextPanel.teleportPoint.transform.position;
        player.gameObject.transform.rotation = nextPanel.teleportPoint.transform.rotation;
        player.playerCamera.transform.rotation = new Quaternion(0, 0, 0, 0);
        player.gravityOnNormals.rayCastGround();
        screenManager.CloseCurrent();
    }

}
