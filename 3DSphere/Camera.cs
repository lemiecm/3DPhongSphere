using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    // Source: https://github.com/Walczi123/Computer-Graphics-Projects
    public class Camera
    {
        public  double width = 400;
        public  double height = 400;
        public  double PosX =0;
        public  double PosY =0;
        public  double PosZ =-300;
        public  double targetX = 0;
        public  double targetY = 0;
        public  double targetZ = 0;

        
        public  Matrix4x4 Matrix()
        {
            (double, double, double) cPos, cTarget, cUp, cX, cY, cZ, tmp;
            double length;
            cPos = (PosX,PosY,PosZ);
            cTarget = (targetX, targetY, targetZ);
            cUp = (0, 1, 0);
            //cZ
            tmp = (cPos.Item1 - cTarget.Item1, cPos.Item2 - cTarget.Item2, cPos.Item3 - cTarget.Item3);
            length = Operations.LengthOfVector(tmp);
            cZ = (tmp.Item1 / length, tmp.Item2 / length, tmp.Item3 / length);
            //cX
            tmp = Operations.CrossProdOfVectors(cUp, cZ);
            length = Operations.LengthOfVector(tmp);
            cX = (tmp.Item1 / length, tmp.Item2 / length, tmp.Item3 / length);
            //cY
            tmp = Operations.CrossProdOfVectors(cZ, cX);
            length = Operations.LengthOfVector(tmp);
            cY = (tmp.Item1 / length, tmp.Item2 / length, tmp.Item3 / length);
            //result
            Matrix4x4 result = new Matrix4x4((float)cX.Item1, (float)cX.Item2, (float)cX.Item3, (float)Operations.MultiplicationOfVectors(cX, cPos),
                                            (float)cY.Item1, (float)cY.Item2, (float)cY.Item3, (float)Operations.MultiplicationOfVectors(cY, cPos),
                                            (float)cZ.Item1, (float)cZ.Item2, (float)cZ.Item3, (float)Operations.MultiplicationOfVectors(cZ, cPos),
                                            0, 0, 0, 1);
            return result;
        }
    }
}
