using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grafika3d.Math;
using Grafika3d.Draw;

namespace Grafika3d.Draw
{
    class Models
    {
         static string path = "modele.obj";
       
      
        Mesh meshModels = Import.ImportFromFile(path);
        public Mesh meshModelsProject;
         
        public Models(Matrix4x4 matWorld,Matrix4x4 matView,Matrix4x4 matpro, Vector3 Camera)
        {
            this.meshModelsProject = new Mesh();
        

            Vector3 temp1 = new Vector3(), temp2 = new Vector3(), temp3 = new Vector3();

          
            //lista posortowana 
            List<Triangle> sortToRaster = new List<Triangle>();
            
        

            Vector3 normal = new Vector3(), line1 = new Vector3(), line2 = new Vector3();
            foreach (Triangle item in meshModels.lista)
            {

                Triangle transformed = new Triangle(temp1, temp2, temp3);
                Triangle translate = new Triangle(temp1, temp2, temp3);
                Triangle projected = new Triangle(temp1, temp2, temp3);
                Triangle triView = new Triangle(temp1, temp2, temp3);
               
                transformed.v1 = Vector3.Matrix_MultiplyVector(matWorld, item.v1);
                transformed.v2 = Vector3.Matrix_MultiplyVector(matWorld, item.v2);
                transformed.v3 = Vector3.Matrix_MultiplyVector(matWorld, item.v3);

                //obliczanie normalych
                line1 = Vector3.Vector_Sub(transformed.v2, transformed.v1);
                line2 = Vector3.Vector_Sub(transformed.v3, transformed.v1);

                normal = Vector3.Vectro_CrossPd(line1, line2);

                normal = Vector3.Vector_Normalise(normal);

            
                //normalne by wyswietlac te sciany co powinniśmy 
                Vector3 cameraRay = Vector3.Vector_Sub(transformed.v1, Camera);

                if(Vector3.Vector_DotProduct(normal,cameraRay)<0.0f)
                {
                    //light
                    Vector3 light = new Vector3(0.0f, 0.0f, -1.0f);
                    //rotacja światła wraz z sceną
                    light = Vector3.Matrix_MultiplyVector(matWorld, light);
                    light = Vector3.Vector_Normalise(light);

                    //iloczyn skalarny

                   
                     float dp = System.Math.Max(0.1f, Vector3.Vector_DotProduct(light, normal));
                   


                    //przestrzen swiata--> przestrzen projektowana 
                    triView.v1 = Vector3.Matrix_MultiplyVector(matView, transformed.v1);
                    triView.v2 = Vector3.Matrix_MultiplyVector(matView, transformed.v2);
                    triView.v3 = Vector3.Matrix_MultiplyVector(matView, transformed.v3);

                    //przycinanie trójkątów jeżeli sa za wielkie i za blisko ekranu
                    Vector3 plane_p = new Vector3(0.0f, 0.0f, 0.1f);
                    Vector3 plane_n = new Vector3(0.0f, 0.0f, 1.0f);
                    List<Triangle> toClip = Triangle.ClipAgainstPlane(plane_p,plane_n,triView);
                    int clippedTri = toClip.Count;
                    for (int i = 0; i < clippedTri; i++)
                    {


                        projected.v1 = Vector3.Matrix_MultiplyVector(matpro, toClip[i].v1);
                        projected.v2 = Vector3.Matrix_MultiplyVector(matpro, toClip[i].v2);
                        projected.v3 = Vector3.Matrix_MultiplyVector(matpro, toClip[i].v3);

                        projected.v1 = Vector3.Vector_Div(projected.v1, projected.v1.w);
                        projected.v2 = Vector3.Vector_Div(projected.v2, projected.v2.w);
                        projected.v3 = Vector3.Vector_Div(projected.v3, projected.v3.w);

                        projected.v1.x *= -1.0f;
                        projected.v1.y *= -1.0f;
                        projected.v2.x *= -1.0f;
                        projected.v2.y *= -1.0f;
                        projected.v3.x *= -1.0f;
                        projected.v3.y *= -1.0f;

                        //początkowa translacja sceny 
                        Vector3 offserView = new Vector3(1.0f, 2.0f, 1.0f);
                        projected.v1 = Vector3.Vector_Add(projected.v1, offserView);
                        projected.v2 = Vector3.Vector_Add(projected.v2, offserView);
                        projected.v3 = Vector3.Vector_Add(projected.v3, offserView);
                        //skalowanie sceny 
                        projected.v1.x *= 0.5f * (float)1024;
                        projected.v1.y *= 0.5f * (float)768;

                        projected.v2.x *= 0.5f * (float)1024;
                        projected.v2.y *= 0.5f * (float)768;

                        projected.v3.x *= 0.5f * (float)1024;
                        projected.v3.y *= 0.5f * (float)768;

                      
                        projected.dp = dp;
                       
                         sortToRaster.Add(projected);
                        
                    }

                   
                }

                
            }
            List<Triangle> listtri = new List<Triangle>();
            foreach (Triangle item in sortToRaster)
            {
                //ucinanie dla czterech krawędzi okna
                listtri.Clear();
                listtri.Add(item);
                int newTri = 1;
                for (int i = 0; i < 4; i++)
                {
                    
                    while(newTri>0)
                    {
                        //pierwsza z kolejki
                        Triangle t = listtri.First();
                        listtri.RemoveAt(0);
                        newTri--;
                        List<Triangle> clippedTriangles = new List<Triangle>();
                        switch (i)
                        {
                            case 0:
                                clippedTriangles = Triangle.ClipAgainstPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), t);
                                break;
                            case 1:
                                clippedTriangles = Triangle.ClipAgainstPlane(new Vector3(0.0f, 768-1, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), t);
                                break;
                            case 2:
                                clippedTriangles = Triangle.ClipAgainstPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), t);
                                break;
                            case 3:
                                clippedTriangles = Triangle.ClipAgainstPlane(new Vector3(1024-1, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f), t);
                                break;
                        }
                        foreach (Triangle item2 in clippedTriangles)
                        {
                            listtri.Add(item2);
                        }

                    }
                    newTri = listtri.Count;
                }
             
                foreach (Triangle item3 in listtri)
                {
                    meshModelsProject.Add(item3);
                }
                
                
            }
            
            meshModelsProject.Sort();
          
        }
    }
}
