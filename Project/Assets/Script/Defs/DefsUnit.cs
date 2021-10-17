using System.Collections.Generic;


public class DefsUnit
{
    static public class TriggerName
    {
        public const string RANDOM_BOX = "RandomBox";
        public const string ESSENCE = "Essence";
    }

    static public class RandomBox
    {
        public const int UNIT_IDX   = -1;
        public const int MOVE_SPEED = 14;
    }
}

public enum AttackerClassType
{
    Warrior,
    Rogue,
    Archer,
    Mage,
}

public enum MonsterType
{
    Normal,
    Boss,
}