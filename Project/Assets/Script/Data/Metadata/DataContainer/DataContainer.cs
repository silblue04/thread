using System.Collections.Generic;
using System.Numerics;


public abstract class DataContainer
{
    public abstract class DataBase
    {
        readonly public int idx;
        public DataBase(int idx)
        {
            this.idx = idx;
        }
    }

    public abstract class ConditionDataBase : DataBase
    {
        public int target_value_arg0 { get; private set; }
        public int target_value_arg1 { get; private set; }

        public ConditionDataBase
        (
            int idx
            , int target_value_arg0
            , int target_value_arg1
        )
        : base(idx)
        {
            this.target_value_arg0 = target_value_arg0;
            this.target_value_arg1 = target_value_arg1;
        }
    }

    public abstract void Parsing(List<Dictionary<string, string>> datas);
    public abstract void ClearDatas();
}