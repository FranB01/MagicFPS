using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Slider crosshairManaBar;
    private TextMeshProUGUI textHpBar;
    private TextMeshProUGUI textManaBar;
    [SerializeField] private GameObject hitmarkImage;
    [SerializeField] private Image redScreenImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelEndPanel;

    private int maxHP = 100;
    private int maxMana = 100;
    
    
    private const float hitmarkDuration = 0.1f;
    private float hitmarkTimer = 0f;

    private const float redScreenDuration = 0.25f;
    private float redScreenTimer = 0f;
    private static readonly Color redScreenColor = new Color(1f, 0f, 0f, 0.3f);
    private static readonly Color transparentColor = new Color(1f, 0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);

        textHpBar = hpBar.GetComponentInChildren<TextMeshProUGUI>();
        textManaBar = manaBar.GetComponentInChildren<TextMeshProUGUI>();

        // set bar max values
        hpBar.maxValue = maxHP;
        hpBar.value = maxHP;
        manaBar.maxValue = maxMana;
        manaBar.value = maxMana;
        crosshairManaBar.maxValue = maxMana;
        crosshairManaBar.value = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        hitmarkImage.SetActive(hitmarkTimer > 0f);
        hitmarkTimer = Mathf.Clamp(hitmarkTimer - Time.deltaTime, 0f, hitmarkDuration);
        
        if (redScreenTimer > 0)
        {
            redScreenTimer -= Time.deltaTime;
            float lerp = 1 - (redScreenTimer / redScreenDuration);
            redScreenImage.color = Color.Lerp(redScreenColor, transparentColor, lerp);
        }
        
    }

    public void GetHit(int hp)
    {
        redScreenTimer = redScreenDuration;
        SetHP(hp);
    }

    private void SetHP(int hp)
    {
        textHpBar.text = hp.ToString();
        hpBar.value = hp;
    }

    public void SetMana(int mana)
    {
        textManaBar.text = mana.ToString();
        manaBar.value = mana;
        crosshairManaBar.value = mana;
        crosshairManaBar.gameObject.SetActive(mana != maxMana); // hide crosshair mana if max
    }

    public void Hitmarker()
    {
        hitmarkTimer = hitmarkDuration;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void EndLevel()
    {
        levelEndPanel.SetActive(true);
    }

    public void ButtonRetry(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ButtonMenu(){
        // TODO
        ButtonRetry();
    }
}
