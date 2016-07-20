using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionPanelController : MonoBehaviour, Interactible {

    FirstPersonScript player;
    public ScreenManager screenManager;
    public Animator defaltScreen;
    public Animator teleportScreen;
    public string teleportName;
    public GameObject teleportPoint;
    public bool defaltPanel = false;
    static OptionPanelController[] panels = new OptionPanelController[10];
    public GameObject ButtonTemplete;
    public int topButtonYposition = 92;
    public int spaceBetweenButtons = 2;

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

        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
            {
                OptionPanelController panelController = panels[i];
                GameObject teleportButtonGameObject = (GameObject) Instantiate(ButtonTemplete);
                Button teleportButton = teleportButtonGameObject.GetComponent<Button>();
                RectTransform teleportButtonRectTransform = teleportButton.GetComponent<RectTransform>();
                Text teleportButtonText = teleportButtonGameObject.GetComponentInChildren<Text>();
                teleportButton.name = panels[i].teleportName;
                teleportButtonGameObject.SetActive(true);
                teleportButtonGameObject.transform.SetParent(teleportScreen.gameObject.transform, false);
                teleportButton.onClick.AddListener(delegate { teleport(panelController); });
                teleportButtonText.text = panels[i].teleportName;
                teleportButtonGameObject.transform.localPosition = new Vector3(0, topButtonYposition - (i * spaceBetweenButtons + i * teleportButtonRectTransform.rect.height), 0);
            }
        }
	}

    public void interact ()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            //print(panels[i]);
        }
        player.pauseGame();
        if (player.gamePaused)
        {
            teleport(this);
            screenManager.OpenPanel(defaltScreen);
        } else
        {
            screenManager.CloseCurrent();
        }
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void teleport(OptionPanelController nextPanel)
    {
        nextPanel.screenManager.OpenPanel(nextPanel.teleportScreen);
        player.gameObject.transform.position = nextPanel.teleportPoint.transform.position;
        player.gameObject.transform.rotation = nextPanel.teleportPoint.transform.rotation;
        player.playerCamera.transform.rotation = new Quaternion(0, 0, 0, 0);
        player.gravityOnNormals.rayCastGround();
        screenManager.CloseCurrent();
    }
}


