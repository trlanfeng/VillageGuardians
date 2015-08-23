using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {

    private static ResourceManager instance;

    public static ResourceManager Instance
    {
        get
        {
            if (!instance)
            {
                GameObject obj = new GameObject("ResourceManager");
                instance = obj.AddComponent<ResourceManager>();
            }
            return instance;
        }
    }

    private Queue<AssetPack> loadQueue = new Queue<AssetPack>();

    void LoadAsset(string prefabName,Type type,ILoadCallBack callBack,bool isKeepInMemory = false)
    {
        //AssetPack ap = new AssetPack(prefabName, type, callBack, isKeepInMemory);
    }

    public class AssetPack
    {
        //public AssetPack(string assetName, Type type, ILoadCallBack callBack, bool isKeepInMemory = false)
        //{
        //    this.assetName = assetName;
        //    this.type = type;
        //    this.callBack = callBack;
        //    this.isKeepInMemory = isKeepInMemory;
        //}
        public string assetName;
        ILoadCallBack callBack;
        public Type type;
        bool isKeepInMemory;
    }

    #region 加载完成后回调
    interface ILoadCallBack
    {
        void Success(object asset);
        void Failure();
    }
    #endregion
}
