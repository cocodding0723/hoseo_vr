using UnityEngine;

public class Sample_Fishing : Sample_ILife
{
    public void Motion()
    {
        Debug.Log("대어가 낚이길 기다리고있다......");
    }

    public void Result()
    {
        Debug.Log("오레하 황금잉어를 낚았다.......");
    }
}