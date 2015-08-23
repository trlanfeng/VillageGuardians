using UnityEngine;
public class ActItem
{
    /// <summary>
    /// 行动项
    /// </summary>
    /// <param name="id">行动项ID</param>
    /// <param name="gameObject">行动项的GameObject</param>
    /// <param name="actorType">1为友军，-1为敌军</param>
    public ActItem(int id,GameObject gameObject,int actorType)
    {
        this.id = id;
        this.gameObject = gameObject;
        this.actorType = actorType;
    }

    private GameObject gameObject;

    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }

        set
        {
            gameObject = value;
        }
    }
    /// <summary>
    /// -1：敌人；0：中立；1：友军
    /// </summary>
    public int ActorType
    {
        get
        {
            return actorType;
        }

        set
        {
            actorType = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    private int actorType;
    private int id;
}