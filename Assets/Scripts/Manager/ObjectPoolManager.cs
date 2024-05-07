using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class ObjectPoolManager : MonoBehaviour 
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private static GameObject _objectPoolEmptyHolder;
    private static GameObject _gameObjectEmpty;
    private static GameObject _particleSystemEmpty;
    private static GameObject _candyObjectEmpty;
    private static GameObject _deadCandyObjectEmpty;


    public enum PoolType
    {
        GameObject,
        ParticleSystem,
        Candy,
        DeadCandy,
        none
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }
    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemEmpty = new GameObject("Particle Effects");
        _particleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectEmpty = new GameObject("GameObjects");
        _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _candyObjectEmpty = new GameObject("Candy");
        _candyObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _deadCandyObjectEmpty = new GameObject("Dead Candy");
        _deadCandyObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static  GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation,PoolType poolType = PoolType.none)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObject.FirstOrDefault();


        if(spawnableObj == null) 
        {
            GameObject parentObject = SetParentObject(poolType);

            spawnableObj = Instantiate(objectToSpawn, spawnPosition,spawnRotation);

            if(parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObject.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(p=> p.LookupString == goName);
        if(pool == null)
        {
            return;
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObject.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemEmpty;
            case PoolType.GameObject:
                return _gameObjectEmpty;
            case PoolType.none:
                return null;
            case PoolType.Candy:
                return _candyObjectEmpty;
            case PoolType.DeadCandy:
                return _deadCandyObjectEmpty;
            default:
                return null;
        }
    }
}
public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObject = new List<GameObject>();
}