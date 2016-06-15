using UnityEngine;
using System.Collections;

public class BGManager : MonoBehaviour
{ 

    public GameObject initPos;
    private float moveTarX;
    private float moveTarY;
    private float moveTarZ;

    private Vector3 tempPos;

    public MoveNegative negativeX;
    public MoveNegative negativeY;
    public MoveNegative negativeZ;

    public const float moveSpeedY = 5.0f;
    public const float moveSpeedZ = 5.0f;
    public const float moveSpeedX = 5.0f;

    public const float doudongOffset = 20.0f;



    private int DouCount;

    public enum MoveNegative
    {
        MIN,
        MAX,
        None
    }



    // Use this for initialization
    void Start()
    { 
        Init();
    }


    public void DouDong(int i)
    {
        DouCount = i;
        XYDown();
    }

    public void XYDown()
    {
        DouCount--;
        negativeY = MoveNegative.MIN;
        negativeX = MoveNegative.MIN;
        moveTarY = initPos.transform.localPosition.y - doudongOffset;
        moveTarX = initPos.transform.localPosition.x - doudongOffset;
        if (DouCount <= 0)
        {
            moveTarY = initPos.transform.localPosition.y;
            moveTarX = initPos.transform.localPosition.x;
        }
    }

    public void XYUp()
    {
        DouCount--;
        negativeY = MoveNegative.MAX;
        negativeX = MoveNegative.MAX;
        moveTarY = initPos.transform.localPosition.y + doudongOffset;
        moveTarX = initPos.transform.localPosition.x + doudongOffset;
        if (DouCount <= 0)
        {
            moveTarY = initPos.transform.localPosition.y;
            moveTarX = initPos.transform.localPosition.x;
        }
    }

    public void MoveLevel(int level)
    {
        negativeZ = MoveNegative.MAX;
        switch (level)
        {
            case 1:
                moveTarZ = initPos.transform.localPosition.z + 20;
                break;
            case 2:
                moveTarZ = initPos.transform.localPosition.z + 40;
                break;
            case 3:
                moveTarZ = initPos.transform.localPosition.z + 80;
                break;
        }
    }

    public void Init()
    {
        this.transform.localPosition = initPos.transform.localPosition;
        negativeX = MoveNegative.None;
        negativeY = MoveNegative.None;
        negativeZ = MoveNegative.None;
        DouCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            //Y
            Vector3 vecy = this.transform.localPosition;
            //摄像机y轴运动方向
            if (negativeY == MoveNegative.MIN)
            {
                vecy.y -= moveSpeedY;
            }
            else if (negativeY == MoveNegative.MAX)
            {
                vecy.y += moveSpeedY;
            }

            if (negativeY == MoveNegative.MIN)
            {
                //摄像机y向小
                if (vecy.y < moveTarY)
                {
                    vecy.y = moveTarY;
                    this.transform.localPosition = vecy;
                }
                else
                {
                    this.transform.localPosition = vecy;
                }
            }
            else if (negativeY == MoveNegative.MAX)
            {
                //摄像机y向大
                if (vecy.y > moveTarY)
                {
                    vecy.y = moveTarY;
                    this.transform.localPosition = vecy;
                }
                else
                {
                    this.transform.localPosition = vecy;
                }
            }
            //摄像机X轴运动方向
            if (negativeX == MoveNegative.MIN)
            {
                vecy.x -= moveSpeedX;
            }
            else if (negativeX == MoveNegative.MAX)
            {
                vecy.x += moveSpeedX;
            }

            if (negativeX == MoveNegative.MIN)
            {
                //摄像机y向小
                if (vecy.x < moveTarX)
                {
                    //vecy.x = moveTarX;
                    //this.transform.localPosition = vecy;
                    if (DouCount <= 0)
                    {
                        negativeY = MoveNegative.None;
                        negativeX = MoveNegative.None;
                    }
                    else
                    {
                        XYUp();
                    }
                }
                else
                {
                    this.transform.localPosition = vecy;


                }
            }
            else if (negativeX == MoveNegative.MAX)
            {
                //摄像机y向大
                if (vecy.x > moveTarX)
                {
                    //vecy.x = moveTarX;
                    //this.transform.localPosition = vecy;
                    if (DouCount <= 0)
                    {
                        negativeY = MoveNegative.None;
                        negativeX = MoveNegative.None;
                    }
                    else
                    {
                        XYDown();
                    }
                }
                else
                {
                    this.transform.localPosition = vecy;

                }
            }


            //摄像机Z轴运动方向
            if (negativeZ == MoveNegative.MIN)
            {
                vecy.z -= moveSpeedZ;
            }
            else if (negativeZ == MoveNegative.MAX)
            {
                vecy.z += moveSpeedZ;
            }

            if (negativeZ == MoveNegative.MIN)
            {
                //摄像机y向小
                if (vecy.z < moveTarZ)
                {
                    negativeZ = MoveNegative.None;
                    vecy.z = moveTarZ;
                    this.transform.localPosition = vecy;
                }
                else
                {
                    this.transform.localPosition = vecy;
                }
            }
            else if (negativeZ == MoveNegative.MAX)
            {
                //摄像机y向大
                if (vecy.z > moveTarZ)
                {
                    negativeZ = MoveNegative.None;
                    vecy.z = moveTarZ;
                    this.transform.localPosition = vecy;
                }
                else
                {
                    this.transform.localPosition = vecy;
                }
            }
        }
    }
}
