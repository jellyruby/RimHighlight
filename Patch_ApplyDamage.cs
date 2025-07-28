using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;

namespace OneShotHighlight
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "PostApplyDamage")]
    public static class Patch_ApplyDamage
    {

        private static readonly FieldInfo pawnField = AccessTools.Field(typeof(Pawn_HealthTracker), "pawn");

        // ApplyDamage 실행 전에 호출될 Prefix
        static void Prefix(Pawn_HealthTracker __instance, out bool __state)
        {
            // __instance는 Pawn_HealthTracker 자신이며, pawn 속성으로 대상 폰에 접근 가능
            // __state는 Prefix와 Postfix가 공유하는 변수
            //__state = !__instance.pawn.Dead; // 폰이 죽지 않았다는 사실(true)을 저장
            // 2. AccessTools를 사용해 __instance 객체에서 pawn 필드의 값을 가져옵니다.
            Pawn pawn = (Pawn)pawnField.GetValue(__instance);

            // 가져온 pawn 객체를 사용합니다.
            __state = !pawn.Dead;
        }

        // ApplyDamage 실행 후에 호출될 Postfix
        static void Postfix(Pawn_HealthTracker __instance, DamageInfo dinfo, bool __state)
        {
            // 여기서도 동일하게 값을 가져와야 합니다.
            Pawn pawn = (Pawn)pawnField.GetValue(__instance);

            Messages.Message("죽음뜨나?"+ __instance.ShouldBeDead().ToString(), MessageTypeDefOf.NeutralEvent);

            if (__state && __instance.ShouldBeDead())
            {
                Messages.Message("TST 폰 죽기 직전", MessageTypeDefOf.NeutralEvent);


                if (dinfo.Instigator is Pawn attacker)
                {
                    HighlightManager.TriggerHighlight(pawn, attacker);
                }
            }

           /* // __state가 true (즉, 대미지 전에는 살아있었고)
            // __instance.pawn.Dead가 true (즉, 대미지 후에는 죽었다면)
            if (__state && __instance.pawn.Dead)
            {
                // 공격자가 있는지, 그리고 공격자가 폰인지 확인 (예: 함정이나 화재가 아님)
                if (dinfo.Instigator is Pawn attacker)
                {
                    // 우리가 만든 HighlightManager에게 효과를 실행하라고 요청!
                    HighlightManager.TriggerHighlight(__instance.pawn, attacker);
                }
            }*/
        }
    }
}