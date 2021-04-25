using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika3d.Math
{
    class Matrix4x4
    {
        public float[,] matrix;

        public Matrix4x4()
        {
            this.matrix = new float[4,4];
            for (int i = 0; i <4; i++)
            {
                for (int j = i; j < 4; j++)
                {
                    matrix[i, j] = 0.0f;
                }
            }
        }
        //macierz wskazująca na , pozycja kamery, cel, i kierunek góry 
        public static Matrix4x4 Matrix_PointAt(Vector3 position, Vector3 target, Vector3 up)
        {
            //nowy kierunek 'naprzód'
            Vector3 forward = Vector3.Vector_Sub(target, position);
            forward = Vector3.Vector_Normalise(forward);

            //nowy kierunek 'góra'

            Vector3 temp = Vector3.Vector_Mul(forward, Vector3.Vector_DotProduct(up, forward));
            Vector3 newUp = Vector3.Vector_Sub(up, temp);
            newUp = Vector3.Vector_Normalise(newUp);

            //nowy kierunek 'prawo'
            Vector3 newRight = Vector3.Vectro_CrossPd(newUp, forward);

            Matrix4x4 m = new Matrix4x4();
            m.matrix[0, 0] = newRight.x; m.matrix[0, 1] = newRight.y; m.matrix[0, 2] = newRight.z;
            m.matrix[1, 0] = newUp.x; m.matrix[1, 1] = newUp.y; m.matrix[1, 2] = newUp.z;
            m.matrix[2, 0] = forward.x; m.matrix[2, 1] = forward.y; m.matrix[2, 2] = forward.z;
            m.matrix[3, 0] = position.x; m.matrix[3, 1] = position.y; m.matrix[3, 2] = position.z; m.matrix[3, 3] = 1.0f;

            return m;
        }
        //odwracanie tylko dla tych macierzy z projektu
        public static Matrix4x4 QuickInverse(Matrix4x4 m)
        {
            Matrix4x4 wyn = new Matrix4x4();
            wyn.matrix[0, 0] = m.matrix[0, 0]; wyn.matrix[0, 1] = m.matrix[1, 0];wyn.matrix[0, 2] = m.matrix[2, 0];wyn.matrix[0, 3] = 0.0f;
            wyn.matrix[1, 0] = m.matrix[0, 1]; wyn.matrix[1, 1] = m.matrix[1, 1]; wyn.matrix[1, 2] = m.matrix[2, 1]; wyn.matrix[1, 3] = 0.0f;
            wyn.matrix[2, 0] = m.matrix[0, 2]; wyn.matrix[2, 1] = m.matrix[1, 2]; wyn.matrix[2, 2] = m.matrix[2, 2]; wyn.matrix[2, 3] = 0.0f;
            wyn.matrix[3, 0] = -(m.matrix[3,0]*wyn.matrix[0,0]+m.matrix[3,1]*wyn.matrix[1,0]+m.matrix[3,2]*wyn.matrix[2,0]);
            wyn.matrix[3, 1] = -(m.matrix[3, 0] * wyn.matrix[0, 1] + m.matrix[3, 1] * wyn.matrix[1, 1] + m.matrix[3, 2] * wyn.matrix[2, 1]);
            wyn.matrix[3, 2] = -(m.matrix[3, 0] * wyn.matrix[0, 2] + m.matrix[3, 1] * wyn.matrix[1, 2] + m.matrix[3, 2] * wyn.matrix[2, 2]);
            wyn.matrix[3, 3] = 1.0f;
           
            return wyn;
        }
        //macierz jednostkowa
        public static Matrix4x4 MakeIdentity()
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.matrix[0, 0] = 1.0f;
            matrix.matrix[1, 1] = 1.0f;
            matrix.matrix[2, 2] = 1.0f;
            matrix.matrix[3, 3] = 1.0f;
            return matrix;
            
        }
        public static Matrix4x4 RotateX(double a)
        {
            Matrix4x4 m = new Matrix4x4();
            m.matrix[0, 0] = 1;
            m.matrix[1, 1] = (float)System.Math.Cos(a);
            m.matrix[1, 2] = (float)System.Math.Sin(a);
            m.matrix[2, 1] = -(float)System.Math.Sin(a);
            m.matrix[2, 2] = (float)System.Math.Cos(a);
            m.matrix[3, 3] = 1;
            return m;
        }
        public static Matrix4x4 RotateY(double a)
        {
            Matrix4x4 m = new Matrix4x4();
            m.matrix[0, 0] = (float)System.Math.Cos(a);
            m.matrix[0, 2] = (float)System.Math.Sin(a);
            m.matrix[1, 1] = 1;
            m.matrix[2, 0] = -(float)System.Math.Sin(a);
            m.matrix[2, 2] = (float)System.Math.Cos(a);
            m.matrix[3, 3] = 1;
            return m;
        }
        public static Matrix4x4 RotateZ(double a)
        {
            Matrix4x4 m = new Matrix4x4();
            m.matrix[0, 0] = (float)System.Math.Cos(a);
            m.matrix[0, 1] = (float)System.Math.Sin(a);
            m.matrix[1, 0] = -(float)System.Math.Sin(a);
            m.matrix[1, 1] = (float)System.Math.Cos(a);
            m.matrix[2, 2] = 1;
            m.matrix[3, 3] = 1;
            return m;
        }
        public static Matrix4x4 Translation(float x,float y,float z)
        {
            Matrix4x4 m = new Matrix4x4();
            m.matrix[0, 0] = 1.0f;
            m.matrix[1, 1] = 1.0f;
            m.matrix[2, 2] = 1.0f;
            m.matrix[3, 3] = 1.0f;
            m.matrix[3, 0] = x;
            m.matrix[3, 1] = y;
            m.matrix[3, 2] = z;
            return m;
        }
        public static Matrix4x4 ProjectrionMatrix(float fovDeg, float aspectRatio, float near,float far)
        {
            double tan = fovDeg * 0.5f / 180.0f * System.Math.PI;
            float fFovRadio = (float)(1.0f / System.Math.Tan(tan));
            Matrix4x4 m = new Matrix4x4();
            m.matrix[0, 0] = aspectRatio*fFovRadio;
            m.matrix[1, 1] = fFovRadio;
            m.matrix[2, 2] = far / (far - near);
            m.matrix[3, 2] = (-far*near) / (far-near);
            m.matrix[2, 3] = 1.0f;
            m.matrix[3, 3] = 0.0f;
            return m;
        }
        public static Matrix4x4 Multiply(Matrix4x4 m1, Matrix4x4 m2)
        {
            Matrix4x4 wyn = new Matrix4x4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    wyn.matrix[j, i] = m1.matrix[j, 0] * m2.matrix[0, i] + m1.matrix[j, 1] * m2.matrix[1, i] + m1.matrix[j, 2] * m2.matrix[2, i] + m1.matrix[j, 3] * m2.matrix[3, i];
                }
            }
            return wyn;
        }
       
    }
}
