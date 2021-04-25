using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika3d.Math
{
    class Vector3
    {
         public float x, y, z,w;

        public Vector3(float x = 0, float y = 0, float z = 0, float w = 1)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
       //mnożenie macierzy przez wektor 
        public static Vector3 Matrix_MultiplyVector(Matrix4x4 m, Vector3 v)
        {
            Vector3 v1 = new Vector3();
            v1.x = v.x * m.matrix[0,0] + v.y * m.matrix[1,0]+v.z*m.matrix[2,0]+v.w*m.matrix[3,0];
            v1.y= v.x * m.matrix[0, 1] + v.y * m.matrix[1, 1] + v.z * m.matrix[2, 1] + v.w * m.matrix[3, 1];
            v1.z= v.x * m.matrix[0, 2] + v.y * m.matrix[1, 2] + v.z * m.matrix[2, 2] + v.w * m.matrix[3, 2];
            v1.w= v.x * m.matrix[0, 3] + v.y * m.matrix[1, 3] + v.z * m.matrix[2, 3] + v.w * m.matrix[3, 3];
            return v1;
        }
        //dodawanie wektorów
        public static Vector3 Vector_Add(Vector3 v1, Vector3 v2)
        {
            Vector3 v3 = new Vector3();
            v3.x = v1.x + v2.x;
            v3.y = v1.y + v2.y;
            v3.z = v1.z + v2.z;
            return v3;
        }
        //odejmowanie wektorow
        public static Vector3 Vector_Sub(Vector3 v1, Vector3 v2)
        {
            Vector3 v3 = new Vector3();
            v3.x = v1.x - v2.x;
            v3.y = v1.y - v2.y;
            v3.z = v1.z - v2.z;
            return v3;
        }
        //mnożnie przez liczbe
        public static Vector3 Vector_Mul(Vector3 v1, float s)
        {
            Vector3 v3 = new Vector3();
            v3.x = v1.x * s;
            v3.y = v1.y *s;
            v3.z = v1.z *s;
            return v3;
        }
        //dzielenie przez liczbe
        public static Vector3 Vector_Div(Vector3 v1, float s)
        {
            Vector3 v3 = new Vector3();
            v3.x = v1.x /s;
            v3.y = v1.y /s;
            v3.z = v1.z /s;
            return v3;
        }
        //iloczyn skalarny
        public static float Vector_DotProduct(Vector3 v1, Vector3 v2)
        {
            return (v1.x * v2.x) +(v1.y*v2.y)+(v1.z*v2.z);
        }
        //długość
        public static float Vector_Lenght(Vector3 v1)
        {
            return (float)System.Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z);
        }
        //normalizacja
        public static Vector3 Vector_Normalise(Vector3 v)
        {
            float l = Vector_Lenght(v);
            Vector3 v2 = new Vector3();
            v2.x = v.x / l;
            v2.y = v.y / l;
            v2.z = v.z / l;
            return v2;
        }
        //iloczyn wektorowy 
        public static Vector3 Vectro_CrossPd(Vector3 v1, Vector3 v2)
        {
            Vector3 v3 = new Vector3();
            v3.x = (v1.y * v2.z) - (v1.z*v2.y);
            v3.y = (v1.z * v2.x) - (v1.x * v2.z);
            v3.z = (v1.x * v2.y) - (v1.y * v2.x);
            return v3;
        }
        //przecinanie płaszczyzny 
        public static Vector3 IntersectPlane(Vector3 plane_p, Vector3 plane_n,Vector3 linestart,Vector3 lineend)
        {
            plane_n = Vector_Normalise(plane_n);
            float plane_d = -Vector_DotProduct(plane_n, plane_p);
            float ad = Vector_DotProduct(linestart, plane_n);
            float bd = Vector_DotProduct(lineend, plane_n);
            float t = (-plane_d - ad) / (bd - ad);
            Vector3 lineStartToEnd = Vector_Sub(lineend, linestart);
            Vector3 lineToIntersect = Vector_Mul(lineStartToEnd, t);

            return Vector_Add(linestart, lineToIntersect);
        }

    }
}
