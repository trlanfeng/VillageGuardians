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
    public List<GameObject> playerList;
    public List<GameObject> enemyList;
    int actorID;

    public List<ActItem> ActorList;
    //当前是否在行动
    bool inAct;
    //行动计时器
    float actTimer;
    //每回合行动时间
    float everyActTime;

    GameManager GM;

    // Use this for initialization
    void Start()
    {
        GM = this.GetComponent<GameManager>();

        inAct = false;
        actorID = 0;
        currentActorIndex = 0;
        ActorList = new List<ActItem>();
        for (int i = 0; i < playerList.Count; i++)
        {
            ActorList.Add(new ActItem(actorID, playerList[i], 1));
            actorID++;
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            ActorList.Add(new ActItem(actorID, enemyList[i], -1));
            actorID++;
        }
        actTimer = 2f;
        everyActTime = 3f;
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
        if (!inAct)
        {
            actTimer += Time.deltaTime;
            //if (Input.GetMouseButtonUp(0))
            //{
            //    turnBase();
            //}
        }
        //if (actTimer > everyActTime)
        //{
        //    actTimer = 0;
        //    turnBase();
        //}
        if (blinkTotal > 0)
        {
            hertBlink(ActorList[currentActorIndex].GameObject, false);
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
        if (ActorList[currentActorIndex].ActorType == 1)
        {
            moveDistance = Mathf.Abs(moveDistance);
        }
        else if (ActorList[currentActorIndex].ActorType == -1)
        {
            moveDistance = Mathf.Abs(moveDistance) * -1;
        }
        trans.DOMoveX(trans.position.x + moveDistance, 0.1f).OnComplete(() =>
        {
            Debug.Log("执行了一次攻击！");
            //让怪物闪烁0.5秒
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
        Image img = go.GetComponent<Image>();
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
    #endregion
}
