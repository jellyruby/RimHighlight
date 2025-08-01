using RimWorld;
using Verse;
using Verse.Sound;

namespace OneShotHighlight
{
    public class HighlightManager : GameComponent
    {
        private static bool highlightActive = false;
        private static int highlightTicksRemaining = 0;
        private const int HighlightDuration = 120; // 하이라이트 지속 시간 (120틱 = 2초)

        private static Pawn victim;
        private static Pawn attacker;

        public HighlightManager(Game game) { } // GameComponent는 이 생성자가 필요합니다.

        // 외부에서 호출할 정적 메서드
        public static void TriggerHighlight(Pawn aVictim, Pawn anAttacker)
        {
            // 이미 하이라이트가 진행 중이면 무시
            if (highlightActive) return;

            victim = aVictim;
            attacker = anAttacker;
            highlightActive = true;
            highlightTicksRemaining = HighlightDuration;

            // 1. 카메라 이동
            Find.CameraDriver.JumpToCurrentMapLoc(victim.Position);

            // 2. 효과음 재생 (XML에 SoundDef를 먼저 정의해야 함)
            //SoundDefOf.BulletImpact_Metal.PlayOneShot(new TargetInfo(victim.Position, victim.Map));

            // 2. 화면 정지 
            Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;

            // 3. 텍스트 효과 (Mote)
            MoteMaker.ThrowText(victim.DrawPos, victim.Map, "ONE SHOT!", 3.5f);

        }

        // 매 틱마다 호출되는 메서드
        public override void GameComponentTick()
        {
            base.GameComponentTick();

            if (highlightActive)
            {
                // 4. 슬로우 모션
                // TickManager를 강제로 보통 속도로 되돌리려는 다른 신호들을 무시
                Find.TickManager.slower.SignalForceNormalSpeedShort();

                // 게임 속도를 가장 느리게 설정
                if (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused)
                {
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                }

                highlightTicksRemaining--;

                if (highlightTicksRemaining <= 0)
                {
                    highlightActive = false;
                    // 슬로우 모션 해제
                    if (Find.TickManager.CurTimeSpeed == TimeSpeed.Normal)
                    {
                        // 원래 속도로 복구
                        Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
                    }
                }
            }
        }



        // 세이브/로드 관련
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref highlightActive, "highlightActive", false);
            Scribe_Values.Look(ref highlightTicksRemaining, "highlightTicksRemaining", 0);
            Scribe_References.Look(ref victim, "victim");
            Scribe_References.Look(ref attacker, "attacker");
        }
    }
}