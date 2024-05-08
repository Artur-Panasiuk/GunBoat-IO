using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Lvl_Panel : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI regenerationText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI sideCannonText;
    public TextMeshProUGUI frontCannonText;
    public TextMeshProUGUI ramText;

    public TextMeshProUGUI avPoints;

    public Button hpButton;
    public Button regenerationButton;
    public Button speedButton;
    public Button sideCannonButton;
    public Button frontCannonButton;
    public Button ramButton;

    public Player player;

    private Vector3 hiddenPosition;
    private Vector3 visiblePosition;

    public float moveDistance = 500.0f;
    public float moveSpeed = 5.0f;

    void handlehpbtt()
    {
        player.increaseHpLvl();
    }
    void handleregenerationbtt()
    {
        player.increaseRegenLvl();
    }
    void handlespeedbtt() 
    {
        player.increaseSpdLvl();
    }
    void handlesidebtt() 
    {
        player.increaseSCDLvl();
    }
    void handlefrontbtt() 
    { 
        player.increaseFCDLvl();
    }
    void handlerambtt() 
    {
        player.increaseRamLvl();
    }
    void updateNumbers()
    {
        avPoints.text = (player.GetAvaiablePoints()).ToString();

        hpText.text = (player.GetShipLevelData().hp_lvl).ToString();
        regenerationText.text = (player.GetShipLevelData().regeneration_lvl).ToString();
        speedText.text = (player.GetShipLevelData().speed_lvl).ToString();
        sideCannonText.text = (player.GetShipLevelData().side_cannon_dmg_lvl).ToString();
        frontCannonText.text = (player.GetShipLevelData().front_cannon_dmg_lvl).ToString();
        ramText.text = (player.GetShipLevelData().ram_dmg_lvl).ToString();
    }


    void Start()
    {
        visiblePosition = gameObject.transform.position;
        hiddenPosition = visiblePosition - new Vector3(moveDistance, 0, 0);
        transform.position = (player.GetAvaiablePoints() > 0) ? visiblePosition : hiddenPosition;

        hpButton.onClick.AddListener(handlehpbtt);
        regenerationButton.onClick.AddListener(handleregenerationbtt);
        speedButton.onClick.AddListener(handlespeedbtt);
        sideCannonButton.onClick.AddListener(handlesidebtt);
        frontCannonButton.onClick.AddListener (handlefrontbtt);
        ramButton.onClick.AddListener(handlerambtt);
    }

    void Update()
    {
        if (player.GetAvaiablePoints() > 0)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, visiblePosition, moveSpeed * Time.deltaTime);

            hpButton.interactable = true;
            regenerationButton.interactable= true;
            speedButton.interactable= true;
            sideCannonButton.interactable= true;
            frontCannonButton.interactable= true;
            ramButton.interactable= true;
        }
        else
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, hiddenPosition, moveSpeed * Time.deltaTime);

            hpButton.interactable = false;
            regenerationButton.interactable= false;
            speedButton.interactable = false;
            sideCannonButton.interactable = false;
            frontCannonButton.interactable = false;
            ramButton.interactable = false;
        }

        updateNumbers();

    }
}
