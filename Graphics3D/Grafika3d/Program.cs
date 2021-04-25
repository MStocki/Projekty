using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Grafika3d.Math;
using Grafika3d.Draw;
using System.Diagnostics;


namespace Grafika3d
{
    class Program
    { 
        
       
        static Clock clock = new Clock();
        //macierz projekcji
        static Matrix4x4 projectionMatrix = new Matrix4x4();
        //macierze obrotów
        static Matrix4x4 matrotZ=new Matrix4x4(), matRotX=new Matrix4x4(),matrotY=new Matrix4x4();
        //macierz łaczna obrotu 
        static Matrix4x4 matworld = new Matrix4x4();
        //macierz obrotu kamery
        static Matrix4x4 matcamrot = new Matrix4x4();
        //macierz kamery 
        static Matrix4x4 matcam = new Matrix4x4();
        //macierz przestrzeni widzianej ( odwrócona macierz kamery 
        static Matrix4x4 matview = new Matrix4x4();
        static ContextSettings context = new ContextSettings();
        static RenderWindow window;
        static Scena scena = new Scena();
        //kąty obrotu
        static double Xalpha = 2 * System.Math.PI / 10;
        static double Yalpha = 2 * System.Math.PI / 10;
        static double Zalpha = 2 * System.Math.PI / 10;
        //kamera
        static Vector3 camera= new Vector3(0.0f,0.0f,-15.0f);
        //kierunek poglądu        
         static Vector3 lookDir = new Vector3(0.0f,0.0f,1.0f);
        //kierunek przyblizania się kamery
        static Vector3 Forward = new Vector3();
        static float Yaw = 0.0f;
        static Models modele;
     

        static void Main(string[] args)
        {
            clock.Restart();
            projectionMatrix = Matrix4x4.ProjectrionMatrix(90.0f, 1024 / 768, 0.1f, 1000.0f);


            matrotZ = Matrix4x4.RotateZ(Zalpha);
            matRotX = Matrix4x4.RotateX(Xalpha);
            matrotY = Matrix4x4.RotateY(Yalpha);
        
            matworld = Matrix4x4.MakeIdentity();
            matworld = Matrix4x4.Multiply(matrotZ, matRotX);
            matworld = Matrix4x4.Multiply(matworld, matrotY);
    
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 target = new Vector3(0.0f, 0.0f, 5.0f);


            matcamrot = Matrix4x4.RotateY(Yaw);
            lookDir = Vector3.Matrix_MultiplyVector(matcamrot, target);
            target = Vector3.Vector_Add(camera, lookDir);
            matcam = Matrix4x4.Matrix_PointAt(camera, target, up);
            matview = Matrix4x4.QuickInverse(matcam);

            
            window = new RenderWindow(new VideoMode(1024, 768), "Grafika Michał Stocki", Styles.Default, context);
            
            window.Closed += Window_Closed;
            window.Resized += Window_Resized;
            window.KeyPressed += Window_KeyPressed;    

            modele = new Models(matworld, matview, projectionMatrix, camera);

            scena.Add(modele.meshModelsProject);
         
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw(scena);
                window.Display();

            }
            
        }  
        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {

            Forward = Vector3.Vector_Mul(lookDir, 1.01f);
          
            if (e.Code == Keyboard.Key.Up)
            {
           
                camera.y += 0.1f;

                    
            }
            if (e.Code == Keyboard.Key.Down)
            {
               
               camera.y -= 0.1f;
            }
            if (e.Code == Keyboard.Key.Left)
            {
                
               camera.x -= 8.0f * clock.ElapsedTime.AsSeconds();
            }
            if (e.Code == Keyboard.Key.Right)
            {
                camera.x += 8.0f * clock.ElapsedTime.AsSeconds();
            }
            //Dodatkowy obrót kamery oś Y , wyłączona do pokazu 
            //if (e.Code == Keyboard.Key.A)
            //{
                
            //    Yaw -= 0.5f * clock.ElapsedTime.AsSeconds();
                         
            //}
            //if (e.Code == Keyboard.Key.D)
            //{
            //    Yaw += 0.5f * clock.ElapsedTime.AsSeconds();
                
            //}

            if (e.Code == Keyboard.Key.W)
            {
                camera = Vector3.Vector_Add(camera, Forward);
             
            }
            if (e.Code == Keyboard.Key.S)
            {
                camera = Vector3.Vector_Sub(camera, Forward);
            }

            if (e.Code == Keyboard.Key.Z)
            {
                Zalpha += 0.1f;
            }
            if (e.Code == Keyboard.Key.X)
            {
                Xalpha += 0.1f;
            }
            if (e.Code == Keyboard.Key.Y)
            {
                Yalpha += 0.1f;

            }
            clock.Restart();
            matrotZ = Matrix4x4.RotateZ(Zalpha);
            matRotX = Matrix4x4.RotateX(Xalpha);
            matrotY = Matrix4x4.RotateY(Yalpha);
            matworld = Matrix4x4.MakeIdentity();
            matworld = Matrix4x4.Multiply(matrotZ, matRotX);
            matworld = Matrix4x4.Multiply(matworld,matrotY);
       
            Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 target = new Vector3(0.0f, 0.0f, 1.0f);

            matcamrot = Matrix4x4.RotateY(Yaw);
            lookDir = Vector3.Matrix_MultiplyVector(matcamrot, target);
            target = Vector3.Vector_Add(camera, lookDir);
            matcam = Matrix4x4.Matrix_PointAt(camera, target, up);
            matview = Matrix4x4.QuickInverse(matcam);
            modele = new Models(matworld,matview,projectionMatrix,camera);
            scena.objectMeshList.RemoveAt(0);
            scena.Add(modele.meshModelsProject);
        }

        private static void Window_Resized(object sender, SizeEventArgs e)
        {
            View newView = new View(new FloatRect(0, 0, e.Width, e.Height));
            window.SetView(newView);
            
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
