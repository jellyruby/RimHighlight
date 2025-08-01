using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

// 예시 데이터 구조
public struct ReplayTickData
{
    public int Tick; // 게임 틱
    public Vector3 PawnPosition; // 폰의 위치
    public Rot4 PawnRotation;   // 폰의 방향
    // 필요하다면 더 많은 정보 (애니메이션 상태 등)
}



namespace OneShotHighlight
{
    public class Recorder : GameComponent
    {

        // 틱 카운터
        private int tickCounter = 0;    

        // 리플레이 기록을 유지할 최대 틱 수 (2초 = 120틱)
        private const int MaxHistoryTicks = 240;

        // 각 폰의 ID를 키로, 지난 120틱의 데이터를 값으로 저장
        private Dictionary<int, Queue<ReplayTickData>> pawnHistory = new Dictionary<int, Queue<ReplayTickData>>();

        // 3. GameComponentTick 메서드를 재정의(override)합니다.
        public override void GameComponentTick()
        {
            // 부모 클래스의 메서드를 호출하는 것이 좋은 습관입니다.
            base.GameComponentTick();

            // 기존 코드
            // List<Pawn> allPawns = PawnsFinder.AllMaps_Spawned;

            // 수정된 코드
            IReadOnlyList<Pawn> allPawns = PawnsFinder.AllMaps_Spawned;
            int currentTick = Find.TickManager.TicksGame;

            foreach (Pawn pawn in allPawns)
            {
                // 폰이 유효한지(죽지 않았는지 등) 확인하는 것이 좋습니다.
                if (pawn == null || pawn.Destroyed)
                {
                    continue; // 다음 폰으로 넘어감
                }

                int pawnId = pawn.thingIDNumber;

                // 1. 해당 폰의 기록이 처음이라면, 새로운 Queue를 생성합니다.
                if (!pawnHistory.ContainsKey(pawnId))
                {
                    pawnHistory[pawnId] = new Queue<ReplayTickData>();
                }

                // 2. 현재 폰의 데이터를 구조체에 담습니다.
                ReplayTickData tickData = new ReplayTickData
                {
                    Tick = currentTick,
                    PawnPosition = pawn.DrawPos, // DrawPos가 시각적 위치를 더 잘 반영합니다.
                    PawnRotation = pawn.Rotation
                };

                // 3. 해당 폰의 Queue에 새로운 데이터를 추가합니다.
                pawnHistory[pawnId].Enqueue(tickData);

                // 4. Queue의 크기가 최대치를 초과하면, 가장 오래된 데이터를 제거합니다.
                while (pawnHistory[pawnId].Count > MaxHistoryTicks)
                {
                    pawnHistory[pawnId].Dequeue();
                }

                /*if (tickCounter % 240 == 0)
                {
                    Log.Message($"HighlightManager Tick: {pawnHistory[pawnId].Peek().Tick}");
                    Log.Message($"HighlightManager Pos: {pawnHistory[pawnId].Peek().PawnPosition}");
                    Log.Message($"HighlightManager Rot: {pawnHistory[pawnId].Peek().PawnRotation}");
                }*/
            }

            tickCounter++;

        }

        // 하이라이트 발동 시, 특정 폰의 기록을 가져오기 위한 메서드
        public Queue<ReplayTickData> GetPawnHistory(Pawn pawn)
        {
            if (pawn != null && pawnHistory.ContainsKey(pawn.thingIDNumber))
            {
                // 새로운 Queue를 반환하여 원본 데이터가 수정되는 것을 방지
                return new Queue<ReplayTickData>(pawnHistory[pawn.thingIDNumber]);
            }
            return null; // 기록이 없으면 null 반환
        }

        // 게임을 저장하고 불러올 때, 기록된 데이터를 함께 저장/로드하기 위한 메서드
        public override void ExposeData()
        {
            base.ExposeData();
            // pawnHistory는 복잡한 타입이라 기본 Scribe로는 저장이 어렵습니다.
            // 지금 단계에서는 생략하지만, 실제 모드에서는 커스텀 저장이 필요합니다.
            // Scribe_Collections.Look(ref pawnHistory, "pawnHistory", LookMode.Value, LookMode.Deep);
        }

        public Recorder(Game game)
        {
        }

    }


}
