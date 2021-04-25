using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grafika3d.Math;

namespace Grafika3d.Draw
{
    class Triangle:IComparable
    {
        public Vector3 v1, v2, v3;
        public float dp { get; set; }



        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3, float dp = 1.0f)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.dp = dp;

        }

        public  int CompareTo(object obj)
        {
            
            Triangle t = (Triangle)obj;
            float tempZ1 = (this.v1.z + this.v2.z + this.v3.z) / 3.0f;
            float tempZ2 = (t.v1.z + t.v2.z + t.v3.z) / 3.0f;
            //porownanie po v1 czy kolejnosc jest wlasciwa od najwiekszych Z do najmniejszych
            return (-1)*tempZ1.CompareTo(tempZ2);
           
        }
        //płaszczyzna punktu, płaszczynza normal
        //dzielenie trójąktów na mniejsze, nie wychodzące poza obszar widoku 
        public static List<Triangle> ClipAgainstPlane(Vector3 plane_p, Vector3 plane_n, Triangle tri)
        {
            List<Triangle> list = new List<Triangle>();
            
            //upewniamy się, że plane jest znormalizowane 
            plane_n = Vector3.Vector_Normalise(plane_n);
            float dist(Vector3 point)
            {
               
                return plane_n.x * point.x + plane_n.y * point.y + plane_n.z * point.z - Vector3.Vector_DotProduct(plane_n, plane_p);
            }
            //przechowujemy punkty na zewnątrz  i wewnątrz 
            Vector3[] inside_points = new Vector3 [ 3 ];
            Vector3[] outside_points = new Vector3[3];
            int nInPoint = 0;
            int nOutPoint = 0;

            //dystans od każdego punktu wierzchołka
            float d1 = dist(tri.v1);
            float d2 = dist(tri.v2);
            float d3 = dist(tri.v3);
            if (d1 >= 0)
            {
                inside_points[nInPoint] = tri.v1;
                nInPoint++;
            }
            else
            {
                outside_points[nOutPoint] = tri.v1;
                nOutPoint++;
            }
            if (d2 >= 0)
            {
                inside_points[nInPoint] = tri.v2;
                nInPoint++;
            }
            else
            {
                outside_points[nOutPoint] = tri.v2;
                nOutPoint++;
            }
            if (d3 >= 0)
            {
                inside_points[nInPoint] = tri.v3;
                nInPoint++;
            }
            else
            {
                outside_points[nOutPoint] = tri.v3;
                nOutPoint++;
            }
            //jeżeli 0 nInpoint to nie dzielimy na inne,nie rysujemy ich 
            if(nInPoint==3)
            {
                list.Add(tri);
            }
            if (nInPoint == 1 && nOutPoint == 2)
            {
                float dp = tri.dp;
                Vector3 v1 = inside_points[0];
                Vector3 v2 = Vector3.IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0]);
                Vector3 v3 = Vector3.IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[1]);
                Triangle temp = new Triangle(v1, v2, v3, dp);
                list.Add(temp);
            }
            if (nInPoint == 2 && nOutPoint == 1)
            {
                float dp = tri.dp;
                Vector3 v1 = inside_points[0];
                Vector3 v2 = inside_points[1];
                Vector3 v3 = Vector3.IntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0]);
                Vector3 v4 = inside_points[1];
                Vector3 v5 = v3;
                Vector3 v6 = Vector3.IntersectPlane(plane_p, plane_n, inside_points[1], outside_points[0]);
                Triangle temp = new Triangle(v1, v2, v3, dp);
                Triangle temp2 = new Triangle(v4, v5, v6, dp);
                list.Add(temp);
                list.Add(temp2);
            }
            return list;


        }
    }
}
