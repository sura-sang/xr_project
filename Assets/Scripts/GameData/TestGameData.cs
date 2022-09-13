using System.Collections.Generic;


// 조리예 입니다.

namespace SuraSang
{
    public class TestGameData : IGameData
    {
        public enum TestEnum
        {
            Option1,
            Option2,
        }

        public int id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public float Value { get; set; }
        public bool Bool { get; set; }
        public TestEnum Enum { get; set; }
    }


    [GameDataBaseAttribute(typeof(TestGameData), "TestGameData")]
    public class TestGameDataBase : IGameDataBase
    {
        public Dictionary<int, IGameData> Datas { get; set; }

        public TestGameData GetData(int id)
        {
            if (Datas.TryGetValue(id, out var value))
            {
                return (TestGameData)value;
            }

            return null;
        }

    }
}
