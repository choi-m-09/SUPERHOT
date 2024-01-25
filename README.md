# 게임 소개
  ![그림1](https://github.com/choi-m-09/SUPERHOT/assets/80871047/4497254e-ca70-4777-8282-4801066262e3)

플레이어의 행동에 맞춰 시간이 흐른다는 설정을 가지고 있는 독특한 설정의 게임입니다. VR 버전의 경우 VR헤드셋과 컨트롤러에 대응하여 시간이 흐릅니다. 플레이어의 활동에 따라 시간이 흐르기 때문에 적이 공격하기 전에 먼저 처치하거나 총알을 막거나 회피할 수 있습니다. 따라서 플레이어가 시간을 조정하여 적의 공격 및 총탄을 회피하며 처치하는 전략성이 더욱 강조된 FPS 게임입니다. 이를 토대로 모작 프로젝트를 진행하였습니다.
# 플레이 영상
https://www.youtube.com/watch?v=dYUnHnK-zHk
# 주요 기능
## Bullet Time
플레이어가 움직임에 따라 시간이 흐르도록 VR 헤드셋 및 컨트롤러의 Transform 값을 프레임 이전 값와 현재 값의 거리를 기반으로 TimeScale 값이 설정되고 플레이어가 물건을 잡거나 총의 방아쇠를 당기는 경우 시간이 잠시동안 정상으로 흐르도록 코루틴을 설계하여 플레이어 무브먼트 맞춤형 TimeSystem을 구축하였습니다.

## Enemy AI
Nav Mesh Agent 컴포넌트 기반으로 이동하며 FSM 패턴의 로직을 따릅니다. 자신의 무기 소켓에 있는 오브젝트에 따라 상태가 구분되며 3가지로 나뉩니다.
+ Pistal: 소켓에 Pistal이 있는 경우 상태가 설정됩니다. 플레이어 추격 중 사정거리에 도달하면 사격 준비 애니메이션이 실행되며 Final IK 에셋의 AimIK 컴포넌트를 통해 플레이어 카메라 방향으로 Aiming 되고 사격을 실시합니다. 일정 거리에 도달하면 자리에 멈춰 사격을 진행합니다.

+ Shotgun: 소켓에 Shotgun이 있는 경우 상태가 설정됩니다. Pistal 상태와 전반적인 로직은 같지만 애니메이션 및 애니메이션 이벤트가 다르게 실행됩니다.

+ None: 소켓에 아무것도 감지되지 않은 경우 상태가 설정됩니다. 플레이어를 추격하다가 주위에 무기가 감지되면 무기를 향하여 이동 후 일정 거리에 도달하면 Pick up 애니메이션 실행 후 무기가 소켓에 자식으로 들어가며 해당 무기에 따라 Pistal 또는 Shotgun 상태로 전이됩니다. 무기가 감지되지 않으면 플레이어에게 다가와 근접 공격을 수행합니다.

적은 총알에 맞거나 특정 오브젝트(유리병)에 일정 힘 이상으로 충돌하는 경우 Ragdoll이 적용되고 일정 시간이 지나면 Destroy 됩니다.

## Weapon
무기는 총 3가지 종류가 있습니다. 모든 무기는 컨트롤러의 Grip 버튼을 통하여 잡을 수 있고 총기류는 오브젝트를 잡은 상태에서 컨트롤러의 Trigger 버튼으로 발사할 수 있습니다. 또한 잡고 있는 오브젝트를 던져 적을 공격할 수 있습니다. Collider의 relativeVelocity 속성 값을 이용하여 너무 약하게 충돌되면 충돌 이벤트가 발생하지 않도록 설계 하였습니다. 총기류는 플레이어와 적이 공통으로 사용하며 무기마다 무게 및 로직이 다릅니다. 각 무기의 로직은 다음과 같습니다.
+ Pistal: 플레이어가 소지하고 있는 경우 Trigger 버튼을 누를 시 총구의 Forward 방향으로 Bullet이 Instantiate되는 로직의 함수가 호출됩니다. 적이 소지한 경우 애니메이션 이벤트로 동일한 함수가 호출됩니다.
 
+ Shotgun: 플레이어가 소지하고 있는 경우 Trigger 버튼을 누를 시 총구의 Forward 방향으로 10개의 Bullet이 회전 값이 제한 값 내에서 랜덤하게 설정되어 Instantiate 되고 서로 충돌되지 않도록 잠시 Bullet의 Collider가 비활성화 되도록 코루틴이 호출되는 로직의 함수가 호출됩니다. 적이 소지한 경우 애니메이션 이벤트로 동일한 함수가 호출됩니다. 또한 Pistal보다 무겁도록 Rigidbody의 Mass 값을 조정하였습니다.

+ Bottle: 플레이어만 사용할 수 있는 무기입니다. 무게를 가볍게 설정하여 더 잘 날아가도록 설계하였습니다.

밸런스를 위해 적이 총을 소지한 경우 총알은 무제한이고 플레이어는 제한이 있습니다. 적이 무기를 소지하고 사망하는 경우 Rigidbody의 Addforce 함수로 플레이어 카메라 방향으로 적이 소지한 무기가 날아올 수 있도록 설계하였습니다.

## 분리 효과
무기 및 특정 오브젝트(유리병)는 일정 힘 이상으로 충돌 시 분리 효과가 발생합니다. 오브젝트의 몸체를 부위별로 각기 다른 오브젝트로 분리하여 RigidBody의 AddExplosion 함수를 통하여 오브젝트의 파편이 퍼지도록 로직을 구현하였습니다. Enemy는 특정 부위(머리, 팔, 다리)에 Bullet이 충돌하거나 오브젝트가 일정 힘 이상으로 충돌하게되면 위와 같은 로직으로 머리, 팔, 다리가 분리되는 효과를 구현하였습니다.
# 담당
+ 프로그래밍 : 최명규
+ 3D 디자인 : 임종진, 남진철
+ 애니메이션 : 최명규, 임종진

# 구동 방법
+ 사용 툴 : Unity, 3D Max, Blender 
+ 사용 에셋 : Unity XR ToolKit, Final IK
