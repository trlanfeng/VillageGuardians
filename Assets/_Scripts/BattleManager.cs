using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    //当前行动的角色
    int currentActorIndex;
    int nextActorIndex;
    //可行动角色列表
    private List<Hero> heroList;
    private List<Enemy> enemyList;
    int actorID;
    //当前被攻击对象
    GameObject beHurtGameObject;

    public List<ActItem> ActorList;
    //当前是否在行动
    bool inAct;
    //是否战斗结束
    bool isEnd;
    //行动计时器
    float actTimer;
    //每回合行动时间
    float everyActTime;
    int wave;
    GameManager GM;

    Text leftWave;
    GameObject backToVillageButton;

    public void init()
    {
        wave = 0;
        heroList = new List<Hero>();
        enemyList = new List<Enemy>();
        inAct = false;
        isEnd = false;
        actorID = 0;
        currentActorIndex = 0;
        ActorList = new List<ActItem>();
        //for (int i = 0; i < playerList.Count; i++)
        //{
        //    ActorList.Add(new ActItem(actorID, playerList[i], 1));
        //    actorID++;
        //}
        Hero h = new Hero();
        h.setJsonToHero(GM.JSONO.GetField("job")[0]);
        heroList.Add(h);
        h.GameObject = GameObject.Find("Canvas").transform.Find("Panel_Fight/HeroList/Hero1").gameObject;
        ActorList.Add(h as ActItem);
        actorID++;

        leftWave = GameObject.Find("Canvas").transform.Find("Panel_Fight/Text_leftWave").GetComponent<Text>();
        backToVillageButton = GameObject.Find("Canvas").transform.Find("Panel_Fight/Button").gameObject;
        backToVillageButton.SetActive(false);


        actTimer = 2f;
        everyActTime = 0.5f;

        isAttackToDead = false;

        createEnemys();
    }

    void Start()
    {
        GM = this.GetComponent<GameManager>();
    }

    Button btn;
    bool isAttackToDead;
    int deadID;

    void Update()
    {
        if (!GM.Fight.activeSelf)
        {
            return;
        }
        fixActorIndex();
        if (!inAct && !isEnd)
        {
            actTimer += Time.deltaTime;
        }
        if (actTimer > everyActTime)
        {
            actTimer = 0;
            turnBase();
        }
        if (blinkTotal > 0)
        {
            hertBlink(beHurtGameObject, isAttackToDead, deadID);
        }
    }
    #region 进行一个回合
    float moveDistance = 20f;
    public void turnBase()
    {
        if (inAct || blinkTotal > 0)
        {
            return;
        }
        inAct = true;
        Transform trans = ActorList[currentActorIndex].GameObject.transform;
        int attacker = -1;
        int defencer = -1;
        if (ActorList[currentActorIndex].ActorType == 1)
        {
            //int i = Random.Range(0, enemyList.Count);
            int i = 0;
            attacker = currentActorIndex;
            defencer = i;
            Debug.Log("id:::" + i);
            beHurtGameObject = enemyList[i].GameObject;
            moveDistance = Mathf.Abs(moveDistance);
            Debug.Log("执行了一次攻击！");
            int beHurtHP = heroList[attacker].str - enemyList[defencer].def;
            if (beHurtHP > 0)
            {
                enemyList[defencer].HP = enemyList[defencer].HP - beHurtHP;
                Debug.Log("被攻击者的生命：" + enemyList[defencer].HP);
                if (enemyList[defencer].HP <= 0)
                {
                    isAttackToDead = true;
                    deadID = defencer;
                    Debug.Log("角色等级：" + heroList[attacker].level);
                    Debug.Log("角色经验：" + heroList[attacker].exp);
                    Debug.Log("角色攻击：" + heroList[attacker].str);
                    //升级检测
                    heroList[attacker].checkLevel();
                }
            }
            else
            {
                Debug.Log("未破防");
            }
        }
        else if (ActorList[currentActorIndex].ActorType == -1)
        {
            int i = Random.Range(0, heroList.Count);
            attacker = currentActorIndex - heroList.Count;
            defencer = i;
            Debug.Log("id:::" + i);
            beHurtGameObject = heroList[i].GameObject;
            moveDistance = Mathf.Abs(moveDistance) * -1;
            Debug.Log("执行了一次攻击！");
            int beHurtHP = enemyList[attacker].str - heroList[defencer].def;
            if (beHurtHP > 0)
            {
                heroList[defencer].HP = heroList[defencer].HP - beHurtHP;
                Debug.Log("被攻击者的生命：" + heroList[defencer].HP);
            }
            else
            {
                Debug.Log("未破防");
            }
        }
        trans.DOMoveX(trans.position.x + moveDistance, 0.1f).OnComplete(() =>
        {
            blinkTotal = 0.5f;
            trans.DOMoveX(trans.position.x - moveDistance, 0.1f).OnComplete(() =>
            {
                inAct = false;
            });
        });
        currentActorIndex++;
    }
    #endregion
    #region 切换角色
    void fixActorIndex()
    {
        if (currentActorIndex >= ActorList.Count)
        {
            currentActorIndex -= ActorList.Count;
        }
        nextActorIndex = currentActorIndex + 1;
        if (nextActorIndex >= ActorList.Count)
        {
            nextActorIndex -= ActorList.Count;
        }
    }
    #endregion
    #region AI攻击逻辑
    //选择AI攻击对象
    void selectAttackTarget()
    {

    }
    #endregion
    #region 受伤闪烁
    float blinkTotal = 0;
    float blinkTimer = 0;
    bool timerUp = false;
    void hertBlink(GameObject go, bool isDead, int did)
    {
        blinkTotal -= Time.deltaTime;
        if (go != null)
        {
            Image img = go.transform.Find("Image").GetComponent<Image>();
            if (isDead)
            {
                img.color = new Color(1, 0, 0, blinkTimer);
            }
            else
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, blinkTimer);
            }
            if (blinkTimer >= 1)
            {
                timerUp = false;
            }
            if (blinkTimer <= 0)
            {
                timerUp = true;
            }
            if (!timerUp)
            {
                blinkTimer -= Time.deltaTime * 50;
            }
            else
            {
                blinkTimer += Time.deltaTime * 50;
            }
            if (blinkTotal < 0)
            {
                if (isDead && did > -1)
                {
                    GameObject.Destroy(enemyList[did].GameObject);
                    enemyList.RemoveAt(did);
                    ActorList.RemoveAt(did + heroList.Count);
                    if (enemyList.Count == 0)
                    {
                        createEnemys();
                    }
                }
                isAttackToDead = false;
                deadID = -1;
                inAct = false;
                blinkTotal = 0;
                img.color = new Color(1, 1, 1, 1);
            }
            else
            {
                inAct = true;
            }
        }
        else
        {
            inAct = false;
            blinkTotal = 0;
        }
    }
    #endregion

    void createEnemys()
    {
        enemyList.Clear();
        int[][] enemys = GM.enemyIDList;
        if (wave >= enemys.Length)
        {
            backToVillageButton.SetActive(true);
            isEnd = true;
            return;
        }
        leftWave.text = "第 " + (wave + 1).ToString() + " 波 / 共 " + enemys.Length + " 波";
        Debug.Log("enemyWave:::" + enemys.Length);
        for (int i = 0; i < enemys.Length; i++)
        {
            Debug.Log("enemyWave:::" + i + ":::" + enemys[i].Length);
        }
        for (int j = 0; j < enemys[wave].Length; j++)
        {
            string png = "";
            GM.JSONO.GetField("enemy")[enemys[wave][j]].GetField(ref png, "png");
            GameObject go = Instantiate(Resources.Load<GameObject>("Enemys/Enemy"));
            go.name = "Enemy" + ActorList.Count;
            go.transform.SetParent(GM.Fight.transform.Find("EnemyList"), false);
            go.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(png);
            go.transform.Find("Image").GetComponent<Image>().SetNativeSize();
            Enemy e = new Enemy();
            Debug.Log("enemyID:::" + enemys[wave][j]);
            e.setJsonToEnemy(GM.JSONO.GetField("enemy")[enemys[wave][j]]);
            e.GameObject = go;
            ActorList.Add(e as ActItem);
            enemyList.Add(e);
        }
        wave += 1;
    }
}
