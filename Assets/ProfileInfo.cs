using UnityEngine;
using System.Collections;

public class ProfileInfo : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo// : ProfileInfo//: ScriptableObject
    {
        public ScriptableObject file;
        public int currency = 0;
        public int Currency { get { return currency; } set { currency = value; } }

        string username = "Test User";
        public string Username { get { return username; } set { username = value; } }

        public PlayerInfo() { }
        public PlayerInfo (ScriptableObject _file)
        {
            file = _file;
            ReadFile(file);
        }
        public void ReadFile(ScriptableObject _file)
        {
            
        }
        public void SetPlayerInfo(ScriptableObject _file)
        {
            ReadFile(_file);
        }

    }

    [System.Serializable]
    public class DungeonInfo// : ProfileInfo//: ScriptableObject
    {
        public ScriptableObject file;
        public int roomCountSmall = 0;
        public int roomCountMedium = 0;
        public int roomCountLarge = 0;
        public int roomCountTotal { get { return roomCountSmall + roomCountMedium + roomCountLarge; } }
        float weightingSmall = 1.0f;
        float weightingMedium = 2.0f;
        float weightingLarge = 3.0f;
        public float roomCountTotalWeighted { get { return roomCountSmall * weightingSmall + roomCountMedium * weightingMedium + roomCountLarge * weightingLarge; } }

        int countAI = 0;
        public int CountAI { set { countAI = value; } }

        int roomCountSpecial = 0;
        public int RoomCountSpecial { set { roomCountSpecial = value; } }

        public DungeonInfo() { }
        public DungeonInfo(ScriptableObject _file)
        {
            file = _file;
            ReadFile(file);
        }

        public void ReadFile(ScriptableObject _file)
        {
           
        }
        public void SetDungeonInfo (ScriptableObject _file)
        {
            ReadFile(_file);
        }


    }

    public PlayerInfo playerInfo = new PlayerInfo();
    public DungeonInfo dungeonInfo = new DungeonInfo();
    public string profileRedirect;
    [SerializeField]int testNumber = 0;
    public int TestNumber { get { return testNumber; } set { testNumber = value; } }

    public virtual void ReadFile(ScriptableObject _file) { }

}
