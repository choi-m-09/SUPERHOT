# 게임 소개
  ![그림1](https://github.com/choi-m-09/SUPERHOT/assets/80871047/4497254e-ca70-4777-8282-4801066262e3)

플레이어의 행동에 맞춰 시간이 흐른다는 설정을 가지고 있는 독특한 설정의 게임입니다. VR 버전의 경우 VR헤드셋과 컨트롤러에 대응하여 시간이 흐릅니다. 플레이어의 활동에 따라 시간이 흐르기 때문에 적이 공격하기 전에 먼저 처치하거나 총알을 막거나 회피할 수 있습니다. 따라서 플레이어가 시간을 조정하여 적의 공격 및 총탄을 회피하며 처치하는 전략성이 더욱 강조된 FPS 게임입니다. 이를 토대로 모작 프로젝트를 진행하였습니다.
# 플레이 영상
https://www.youtube.com/watch?v=dYUnHnK-zHk
# 주요 기능
## Bullet Time
플레이어가 움직임에 따라 시간이 흐르도록 VR 헤드셋 및 컨트롤러의 Transform 이전 값과 현재 값을 비교하여 TimeScale 값을 조정하였으며 플레이어가 물건을 잡거나 총의 방아쇠를 당기는 경우 시간이 잠시동안 정상으로 흐르도록 코루틴을 설계하여 맞춤형 TimeSystem을 구축하였습니다.

## Enemy AI
Enemy는 NavMesh Agent를 통하여 플레이어를 추적하고 총을 들고있는 경우 플레이어를 사격하며 일정 거리에 도달하면 그 자리에 멈춥니다. 총을 가지고 있지 않은 경우 플레이어를 추적하다가 주위에 무기가 감지되면 무기를 향하여 이동하며 무기를 입수합니다. 적이 총알에 맞거나 특정 오브젝트(유리병)에 일정한 힘으로 충돌하는 경우 Ragdoll 상태가 되며 특정 부위에 적중 시 몸체 분리 효과가 발생합니다.

## Weapon
무기는 총 3가지 종류가 있으며 모든 무기는 적에게 던져 공격할 수 있으며 너무 약하게 던지면 적이 사망하지 않습니다. 총기류는 VR 컨트롤러의 Trigger 버튼을 통하여 발사할 수 있습니다. 총기류 중 하나인 샷건은 여러 발의 총알이 총구의 Forward 방향으로 정해진 구역 안에서 랜덤하게 스폰되며 첫 스폰 시 총알끼리 충돌되어 사라지지 않도록 로직을 설계하였습니다. 모든 무기는 일정 힘으로 충돌하게 되면 분리 효과가 발생합니다. 밸런스를 위해 적이 총을 소지한 경우 총알은 무제한이고 플레이어는 제한이 있습니다. 적이 무기를 소지하고 사망하는 경우 Rigidbody의 Addforce 함수로 플레이어 카메라 방향으로 무기가 날아올 수 있도록 설계하였습니다.

## 분리 효과
무기 및 특정 오브젝트(유리병)과 Enemy는 일정 힘으로 충돌 시 분리 효과가 발생합니다. 오브젝트를 부위별로 각기 다른 오브젝트로 분리하여 RigidBody의 AddExplosion 함수를 통하여 오브젝트의 파편이 날아가도록 로직을 구현하였습니다. Enemy는 특정 부위(머리, 팔, 다리)에 총알이나 일정 힘으로 오브젝트가 충돌하게되면 위와 같은 로직으로 머리, 팔, 다리가 분리되는 효과를 구현하였습니다.
# 담당
+ 프로그래밍 : 최명규
+ 3D 디자인 : 임종진, 남진철
+ 애니메이션 : 최명규, 임종진

# 구동 방법
+ 사용 툴 : Unity, 3D Max, Blender 
+ 사용 에셋 : Unity XR ToolKit, Final IK
