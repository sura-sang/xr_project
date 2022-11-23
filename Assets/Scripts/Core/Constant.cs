using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuraSang
{
    public static class Constant
    {
        // 리소스 경로

        //흡수 이펙트
        public const string AngerAbsorbEffectPath = "Effect/Prefab/PREffect_sad/E_P_PR_AB"; //O
        public const string HappyAbsorbEffectPath = "Effect/Prefab/PREffect_sad/E_P_PR_AB"; //O
        public const string SadAbsorbEffectPath = "Effect/Prefab/PREffect_sad/E_P_PR_AB"; //O

        //변신 이펙트
        public const string AngerTransEffectPath = "Effect/Prefab/PREffect_ang/E_P_ang_trans"; //O
        public const string HappyTransEffectPath = "Effect/Prefab/PREffect_joy/E_P_PR_joy_TRANS"; //O
        public const string SadTransEffectPath = "Effect/Prefab/PREffect_sad/E_P_PR_SAD_TRANS"; //O

        //스킬 이펙트
        public const string HappySkillEffectPath = "Effect/Prefab/PREffect_joy/E_P_PR_joy_SKILL"; //O
        public const string AngerSkillRushEffectPath = "Effect/Prefab/PREffect_ang/E_P_ang_SKILL1"; //O
        public const string AngerSkillCrashEffectPath = "Effect/Prefab/PREffect_ang/E_P_ang_SKILL2"; //O

        //정령 이펙트
        public const string AngerSoulEffect = "Effect/Prefab/PREffect_ang/E_ang_soul"; //O
        public const string HappySoulEffect = "Effect/Prefab/PREffect_joy/E_PR_Joy_soul"; //O
        public const string SadSoulEffect = "Effect/Prefab/PREffect_sad/E_P_PR_SAD_SOUL"; //O

        //분노 헤롱헤롱 이펙트 
        public const string AngerDizzyEffect = "Effect/Prefab/PREffect_ang/E_MS_ang_STAR"; //O

        //IDLE 이펙트
        public const string AngerIdleEffect = "Effect/Prefab/PREffect_ang/E_MS_ang_idle"; //O
        public const string HappyIdleEffect = "Effect/Prefab/PREffect_joy/E_P_PR_joy_IDLE"; //O

        //Walk 이펙트
        public const string HappyWalkEffect = "Effect/Prefab/PREffect_joy/E_P_PR_joy_WALK"; //O
        public const string HappyWalk2Effect = "Effect/Prefab/PREffect_joy/E_MS_PR_joy_WALK "; //O

        //Run 이펙트
        public const string AngerRunEffect = "Effect/Prefab/PREffect_ang/E_MS_ang_dust"; //O
        public const string HappyRunEffect = "Effect/Prefab/PREffect_joy/E_P_PR_joy_RUN"; //O

        //슬픔 몬스터가 기쁜 스킬에 들어갔을 때 쓰는 이펙트 (둘이 한세트)
        public const string HappyFloorAuraEffect = "Effect/Prefab/PREffect_joy/E_MS_PR_FloorAura"; //O
        public const string HappyMuEffect = "Effect/Prefab/PREffect_joy/E_MS_PR_MU"; //O

        //기쁨 몬스터 이펙트
        public const string HappySmileEffect = "Effect/Prefab/PREffect_joy/E_MS_PR_Joy_Smile"; //O

        //슬픔 몬스터 이펙트
        public const string SadPoolEffect = "Effect/Prefab/PREffect_sad/E_MS_PR_SAD_POOL"; //O

        //분노 몬스터 이펙트
        public const string AngerWalkEffect = "Effect/Prefab/PREffect_ang/E_MS_ang_dust"; //O

        //체크 포인트 이펙트
        public const string CheckPointEffect = "Effect/E_BG_KEY/PR/PR_E_ KEY";

        //통나무 쓰러지는 이펙트
        public const string LogEffect = "Effect/Prefab/PREffect_ang/E_bg_log";

        //잠 이펙트
        public const string SleepEffect = "Effect/E_M_SLEEP/PR/p_sleep";

        //슬픔 플레이어 이펙트
        public const string SadPool = "Effect/Prefab/PREffect_sad/E_P_PR_SAD_POOL";
    }
}
