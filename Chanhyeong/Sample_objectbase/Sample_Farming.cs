using UnityEngine;

public class Sample_Farming : Sample_ILife
{
    public void Motion()
    {
        // 캐릭터 애니메이션이랑 상호작용

        // 채집 실패 애니메이션 (숙련도 미숙 등)

        // 채집중 이동
        Debug.Log("채집중입니다.....");
    }

    public void Result()
    {   
        // 확률에 대한 구현
        Debug.Log("민들레 하나를 획득했따....");
    }
}