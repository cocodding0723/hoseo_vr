using UnityEngine;

public class Sample_Fishing : Sample_ILife
{

    // 이걸 상속하는 부모에서 행동을 진행하는 함수를 만들고
    // 해당 스크립트에서 오버라이드 해서 구현

    // 그리고 bool값이 되었든 어떻게 되었든 해당 개체에 사용자가 접근하는지 (함수를 사용 혹은 뭐 여타~~) 체크할 수 있어야함


    public void Motion()
    {
        Debug.Log("대어가 낚이길 기다리고있다......");
    }

    public void Result()
    {
        Debug.Log("오레하 황금잉어를 낚았다.......");
    }
}