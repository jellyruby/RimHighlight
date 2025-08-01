// 파일: Window_ReplayOverlay.cs
using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace OneShotHighlight
{
    public class Window_ReplayOverlay : Window
    {
        // 생성자에서 창의 기본 속성을 설정
        public Window_ReplayOverlay()
        {
            
            // true: 다른 창 위에 그려짐 (중요)
            this.doWindowBackground = false;

            // true: 배경의 게임을 어둡게 처리하지 않음 (우리가 직접 어둡게 할 것)
            this.absorbInputAroundWindow = true;

            // true: 창이 떠 있는 동안 게임 시간을 정지시킴 (선택 사항)
            this.preventCameraMotion = true;

            // 팝업 창처럼 자동으로 어두워지는 효과를 끔
            this.drawShadow = false;
        }

        // 창의 내용을 그리는 핵심 메서드
        public override void DoWindowContents(Rect inRect)
        {
            // 이 창은 내용이 없으므로 이 메서드는 비워둡니다.
            // 여기에 "REPLAY" 같은 텍스트를 그리는 코드를 추가할 수도 있습니다.
            // 예:
            // Text.Font = GameFont.Medium;
            // Text.Anchor = TextAnchor.MiddleCenter;
            // Widgets.Label(inRect, "R E P L A Y");
            // Text.Anchor = TextAnchor.UpperLeft; // 앵커 리셋
        }

        // 창 배경을 직접 그리는 메서드를 재정의
        public override void PostOpen()
        {
            // 화면 전체를 덮는 반투명한 검은색 사각형을 그립니다.
            // new Color(R, G, B, A) -> R,G,B는 0 (검은색), A는 투명도(0.0~1.0)
            GUI.color = new Color(0f, 0f, 0f, 0.6f); // 60% 투명도의 검은색

            // 화면 전체 크기의 텍스처를 그림
            GUI.DrawTexture(new Rect(0, 0, UI.screenWidth, UI.screenHeight), BaseContent.WhiteTex);

            // 원래 색상으로 복원 (다른 UI에 영향을 주지 않기 위함)
            GUI.color = Color.white;
        }

        // 창이 닫힐 때 호출되는 메서드
        public override void PostClose()
        {
            base.PostClose();
            // 리플레이 모드 종료와 관련된 추가 정리 작업을 여기서 수행할 수 있습니다.
        }
    }
}