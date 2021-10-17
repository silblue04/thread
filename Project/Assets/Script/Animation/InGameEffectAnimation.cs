using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEffectAnimationType : AnimationType
{
    public static readonly InGameEffectAnimationType Pixel_Open = new InGameEffectAnimationType("brick_fx");
    public static readonly InGameEffectAnimationType Pixel_OpenStep = new InGameEffectAnimationType("open_step");
    public static readonly InGameEffectAnimationType Pixel_Light = new InGameEffectAnimationType("brick_light");
    
    public static readonly InGameEffectAnimationType BasicCurreny_Coin = new InGameEffectAnimationType("coin");

    public static readonly InGameEffectAnimationType Trigger_PointOn = new InGameEffectAnimationType("hammer_point_on", true);
    public static readonly InGameEffectAnimationType Trigger_PointOff = new InGameEffectAnimationType("hammer_point_off", true);
    public static readonly InGameEffectAnimationType Trigger_PointJump = new InGameEffectAnimationType("hammer_point_jump", true);

    public static readonly InGameEffectAnimationType ReadyToStartBattle_NormalMonster = new InGameEffectAnimationType("start_battle1", false);
    public static readonly InGameEffectAnimationType ReadyToStartBattleInProgress_NormalMonster = new InGameEffectAnimationType("start_battle2", true);
    public static readonly InGameEffectAnimationType StartBattle_NormalMonster = new InGameEffectAnimationType("start_battle3", false);

    public static readonly InGameEffectAnimationType ReadyToStartBattle_BossMonster = new InGameEffectAnimationType("start_battle1_boss", false);
    public static readonly InGameEffectAnimationType ReadyToStartBattleInProgress_BossMonster = new InGameEffectAnimationType("start_battle2_boss", true);
    public static readonly InGameEffectAnimationType StartBattle_BossMonster = new InGameEffectAnimationType("start_battle3_boss", false);

    public static readonly InGameEffectAnimationType SkipBattle_ChallengeAndGiveUp = new InGameEffectAnimationType("give_up");
    public static readonly InGameEffectAnimationType SkipBattle_TimeOut = new InGameEffectAnimationType("time_out_start");
    
    public static readonly InGameEffectAnimationType CompleteBattle = new InGameEffectAnimationType("finish");
    public static readonly InGameEffectAnimationType CompleteBattle_SemiBossBattle_Start = new InGameEffectAnimationType("clear_start");
    public static readonly InGameEffectAnimationType CompleteBattle_SemiBossBattle_Loop = new InGameEffectAnimationType("clear_loop");
    public static readonly InGameEffectAnimationType CompleteBattle_LastBossBattle_Start = new InGameEffectAnimationType("stage_clear_start");
    public static readonly InGameEffectAnimationType CompleteBattle_LastBossBattle_Loop = new InGameEffectAnimationType("stage_clear_loop", true);

    public static readonly InGameEffectAnimationType PixelBox_Idle = new InGameEffectAnimationType("box_idle", true);
    public static readonly InGameEffectAnimationType PixelBox_Open = new InGameEffectAnimationType("box_open", false);
    public static readonly InGameEffectAnimationType PixellBox_Fade = new InGameEffectAnimationType("box_fade", true);
    public static readonly InGameEffectAnimationType PixelBox_FadeAfterOpen = new InGameEffectAnimationType("box_fade out", false);


    public static readonly InGameEffectAnimationType AlterEgo_Idle = new InGameEffectAnimationType("worker_shadow_idle", true);
    public static readonly InGameEffectAnimationType AlterEgo_Work = new InGameEffectAnimationType("worker_shadow_walk", true);

    public static readonly InGameEffectAnimationType Dragon_Idle = new InGameEffectAnimationType("yongyong_idle", true);
    public static readonly InGameEffectAnimationType Dragon_Work = new InGameEffectAnimationType("yongyong_breath", true);
    public static readonly InGameEffectAnimationType Dragon_Move = new InGameEffectAnimationType("yongyong_walk", true);


    private InGameEffectAnimationType(string animationName, bool loop = false)
       : base(animationName, loop)
    {
        
    }
}

public class InGameEffectAnimation : SpineAnimationBase
{
    
}
