using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Grafika3d.Draw;
using Grafika3d.Math;

namespace Grafika3d
{
     class Import
    {
        static public Mesh ImportFromFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            StreamReader sr1 = new StreamReader(path);
            string s;
            string[] line = new string[4];
            Mesh mesh = new Mesh();
            List<Triangle> list = new List<Triangle>();
            List<Vector3> vec = new List<Vector3>();
           
           
            while ((s =sr.ReadLine()) != null)
            {

                line=s.Split(' ');
              
                if(line[0]=="v")
                {
                    Vector3 vector = new Vector3(0.0f,0.0f,0.0f);             
                    vector.x = float.Parse(line[1], System.Globalization.CultureInfo.InvariantCulture);
                    vector.y = float.Parse(line[2], System.Globalization.CultureInfo.InvariantCulture);
                    vector.z = float.Parse(line[3], System.Globalization.CultureInfo.InvariantCulture);
                    vec.Add(vector);
                }

            }
            while ((s = sr1.ReadLine()) != null)
            {

                line = s.Split(' ');
                int pom1,pom2,pom3;
                if (line[0] == "f")
                {
                    pom1 = int.Parse(line[1]);
                    pom2 = int.Parse(line[2]);
                    pom3 = int.Parse(line[3]);
                    pom1--;
                    pom2--;
                    pom3--;
                    Triangle t = new Triangle(vec[pom1], vec[pom2], vec[pom3]);
                    mesh.Add(t);
                }

            }

            sr.Close();
            sr1.Close();
            return mesh;
        }
    }
}
