using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Ship;
using static Player;

public class PlayerUI : MonoBehaviour
{
    public Player player;

    //public TextMeshProUGUI coordinatesText;

    public GameObject speedLevelIcon;
    public Sprite idleSprite;
    public Sprite midSprite;
    public Sprite maxSprite;

    private Image speedIconImage;
    public Slider healthIndicator;
    public Slider healthEaseIndicator;
    public float healthBarEaseLerpTime;

    public Image guntypeIcon;
    public Image cooldownIcon;
    public Image cooldownIcon2;

    public Slider pointsIndicator;
    public TextMeshProUGUI levelText;
    private float exp;
    private float nextLvlPoints;

    private void UpdateUI()
    {
        //coordinatesText.text = $"x: {(int)player.transform.position.x} y: {(int)player.transform.position.y}";
        
        
        setHealthBarUI(player.getHP() / player.GetStartingHP(), healthBarEaseLerpTime);
        
        
        guntypeIcon.sprite = player.getCurrentGunIcon();
        
        
        float maxCooldownHeight = 1.8f;
        float cooldownTimer = player.getCurrentGun().GetCoolDownValues();
        float cooldownHeightScale = Mathf.Clamp01(cooldownTimer / maxCooldownHeight);
        cooldownIcon.rectTransform.sizeDelta = new Vector2(cooldownIcon.rectTransform.sizeDelta.x, maxCooldownHeight * cooldownHeightScale);
        cooldownIcon2.rectTransform.sizeDelta = new Vector2(cooldownIcon2.rectTransform.sizeDelta.x, maxCooldownHeight * cooldownHeightScale);



        if (speedIconImage == null)
        {
            speedIconImage = speedLevelIcon.GetComponent<Image>();
        }

        switch (player.getCurrentSpeedLevel())
        {
            case SpeedLevel.STOP:
                speedIconImage.sprite = idleSprite;
                break;

            case SpeedLevel.MID:
                speedIconImage.sprite = midSprite;
                break;

            case SpeedLevel.MAX:
                speedIconImage.sprite = maxSprite;
                break;
        }
        



        exp = player.GetExp();
        nextLvlPoints = player.GetNextLevelRequiredPoints();
        pointsIndicator.value = Mathf.Clamp01((exp) / (nextLvlPoints));
        levelText.text = $"{player.GetLevel()} Lvl";
    }

    private void setHealthBarUI(float value, float lerpTime)
    {
        healthIndicator.value = value;
        if (healthEaseIndicator.value != healthIndicator.value)
        {
            healthEaseIndicator.value = Mathf.Lerp(healthEaseIndicator.value, healthIndicator.value, lerpTime);
        }
    }
    void Update()
    {
        UpdateUI();
    }
}
