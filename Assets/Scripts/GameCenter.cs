using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Video;
using TMPro;
using static FoxFollowingAI;


public class GameCenter : MonoBehaviour
{
    public GameObject snowBall;
    public GameObject pebble;

    public static GameObject Audio;
    public static GameObject miniMap;
    public static GameObject settingPanel;
    public static GameObject logFinishHINT;
    public static GameObject Camera;
    public static GameObject BeginHint1;

    public static Transform player;
    public static Transform teleportationPoint;

    public static Light directionalLight;
    public static Light exitLight;

    public static NavMeshAgent agentFOX;

    public static LayerMask groundMask;
    public static LayerMask whatIsMonster;
    #region UI
    public static GameObject catchedBTN;
    public static GameObject noFlameBTN;
    public static GameObject winBTN;

    public static Slider lifeBar;
    public static Slider energyBar;
    public static Slider sensitiveBar;

    public static Image EnergyFillIMG;
    public static Image LifeFillIMG;
    public static Image catchedIMG;
    public static Image noFlameIMG;
    public static Image scoreIMG;
    
    public static Text textLog;
    public static Text textPebble;
    public static Text ranking;
    public static Text detail;

    #endregion

    public static bool isMonsterInSightRange = false;
    public static bool isTouchedSnowBall = false;
    public static bool isGetProtein = false;
    public static bool isEnoughLog = false;
    public static bool isOver = false;

    public static int requiredLog = 6;
    public static int numberLog = 0;
    public static int score = 0;

    public static float timer;
    public static float remainingTime;
    public static float energy = 100f;
    public static float mouseSensitive = 200f;
    public static float sightRange = 200f;
    public static float opacity1 = 0f;
    public static float opacity2 = 1f;

    //分數只加一次
    public static bool tempB = true;

    int numPebble = 10;

    //蛋白特效
    float timeA = 0f;
    //雪球生成
    float timeB = 0f;

    Vector3[] snowBallShowPos = new Vector3[2];


    public Material skyMate;

