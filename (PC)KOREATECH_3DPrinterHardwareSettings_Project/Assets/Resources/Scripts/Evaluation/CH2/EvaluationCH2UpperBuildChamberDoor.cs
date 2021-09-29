using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationCH2UpperBuildChamberDoor : EvaluationInteractiveObject
{
    public bool isOpen = false; // 처음에 닫혀있는 상태이니 초기화를 해줍니다
    private Animation _animation;

    private void Awake()
    {
        base.Awake();

        _animation = GetComponent<Animation>();
    }

    public override void OnMouseDown()
    {
        // 아래는 고정된 부분으로, EvaluationInteractiveObject에 정의되어 있는 부분을 복사,붙여넣기 후 오버라이드해서 사용하시면 됩니다.

        if (!IsInteractive)
            return;

        // 마우스를 따라다니는 설명 패널로 EvaluationObjectInfo 스크립트에 태그들이 추가되어 있습니다. 필요 시 EvaluationObjectInfo스크립트에 case를 추가해서 사용하시면 됩니다. 
        EvaluationSceneController.Instance.FollowGuideController.Hide();

        // EvaluationSceneController.Instance.CompleteStep(TargetSequence, TargetStep);
        // 위 스크립트는 상단 빌드 챔버 도어 & 하단 빌드 챔버가 모두 열려있을 때 -> 하단 챔버 도어 역시 이 스크립트와 비슷한 구조를 가지고 있다고 할 때, 
        // 멤버 변수로 EvaluationCH2UpperBuildChamberDoor와 하단 빌드 챔버 도어를 스크립트를 받게 하고 
        // 재료 카트를 클릭했을 때(재료 카트에 EvaluationInteractiveObject 스크립트를 붙여서) isOpen이 둘다 true이면 처리해주면 되겠죠?
        // 대충 이러한 흐름으로 하드코딩하면 평가를 짤 수 있습니다.
        // 클릭만 하면 해당 시퀀스가 완료된다면 해당 오브젝트에 EvaluationInteractiveObject를 붙이고 시퀀스 번호와 스텝번호를 입력하고, isInteractive를 체크해주면 되고
        // 여러 단계의 검사 & 특별한 동작이 필요한 경우에는 EvaluationInteractiveObject를 상속받아서 별도로 코드를 작성해주시면 됩니다.
        // 2D UI의 경우 EvaluationInteractiveObject 스크립트가 아닌 EvaluationInteractiveUI를 상속받아서 작성해주시면 됩니다.

        DoorToggle();
    }

    public void DoorOpen()
    {
        isOpen = true;
        _animation.Play("ChamberDoorOpen");
    }
    public void DoorClose()
    {
        isOpen = false;
        _animation.Play("ChamberDoorClose");
    }
    public void DoorToggle()
    {
        if (isOpen)
        {
            DoorClose();
        }
        else
        {
            DoorOpen();
        }
    }
}
