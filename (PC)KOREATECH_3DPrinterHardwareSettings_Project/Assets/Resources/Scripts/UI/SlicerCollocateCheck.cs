using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlicerCollocateCheck
{
    // 6면 전부
    public static bool IsTargetInsideRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[8];

        //RightUpBack, RightUpFront, RightDownBack, RightDownFront
        vertex[0] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);
        vertex[2] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        //LeftUpBack, LeftUpFront, LeftDownBack, LeftDownFront
        vertex[4] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[5] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);
        vertex[6] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[7] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        for (int i=0; i<8;i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsTargetLeftSideInRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[4];

        //LeftUpBack, LeftUpFront, LeftDownBack, LeftDownFront
        vertex[0] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);
        vertex[2] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        for (int i = 0; i < 4; i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }
    public static bool IsTargetRightSideInRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[4];

        //RightUpBack, RightUpFront, RightDownBack, RightDownFront
        vertex[0] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);
        vertex[2] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        for (int i = 0; i < 4; i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }
    public static bool IsTargetBackSideInRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[4];

        //RightUpBack, RightDownBack
        vertex[0] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);

        //LeftUpBack, LeftDownBack
        vertex[2] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);

        for (int i = 0; i < 4; i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }
    public static bool IsTargetFrontSideInRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[4];

        //RightUpFront, RightDownFront
        vertex[0] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        //LeftUpFront, LeftDownFront
        vertex[2] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        for (int i = 0; i < 4; i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsTargetUpSideInRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[4];

        //RightUpBack, RightUpFront
        vertex[0] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        //LeftUpBack, LeftUpFront
        vertex[2] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y + target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        for (int i = 0; i < 4; i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsTargetDownSideInRange(Collider range, Collider target)
    {
        Vector3[] vertex = new Vector3[4];

        // RightDownBack, RightDownFront
        vertex[0] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[1] = new Vector3(target.bounds.center.x + target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        // LeftDownBack, LeftDownFront
        vertex[2] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z + target.bounds.size.z / 2);
        vertex[3] = new Vector3(target.bounds.center.x - target.bounds.size.x / 2, target.bounds.center.y - target.bounds.size.y / 2, target.bounds.center.z - target.bounds.size.z / 2);

        for (int i = 0; i < 4; i++)
        {
            if (!range.bounds.Contains(vertex[i]))
            {
                return false;
            }
        }

        return true;
    }
}
