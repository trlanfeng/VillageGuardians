using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //获取各组件的 RectTransform 方便进行设置
    public RectTransform PanelList;

    public RectTransform PanelListContainer;
    public RectTransform PanelListViewer;
    public RectTransform ButtonBack;

    //获取用于生成列表的 Prefab
    public GameObject ListItem;

    //获取滚动条，当数量小于6时不显示，反之
    public GameObject PanelListScrollbar;

    //获取 Menu 面板和 List 面板，用于切换动画
    public RectTransform PanelMenu;

    //定义JSON Object变量
    private JSONObject JSONO;

    private string dataJSON;

    //当前列表
    private GameObject[] currentDataList;

    //存储位置
    public GameObject dataList;

    public GameObject showList;

    //定义存储
    private GameObject[] itemList;

    private GameObject[] equipmentList;
    private GameObject[] magicList;
    private GameObject[] jobList;
    private GameObject[] enemyList;

    private Vector2 leftPosition = new Vector2(-480f, -65f);
    private Vector2 centerPosition = new Vector2(0, -65f);
    private Vector2 rightPosition = new Vector2(480f, -65f);

    public GameObject Village;
    public GameObject Fight;
    public GameObject Canvas;

    private void Awake()
    {
        TextAsset dataFile = Resources.Load("dataFile") as TextAsset;
        dataJSON = dataFile.text;
        JSONO = new JSONObject(dataJSON);
        createItemList();
        createEquipmentList();
        createMagicList();
        createJobList();
        Canvas = GameObject.Find("Canvas");
        Village = Canvas.transform.Find("Panel_Village").gameObject;
        Village.SetActive(true);
        Fight = Canvas.transform.Find("Panel_Fight").gameObject;
        Fight.SetActive(false);
        //createEnemyList();
    }

    private void Start()
    {
        //currentDataList = getList("item");
        //addItemToList(currentDataList);
    }

    private void Update()
    {
    }

    //创建列表项
    public void CreateItem()
    {
        Transform newItem = Instantiate(ListItem).transform;
        newItem.SetParent(PanelListContainer);
    }

    //打开列表
    public void openList(string type = "")
    {
        getList(type);
        //如果列表数目小于6
        if (PanelListContainer.childCount < 6)
        {
            //设置返回按钮的位置，跟在随后一个下面
            ButtonBack.anchoredPosition = new Vector3(0, -125f - (PanelListContainer.childCount - 1) * 83);
            //根据数目设置各Panel大小
            PanelList.sizeDelta = new Vector2(464f, (PanelListContainer.childCount + 1) * 83f - 3f);
            PanelListContainer.sizeDelta = new Vector2(464f, PanelListContainer.childCount * 83f);
            PanelListViewer.sizeDelta = PanelListContainer.sizeDelta;
            //隐藏滚动条
            PanelListScrollbar.SetActive(false);
        }
        //反之
        else
        {
            //固定在第6个的位置
            ButtonBack.anchoredPosition = new Vector3(0, -125f - 4 * 83);
            //根据数目设置各Panel大小
            PanelList.sizeDelta = new Vector2(464f, 500f);
            PanelListContainer.sizeDelta = new Vector2(464f, PanelListContainer.childCount * 83f);
            PanelListViewer.sizeDelta = new Vector2(464f, 5 * 83f);
            //显示滚动条
            PanelListScrollbar.SetActive(true);
        }
        //缓动动画
        DOTween.To(() => PanelMenu.anchoredPosition, x => PanelMenu.anchoredPosition = x, leftPosition, 0.2f);
        DOTween.To(() => PanelList.anchoredPosition, x => PanelList.anchoredPosition = x, centerPosition, 0.2f);
    }

    /// <summary>
    /// 关闭列表
    /// </summary>
    public void openMenu()
    {
        removeItemToData();
        //缓动动画
        DOTween.To(() => PanelMenu.anchoredPosition, x => PanelMenu.anchoredPosition = x, centerPosition, 0.2f);
        DOTween.To(() => PanelList.anchoredPosition, x => PanelList.anchoredPosition = x, rightPosition, 0.2f);
    }

    /// <summary>
    /// 得到 item 列表
    /// </summary>
    public void createItemList()
    {
        int x = JSONO.GetField("item").Count;
        itemList = new GameObject[x];
        for (int i = 0; i < x; i++)
        {
            Item item = new Item();
            GameObject newItem = Instantiate(ListItem);
            newItem.transform.SetParent(dataList.transform, false);
            newItem.name = "item" + i.ToString();
            JSONO.GetField("item")[i].GetField(ref item.name, "name");
            JSONO.GetField("item")[i].GetField(ref item.png, "png");
            JSONO.GetField("item")[i].GetField(ref item.effect, "effect");
            JSONO.GetField("item")[i].GetField(ref item.gold, "gold");
            JSONO.GetField("item")[i].GetField(ref item.cure, "cure");
            JSONO.GetField("item")[i].GetField(ref item.comment, "comment");
            string text_desc = item.name + "\n" + item.comment;
            int currentCount = 0;
            string text_info = "数量 " + currentCount.ToString() + "\n" + item.gold.ToString() + " G";
            Text Text_desc = newItem.transform.Find("Text_desc").GetComponent<Text>();
            Text_desc.text = text_desc;
            Text Text_info = newItem.transform.Find("Text_info").GetComponent<Text>();
            Text_info.text = text_info;
            Image IconImage = newItem.transform.Find("Icon/Image").GetComponent<Image>();
            Sprite Icon = Resources.Load<Sprite>(item.png);
            IconImage.sprite = Icon;
            IconImage.preserveAspect = true;
            itemList[i] = newItem;
        }
    }

    #region 获取 weapon 和 armor 列表

    /// <summary>
    /// 获取 weapon 和 armor 列表
    /// </summary>
    public void createEquipmentList()
    {
        int x = JSONO.GetField("weapon").Count;
        int y = JSONO.GetField("armor").Count;
        equipmentList = new GameObject[x + y];
        //weapon
        string name = "";
        string png = "";
        string effect = "";
        string comment = "";
        int gold = 0;
        int attack = 0;
        for (int i = 0; i < x; i++)
        {
            GameObject newItem = Instantiate(ListItem);
            newItem.transform.SetParent(dataList.transform);
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.name = "weapon" + i.ToString();
            JSONO.GetField("weapon")[i].GetField(ref name, "name");
            JSONO.GetField("weapon")[i].GetField(ref png, "png");
            JSONO.GetField("weapon")[i].GetField(ref effect, "effect");
            JSONO.GetField("weapon")[i].GetField(ref gold, "gold");
            JSONO.GetField("weapon")[i].GetField(ref attack, "attack");
            JSONO.GetField("weapon")[i].GetField(ref comment, "comment");
            string text_desc = name + "\n" + comment;
            string text_info = "\n" + gold.ToString() + " G";
            Text Text_desc = GameObject.Find(newItem.name + "/Text_desc").GetComponent<Text>();
            Text_desc.text = text_desc;
            Text Text_info = GameObject.Find(newItem.name + "/Text_info").GetComponent<Text>();
            Text_info.text = text_info;
            Image IconImage = GameObject.Find(newItem.name + "/Icon/Image").GetComponent<Image>();
            Sprite Icon = Resources.Load<Sprite>(png);
            IconImage.sprite = Icon;
            IconImage.preserveAspect = true;
            equipmentList[i] = newItem;
        }
        //armor
        name = "";
        png = "";
        effect = "";
        comment = "";
        gold = 0;
        int defense = 0;
        for (int i = 0; i < y; i++)
        {
            GameObject newItem = Instantiate(ListItem);
            newItem.transform.SetParent(dataList.transform);
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.name = "armor" + i.ToString();
            JSONO.GetField("armor")[i].GetField(ref name, "name");
            JSONO.GetField("armor")[i].GetField(ref png, "png");
            JSONO.GetField("armor")[i].GetField(ref effect, "effect");
            JSONO.GetField("armor")[i].GetField(ref gold, "gold");
            JSONO.GetField("armor")[i].GetField(ref defense, "defense");
            JSONO.GetField("armor")[i].GetField(ref comment, "comment");
            string text_desc = name + "\n" + comment;
            string text_info = "\n" + gold.ToString() + " G";
            Text Text_desc = GameObject.Find(newItem.name + "/Text_desc").GetComponent<Text>();
            Text_desc.text = text_desc;
            Text Text_info = GameObject.Find(newItem.name + "/Text_info").GetComponent<Text>();
            Text_info.text = text_info;
            Image IconImage = GameObject.Find(newItem.name + "/Icon/Image").GetComponent<Image>();
            Sprite Icon = Resources.Load<Sprite>(png);
            IconImage.sprite = Icon;
            IconImage.preserveAspect = true;
            equipmentList[i + x] = newItem;
        }
    }

    #endregion 获取 weapon 和 armor 列表

    #region 得到 Magic 列表

    /// <summary>
    /// 得到 Magic 列表
    /// </summary>
    public void createMagicList()
    {
        int x = JSONO.GetField("magic").Count;
        magicList = new GameObject[x];
        string name = "";
        string png = "";
        string sound = "";
        string comment = "";
        int gold = 0;
        float damage = 0;
        int costMP = 0;
        bool all = false;
        for (int i = 0; i < x; i++)
        {
            GameObject newItem = Instantiate(ListItem);
            newItem.transform.SetParent(dataList.transform);
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.name = "magic" + i.ToString();
            JSONO.GetField("magic")[i].GetField(ref name, "name");
            JSONO.GetField("magic")[i].GetField(ref png, "png");
            JSONO.GetField("magic")[i].GetField(ref sound, "sound");
            JSONO.GetField("magic")[i].GetField(ref gold, "gold");
            JSONO.GetField("magic")[i].GetField(ref damage, "damage");
            JSONO.GetField("magic")[i].GetField(ref costMP, "costMP");
            JSONO.GetField("magic")[i].GetField(ref all, "all");
            JSONO.GetField("magic")[i].GetField(ref comment, "comment");
            string text_desc = name + "\n" + comment;
            string text_info = "\n" + gold.ToString() + " G";
            Text Text_desc = GameObject.Find(newItem.name + "/Text_desc").GetComponent<Text>();
            Text_desc.text = text_desc;
            Text Text_info = GameObject.Find(newItem.name + "/Text_info").GetComponent<Text>();
            Text_info.text = text_info;
            Image IconImage = GameObject.Find(newItem.name + "/Icon/Image").GetComponent<Image>();
            Sprite Icon = Resources.Load<Sprite>("images/magic/book");
            IconImage.sprite = Icon;
            IconImage.preserveAspect = true;
            magicList[i] = newItem;
        }
    }

    #endregion 得到 Magic 列表

    #region 得到 Job 列表

    /// <summary>
    /// 得到 Job 列表
    /// </summary>
    public void createJobList()
    {
        int x = JSONO.GetField("job").Count;
        jobList = new GameObject[x];
        string name = "";
        string png = "";
        int gold = 0;
        float baseHP = 0;
        float baseMP = 0;
        float baseStr = 0;
        float baseMag = 0;
        float baseDef = 0;
        for (int i = 0; i < x; i++)
        {
            GameObject newItem = Instantiate(ListItem);
            newItem.transform.SetParent(dataList.transform);
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.name = "job" + i.ToString();
            JSONO.GetField("job")[i].GetField(ref name, "name");
            JSONO.GetField("job")[i].GetField(ref png, "png");
            JSONO.GetField("job")[i].GetField(ref gold, "gold");
            JSONO.GetField("job")[i].GetField(ref baseHP, "baseHP");
            JSONO.GetField("job")[i].GetField(ref baseMP, "baseMP");
            JSONO.GetField("job")[i].GetField(ref baseStr, "baseStr");
            JSONO.GetField("job")[i].GetField(ref baseMag, "baseMag");
            JSONO.GetField("job")[i].GetField(ref baseDef, "baseDef");
            string text_desc = name + "\n";
            string text_info = "\n" + gold.ToString() + " G";
            Text Text_desc = GameObject.Find(newItem.name + "/Text_desc").GetComponent<Text>();
            Text_desc.text = text_desc;
            Text Text_info = GameObject.Find(newItem.name + "/Text_info").GetComponent<Text>();
            Text_info.text = text_info;
            Image IconImage = GameObject.Find(newItem.name + "/Icon/Image").GetComponent<Image>();
            Sprite Icon = Resources.Load<Sprite>(png);
            IconImage.sprite = Icon;
            IconImage.preserveAspect = true;
            jobList[i] = newItem;
        }
    }

    #endregion 得到 Job 列表

    /// <summary>
    /// 得到 Enemy 列表
    /// </summary>
    public void createEnemyList()
    {
        int x = JSONO.GetField("weapon").Count;
        enemyList = new GameObject[x];
        string name = "";
        string png = "";
        int HP = 0;
        int str = 0;
        int def = 0;
        int exp = 0;
        int gold = 0;
        for (int i = 0; i < x; i++)
        {
            GameObject newItem = Instantiate(ListItem);
            newItem.transform.SetParent(PanelListContainer);
            newItem.transform.localScale = new Vector3(1, 1, 1);
            newItem.name = "item" + i.ToString();
            JSONO.GetField("item")[i].GetField(ref name, "name");
            JSONO.GetField("item")[i].GetField(ref png, "png");
            JSONO.GetField("item")[i].GetField(ref HP, "HP");
            JSONO.GetField("item")[i].GetField(ref str, "str");
            JSONO.GetField("item")[i].GetField(ref def, "def");
            JSONO.GetField("item")[i].GetField(ref exp, "exp");
            JSONO.GetField("item")[i].GetField(ref gold, "gold");
            enemyList[i] = newItem;
        }
    }

    /// <summary>
    /// 生成怪物列表
    /// </summary>
    /// <param name="level">第 level 关卡</param>
	private void generateEnemyList(int level)
    {
        string enemyListString = "";
        JSONO.GetField("level")[level].GetField(ref enemyListString, "enemy");
        string[] enemyList = enemyListString.Split('.');
        //共随机多少个怪
        int enemyCount = 0;
        int enemyCountMin = 0;
        int enemyCountMax = 0;
        if (level < 10)
        {
            enemyCountMin = 4;
            enemyCountMax = 8;
        }
        else if (level >= 10 && level < 18)
        {
            enemyCountMin = 6;
            enemyCountMax = 12;
        }
        else
        {
            enemyCountMin = 8;
            enemyCountMax = 16;
        }
        enemyCount = Random.Range(enemyCountMin, enemyCountMax);
        //第一个随机70%-100%的几率，第一个怪物的数量
        int enemy1Count = Random.Range(70, 100);
        //第二个取1减掉第一个的百分比几率再乘以怪物的总数，第二个怪物的数量
        int enemy2Count = Mathf.CeilToInt(enemyCount * (1 - enemy1Count / 100f));
        //剩余需要生成的怪物数量
        int columnLastEnemy = 4;
        List<int> enemyColumnList = new List<int>();
        do
        {
            int t = Random.Range(1, 4);
            enemyColumnList.Add(t);
            columnLastEnemy -= t;
        } while (columnLastEnemy > 0);
        //enemyColumnList是需要生成的怪物的所有数据，接下来根据第二种怪物的几率，对其中的怪物进行计算
        GameObject[] enemyListGameObject = new GameObject[enemyCount];
        List<int> changedID = new List<int>();
        do
        {
            int t;
            do
            {
                t = Random.Range(0, enemyCount - 1);
            } while (changedID.Contains(t));
            // enemyListGameObject[t] 改为第二个怪物
            changedID.Add(t);
            enemy2Count -= 1;
        } while (enemy2Count > 0);
        //得到所有最终需要生成的怪物资料 enemyListGameObject
        //根据 enemyColumnList 进行生成
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    public void getList(string type)
    {
        switch (type)
        {
            case "item":
                if (itemList == null)
                {
                    createItemList();
                }
                addItemToList(itemList);
                break;

            case "equipment":
                if (equipmentList == null)
                {
                    createEquipmentList();
                }
                addItemToList(equipmentList);
                break;

            case "magic":
                if (magicList == null)
                {
                    createMagicList();
                }
                addItemToList(magicList);
                break;

            case "job":
                if (jobList == null)
                {
                    createJobList();
                }
                addItemToList(jobList);
                break;

            case "enemy":
                if (enemyList == null)
                {
                    createEnemyList();
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 将得到的列表添加进入UI中
    /// </summary>
    public void addItemToList(GameObject[] list)
    {
        int y = list.Length;
        for (int i = 0; i < y; i++)
        {
            list[i].transform.SetParent(showList.transform);
            list[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void removeItemToData()
    {
        while (showList.transform.childCount > 0)
        {
            showList.transform.GetChild(0).SetParent(dataList.transform);
        }
    }

    public void beginFight()
    {
        Village.SetActive(false);
        Fight.SetActive(true);
    }
    public void backToVillage()
    {
        Village.SetActive(true);
        Fight.SetActive(false);
    }
}