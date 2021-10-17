using System.Collections.Generic;


public class DefsInGame
{
    // Effect
    public const int CREATE_EFFECT_NUM_PER_FRAME_IN_MAP = 13;

    // ItemSpreader
    public const int MAX_ITEM_NUM_PER_SPREAD = 40;

    // Unit Slot
    public const int UNIT_ATTACTER_SLOT_NUM = 4;
    public const int UNIT_MONSTER_SLOT_NUM  = 4;


    static public int ConvertStageIdxToChapterIdx(int stage_idx)
    {
        return stage_idx / DefsMetadata.MAX_STAGE_NUM_PER_CHAPTER;
    }
}

public enum InGameType
{
    General,

    // TODO : 요일 던전
}

public enum InGameBattleType
{
    NormalMonsterBattle,
    BossMonsterBattle,
}


public interface IReadyToStartChapter
{
    void ReadyToStartChapter();
}

public interface IStartChapter
{
    void StartChapter();
}

public interface IReadyToStartStage
{
    void ReadyToStartStage();
}

public interface IStartStage
{
    void StartStage();
}

public interface IReadyToStartBattle
{
    void ReadyToStartBattle();
}

public interface IStartBattle
{
    void StartBattle();
}


// public readonly struct PixelDamageData // TODO : ref
// {
//     public PixelPlane pixelPlane { get; }
//     public System.Numerics.BigDecimal damage { get; }

//     public PixelDamageData
//     (
//         PixelPlane pixelPlane
//         , System.Numerics.BigDecimal receivedDamage
//     )
//     {
//         this.pixelPlane = pixelPlane;
//         this.damage = receivedDamage;
//     }
// }


public enum SkipBossBattleType
{
    Challenge,
    GiveUp,
    TimeOut,
}

public interface IReadyToCompleteBattle
{
    void ReadyToCompleteBattle();
}

public interface ICompleteBattle
{
    void CompleteBattle();
}

public interface IReadyToCompleteStage
{
    void ReadyToCompleteStage();
}

public interface ICompleteStage
{
    void CompleteStage();
}

public interface IReadyToCompleteChapter
{
    void ReadyToCompleteChapter();
}

public interface ICompleteChapter
{
    void CompleteChapter();
}