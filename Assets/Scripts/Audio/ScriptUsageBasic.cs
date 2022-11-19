
// FMOD 사용법을 보여주는 Unity Behaviour 스크립트입니다.
// 게임 내 FMOD Studio API보다 효과적일 수 있습니다.
// Unity의 FMOD 구성요소 보다 더 많은 엑세스를 제공합니다.

// 컨텐츠 :
// 1. 디자이너가 Unity Inspector에서 이벤트를 선택할 수 있도록 허용
// 2. 인스턴스를 사용하여 수명동안 이벤트 관리 가능
// 3. 원샷 사운드
// 4. 이벤트 인스턴스 생성 및 시작
// 5. 게임 개체에 이벤트 인스턴스 첨부
// 6. 인스턴스 즉시 중지 및 리소스 해제
// 7. 리소스 해제
// 8. 인스턴스 시작 및 다시 시작
// 9. 인스턴스를 게임 개체의 위치로 수동 업데이트
// 10. 프레임마다 매개변수 업데이트
// 11. 원샷 이벤트 재생
// 12. 페이드 아웃으로 이벤트 중지
// 13. 매개변수로 원샷 이벤트 재생
// 14. 모든 이벤트 중지

using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;

namespace SuraSang
{
    public class ScriptUsageBasic : MonoBehaviour
    {
        // 1. EventReference 유형을 사용하면 디자이네에게
        // 이벤트 선택을 위한 UI가 표시됩니다.
        public FMODUnity.EventReference playerStateEvent;

        // 2. EventInstance 클래스를 사용하면
        // 매개변수 시작과 중지 및 변경을 포함하여 수명동안 이벤트를 관리할 수 있습니다.
        private FMOD.Studio.EventInstance playerState;

        // 3. 이 두 이벤트는 OneShot 사운드를 나타냅니다.
        // 이런 이벤트는 길이가 유한한 소리힙니다.
        public FMODUnity.EventReference DamageEvent;
        public FMODUnity.EventReference HealEvent;

        // 이 이벤트도 일회성이지만, 상태를 계속 추적하고 종료되면 특정 조치를 취하려고 합니다.
        public FMODUnity.EventReference PlayerIntroEvent;
        private FMOD.Studio.EventInstance playerintro;

        // 체력을 관리하는 FMOD 프로젝트 내 파라미터 이름입니다.
        FMOD.Studio.PARAMETER_ID healthParameterId, fullHealthParameterId;

        public int StartingHealh = 100;
        int health;
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            health = StartingHealh;

            // 4. 이벤트 인스턴스를 만들고 수동으로 시작합니다.
            playerState = FMODUnity.RuntimeManager.CreateInstance(playerStateEvent);
            playerState.start();

            playerintro = FMODUnity.RuntimeManager.CreateInstance(PlayerIntroEvent);
            playerintro.start();

            // 5. RuntimeManager는 이벤트 인스턴스를 추적합니다.
            // 매 프레임마다 지정된 게임 개체와 일치하도록 위치를 업데이트 할 수 있습니다.
            // 이것은 8번에 표시된 대로 인스턴스에 대해 수동으로 수행하는 것보다 더 쉬운 대안입니다.
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerintro, GetComponent<Transform>(), GetComponent<Rigidbody>());

            // 9번에 표시된 대로 업데이트를 할 때 마다 매개변수를 설정하는 것보다 핸들을 사용하는 것이 성능에 훨씬 좋습니다.
            FMOD.Studio.EventDescription healthEventDescription;
            playerState.getDescription(out healthEventDescription);
            FMOD.Studio.PARAMETER_DESCRIPTION healthParameterDescription;
            healthEventDescription.getParameterDescriptionByName("health", out healthParameterDescription);
            healthParameterId = healthParameterDescription.id;

            // 13번에 표시된 대로 ReceiveHealth() 에서 사용하기 위해 "FullHeal" 매개변수에 대한 핸들을 캐시합니다.
            // 이벤트 인스턴스가 재생될 때마다 재생성 되더라도 핸들은 항상 동일하게 유지됩니다.
            FMOD.Studio.EventDescription fullHealEventDescription = FMODUnity.RuntimeManager.GetEventDescription(HealEvent);
            FMOD.Studio.PARAMETER_DESCRIPTION fullHealParameterDescription;
            fullHealEventDescription.getParameterDescriptionByName("FullHeal", out fullHealParameterDescription);
            fullHealthParameterId = fullHealParameterDescription.id;
        }

        private void OnDestroy()
        {
            StopAllPlayerEvents();

            // 6. 유니티 객체가 비활성화 되었을 때 리스소르르 해제하는 방법입니다.
            playerState.release();
        }

        private void SpawnIntoWorld()
        {
            health = StartingHealh;

            // 7. 단일 이벤트를 필요한만큼 시작 및 중지할 수 있습니다.
            playerState.start();
        }

        private void Update()
        {
            // 8. 3D 이벤트의 인스턴스를 수동으로 업데이트 하는 방법입니다.
            // 5번에 표시된 내용은 이것을 자동으로 수행할 수 있는 방법을 보여줍니다.
            playerState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, rb));

            // 9. 매 프레[임마다 인스턴스의 매개변수를 업데이트 하는 방법입니다.
            playerState.setParameterByID(healthParameterId, (float)health);

            // 10. 인스턴스의 재생 상태를 쿼리하는 방법입니다.
            // 이 작업은 원샷을 재생할 때 유용하게 사용할 수 있습니다.
            // 완료될 때, Sustaining 및 Fading Out 등 다른 재생상태를 확인할 수 있습니다.
            if (playerintro.isValid())
            {
                FMOD.Studio.PLAYBACK_STATE playbackState;
                playerintro.getPlaybackState(out playbackState);

                if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
                {
                    playerintro.release();
                    playerintro.clearHandle();
                    SendMessage("PlayerIntroFinished");
                }
            }
        }

        private void TakeDamage()
        {
            health -= 1;

            // 11. 게임 오브젝트의 현재 위치에서 원샷을 재생하는 방법입니다.
            FMODUnity.RuntimeManager.PlayOneShot(DamageEvent, transform.position);

            if (health == 0)
            {
                // 12. 설정한 AHDSR을 허용하면서 사운드를 중지하는 방법입니다.
                // 페이드 아웃 등을 제어할 수 있습니다.
                playerState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        private void ReceiveHealth(bool restoreAll)
        {
            if (restoreAll)
            {
                health = StartingHealh;
            }
            else
            {
                health = Mathf.Min(health + 3, StartingHealh);
            }

            // 13. 원샷 사운드를 수동으로 재생하여 시작하기 전에 매개변수를 설정할 수 있도록 하는 방법입니다.
            FMOD.Studio.EventInstance heal = FMODUnity.RuntimeManager.CreateInstance(HealEvent);
            heal.setParameterByID(fullHealthParameterId, restoreAll ? 1.0f : 0.0f);
            heal.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            heal.start();
            heal.release();
        }

        // 14. 버스를 검색하는 방법과 버스를 사용하여 여러 이벤트를 제어하는 방법입니다.
        // 아직 재생 중이지만 제어할 수 없는 이벤트를 정지하는데 유용할 수 있습니다.
        private void StopAllPlayerEvents()
        {
            FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/player");
            playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
