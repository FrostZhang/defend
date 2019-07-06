using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    [System.Serializable]
    public class EnimyData
    {
        public int hp;
        public int att;
        public float speed;
    }

    [System.Serializable]
    public class CusData
    {
        public int money;
        public int stagelevel;   //已解锁关卡等级
        public int gun;
    }

    [System.Serializable]
    public class JdData
    {
        public int level;
    }

    [System.Serializable]
    public class JDdatas
    {
        public List<JdData> fs;
    }
}
