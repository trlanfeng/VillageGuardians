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

    GameManager GM;

    // Use this for initialization
    void Start()
    {
        GM = this.GetComponent<GameManager>();
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
        heroList.Add(h);
        h.GameObject = GameObject.Find("Canvas").transform.Find("Panel_Fight/HeroList/Hero1").gameObject;
        ActorList.Add(h as ActItem);
        actorID++;

        createEnemys();

        actTimer = 2f;
        everyActTime = 0.5f;
    }
    Button btn;
    // Update is called once per frame
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
            hertBlink(beHurtGameObject, false);
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
            int i = Random.Range(0, enemyList.Count);
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
                    GameObject.Destroy(enemyList[defencer].GameObject);
                    enemyList.RemoveAt(defencer);
                    ActorList.RemoveAt(defencer + heroList.Count);
                    if (enemyList.Count == 0)
                    {
                        createEnemys();
                    }
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
    void hertBlink(GameObject go, bool isDead)
    {
        blinkTotal -= Time.deltaTime;
        if (go != null)
        {
            Image img = go.transform.Find("Image").GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, blinkTimer);
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
                inAct = false;
                blinkTotal = 0;
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
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

    int wave = 0;
    void createEnemys()
    {
        enemyList.Clear();
        int[][] enemys = GM.enemyIDList;
        if (wave >= enemys.Length)
        {
            isEnd = true;
            return;
        }
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
            e.setJsonToEnemy(GM.JSONO.GetField("enemy")[enemys[wave][j]]);
            e.GameObject = go;
            ActorList.Add(e as ActItem);
            enemyList.Add(e);
        }
        wave += 1;
    }
}
