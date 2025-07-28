using RimWorld;
using Verse;

namespace Plague
{
    public class ThingDef_PlagueBullet : ThingDef
    {
        public float AddHediffChance = 0.05f;
        // 1. 기본값 할당을 제거합니다.
        public HediffDef HediffToAdd;

        // 2. ResolveReferences 메서드를 재정의(override)합니다.
        public override void ResolveReferences()
        {
            // 부모 클래스의 메서드를 먼저 호출하는 것이 좋은 습관입니다.
            base.ResolveReferences();

            // 3. XML에서 HediffToAdd 값을 지정하지 않았을 경우에만 기본값을 할당합니다.
            // 이 시점에서는 HediffDefOf가 완전히 초기화되어 안전합니다.
            if (HediffToAdd == null)
            {
                HediffToAdd = HediffDefOf.Plague;
            }
        }
    }
}