    void Awake() { Loading(); }
    void Start()
    {

        timer = 0f;
        BeginHint1.SetActive(false);
        miniMap.SetActive(false);
        settingPanel.SetActive(false);
        logFinishHINT.SetActive(false);
        winBTN.SetActive(false);
        exitLight.enabled = false;
        scoreIMG.enabled = false;
        ranking.enabled = false;
        detail.enabled = false;
        isGetProtein = false;
        isEnoughLog = false;
        isOver = false;
        foxisFound = false;
        numberLog = 0;
        numPebble = 10;
        opacity1 = 0f;
        opacity2 = 1f;
        remainingTime = 60f;
        energy = 100f;
        sightRange = 200f;
        snowBallShowPos[0] = new Vector3(2098, 127, 2985);
        snowBallShowPos[1] = new Vector3(2768, 96, 2026);
        if (Time.time < 120f) { Invoke("ShowBeginHint", 1.5f); }
    }
    void Update()
    {
        timer += Time.deltaTime;

        textLog.text = numberLog + " / " + requiredLog;
        textPebble.text = numPebble.ToString();

        ShowEnoughLogHint();
        DirectionalLightSetting();
        OnOffSetting(KeyCode.Escape);
        SetLife(Time.deltaTime * .7f);
        SetEnergy(Time.deltaTime * .8f);
        ProteinEffect(Time.deltaTime * 5f);
        ThrowPebble(KeyCode.E,Time.deltaTime * 1333f);
        SnowBallGenerator(snowBallShowPos[Random.Range(0, 2)], 10f);
    }
    void OnOffSetting(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            settingPanel.SetActive(!settingPanel.activeSelf);

            if (Time.timeScale == 0f) Time.timeScale = 1f;
            else Time.timeScale = 0f;
        }
    }
    void DirectionalLightSetting()
    {
        if (remainingTime <= 40f)
            directionalLight.intensity = remainingTime / 40f;
        else
            directionalLight.intensity = 1f;
    }
    void SetLife(float decreaseSpeed)
    {
        remainingTime -= decreaseSpeed;
        lifeBar.value = remainingTime;
        if(remainingTime <= 0f)
        {
            ShowSomethingSlowly(noFlameIMG, Time.deltaTime * .2f);
            ShowSomethingSlowly(noFlameBTN.GetComponent<Image>(), Time.deltaTime * .2f);
            if (!isOver)
            {
                FindObjectOfType<AudioManager>().Play("cry");
                isOver = true;
            }
        }
    }
    void SetEnergy(float recoverySpeed)
    {
        energyBar.value = energy;
        if (!(energy < 100f)) return;
        energy += recoverySpeed;
    }
    void ShowEnoughLogHint()
    {
        if (numberLog == requiredLog && !isEnoughLog)
        {
            logFinishHINT.SetActive(true);
            Time.timeScale = 0f;

            RenderSettings.skybox = skyMate;

        }
    }
    void ThrowPebble(KeyCode key,float speed)
    {
        if (Input.GetKeyDown(key) && numPebble > 0)
        {
            GameObject pebbleClone = Instantiate(pebble, Camera.transform.position, Quaternion.identity);
            pebbleClone.GetComponent<Rigidbody>().velocity = player.transform.forward * speed;
            FindObjectOfType<AudioManager>().Play("throw");
            numPebble--;
        }
    }
    void ProteinEffect(float frequency)
    {
        if (isGetProtein && timer > timeA + frequency)
        {
            //EFFECT
            if (EnergyFillIMG.color == Color.magenta) EnergyFillIMG.color = Color.green;
            else EnergyFillIMG.color = Color.magenta;

            timeA = timer;
        }
    }
    void ShowBeginHint()
    {
        BeginHint1.SetActive(true);
        Time.timeScale = 0f;
    }
    public static void ShowSomethingSlowly(Image img, float showSpeed)
    {
        if (opacity1 != 1f)
        {
            opacity1 += showSpeed;
            img.color = new Color(img.color.r, img.color.g, img.color.b, opacity1);
        }
    }
    public static void DisappearSomethingSlowly(Material m, float disappearSpeed)
    {
        if (opacity2 > 0f)
        {
            opacity2 -= disappearSpeed;
            m.color = new Color(m.color.r, m.color.g, m.color.b, opacity2);
        }
    }
    void SnowBallGenerator(Vector3 position, float cycle)
    {
        if (timer == 0 || timer > timeB + cycle)
        {
            opacity2 = 1f;
            isTouchedSnowBall = false;
            Instantiate(snowBall, position, Quaternion.identity);
            timeB = timer;
        }
    }
    public static void ScoreCalculator()
    {
        // 35:S 30^:A 20^:B 10^:C 0^:D
        if (tempB)
        {
            score += (numberLog - requiredLog) * 5;

            if (timer < 420f)
                score += 10;
            if (timer > 420f && timer < 600f)
                score += 5;
            if (foxisFound)
                score += 10;

            tempB = false;
        }
        if (score == 35)
        {
            ranking.text = "S";
            ranking.color = Color.yellow;
        }
        else if (score == 30)
        {
            ranking.text = "A";
            ranking.color = Color.red;
        }
        else if (score >= 20)
        {
            ranking.text = "B";
            ranking.color = Color.blue;
        }
        else if (score >= 10)
        {
            ranking.text = "C";
            ranking.color = Color.green;
        }
        else
        {
            ranking.text = "D";
            ranking.color = Color.gray;
        }
    }
    public static string Second_TO_Minute(float second_)
    {
        if (second_ <= 60f)     return ((int)second_).ToString();
        
        int minute = (int)second_ / 60;
        int second = (int)second_ % 60;

        if (minute < 10f && second < 10f)
            return ("0" + minute + " : 0" + second);
        else if (minute < 10f && second > 10f)
            return ("0" + minute + " : " + second);
        else if (minute > 10f && second < 10f)
            return (minute + " : 0" + second);
        else
            return (minute + " : " + second);
    }
    void Loading()
    {
        Audio = GameObject.Find("Audio");
        miniMap = GameObject.Find("MiniMap");
        settingPanel = GameObject.Find("Canvas/SettingPanel");
        player = GameObject.Find("Player").GetComponent<Transform>();
        agentFOX = GameObject.Find("Fox").GetComponent<NavMeshAgent>();
        logFinishHINT = GameObject.Find("Canvas/LogFinishHINT");
        Camera = GameObject.Find("Main Camera");
        BeginHint1 = GameObject.Find("Canvas/BeginHints/Hint1");

        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        exitLight = GameObject.Find("Exit Light").GetComponent<Light>();
        teleportationPoint = GameObject.Find("Teleport Center").GetComponent<Transform>();
        textLog = GameObject.Find("Canvas/LogInfo/LogText").GetComponent<Text>();
        textPebble = GameObject.Find("Canvas/PebbleInfo/PebbleText").GetComponent<Text>();
        lifeBar = GameObject.Find("Canvas/LifeSLD").GetComponent<Slider>();
        energyBar = GameObject.Find("Canvas/EnergySLD").GetComponent<Slider>();
        sensitiveBar = GameObject.Find("Canvas/SettingPanel/MouseSLD").GetComponent<Slider>();
        EnergyFillIMG = GameObject.Find("Canvas/EnergySLD/Fill Area/Fill").GetComponent<Image>();
        LifeFillIMG = GameObject.Find("Canvas/LifeSLD/Fill Area/Fill").GetComponent<Image>();
        catchedIMG = GameObject.Find("Canvas/CatchedIMG").GetComponent<Image>();
        noFlameIMG = GameObject.Find("Canvas/NoFlameIMG").GetComponent<Image>();
        scoreIMG = GameObject.Find("Canvas/ScoreIMG").GetComponent<Image>();
        catchedBTN = GameObject.Find("Canvas/CatchedIMG/RestartBTN");
        noFlameBTN = GameObject.Find("Canvas/NoFlameIMG/RestartBTN");
        winBTN = GameObject.Find("Canvas/ScoreIMG/RestartBTN");

        ranking = GameObject.Find("Canvas/ScoreIMG/Ranking").GetComponent<Text>();
        detail = GameObject.Find("Canvas/ScoreIMG/Detail").GetComponent<Text>();
    }
}
