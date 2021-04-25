using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SFML.Graphics;
using Grafika3d.Math;
using Grafika3d.Draw;

namespace Grafika3d
{
    class Engine
    {
        
      
        Color[,] Bitmap;
        
        public Engine(int X,int Y)
        {
          
            Bitmap = new Color[X, Y];
        }

        private void FillTriangle(Triangle triangle)
        {

            Vector3 temp;
            // Porządkujemy punkty wg współrzędnych y
            if (triangle.v2.y < triangle.v1.y)
            {
                temp = triangle.v1;
                triangle.v1 = triangle.v2;
                triangle.v2 = temp;
            }
            if (triangle.v3.y < triangle.v1.y)
            {
                temp = triangle.v1;
                triangle.v1 = triangle.v3;
                triangle.v3 = temp;
            }
            if (triangle.v3.y < triangle.v2.y)
            {
                temp = triangle.v2;
                triangle.v2 = triangle.v3;
                triangle.v3 = temp;
            }
            Vector3 A = triangle.v1;
            Vector3 B = triangle.v2;
            Vector3 C = triangle.v3;

           
            //płaski u dołu 
          
            if (B.y == C.y)
            {
                fillBottomFlatTriangle(A, B, C);
            }
          
            //płaski u góry 
            else if (A.y == B.y)
            {
                fillTopFlatTriangle(A, B, C);
            }
            else
            {
                //podział na płaski u góry i płaski u dołu
                float z = A.z + ((float)(B.y - A.y) / (float)(C.y - A.y)) * (C.z - A.z);

                Vector3 v = new Vector3(
                  (A.x + ((B.y - A.y) / (C.y - A.y)) * (C.x - A.x)), B.y, z);

                fillBottomFlatTriangle(A, B, v);
                fillTopFlatTriangle(B, v, C);
            }
            //płaski u dołu
            void fillBottomFlatTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
            {
                //nachylenia 
                float slope1 = (v2.x - v1.x) / (v2.y - v1.y);
                float slope2 = (v3.x - v1.x) / (v3.y - v1.y);

                float currentx1 = v1.x;
                float currentx2 = v1.x;

                for (int scanlineY = (int)v1.y; scanlineY <= v2.y; scanlineY++)
                {
                    float z1 = v1.z + ((scanlineY - v1.y) /(v2.y - v1.y)) * (v2.z - v1.z);
                    float z2 = v1.z + ((scanlineY - v1.y) /(v3.y - v1.y)) * (v3.z - v1.z);
                    Vector3 from = new Vector3(currentx1, (float)scanlineY, z1);
                    Vector3 to = new Vector3(currentx2, (float)scanlineY, z2);
                    DrawLine(from, to, triangle.dp);
                    currentx1 += slope1;
                    currentx2 += slope2;
                }
            }
            //płaski u góry 
            void fillTopFlatTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
            {
                //nachylenia
                float slope1 = (v3.x - v1.x) / (v3.y - v1.y);
                float slope2 = (v3.x - v2.x) / (v3.y - v2.y);

                float currentx1 = v3.x;
                float currentx2 = v3.x;

                for (int scanlineY = (int)v3.y; scanlineY > v1.y; scanlineY--)
                {
                    float z1 = v3.z + ((scanlineY - v3.y) / (v1.y - v3.y)) * (v1.z - v3.z);
                    float z2 = v3.z + ((scanlineY - v3.y) / (v2.y - v3.y)) * (v2.z - v3.z);
                    Vector3 from = new Vector3(currentx1, (float)scanlineY, z1);
                    Vector3 to = new Vector3(currentx2, (float)scanlineY, z2);

                    DrawLine(from, to, triangle.dp);
                    currentx1 -= slope1;
                    currentx2 -= slope2;
                }
            }


        }
        //rysowanie odcinka
        private void DrawLine(Vector3 from, Vector3 to, float dp, bool contur = false)
        {

            float x0 = from.x;
            float x1 = to.x;
            float y0 = from.y;
            float y1 = to.y;
            float z0 = from.z;
            float z1 = to.z;
            //funkcje pomocnicze
            int ipart(float x) { return (int)x; }
            int round(float x) { return ipart(x + 0.5f); }
            void swap(ref float o1, ref float o2)
            {
                float tmp = o1;
                o1 = o2;
                o2 = tmp;
            }
            //zamiana jeżeli nachylenie jest >1 
            bool krok = System.Math.Abs(y1 - y0) > System.Math.Abs(x1 - x0);
            if (krok)
            {
                swap(ref x0, ref y0);
                swap(ref x1, ref y1);
            }
            //jeżeli początek jest > koniec 
            if (x0 > x1)
            {
                swap(ref x0, ref x1);
                swap(ref y0, ref y1);
                swap(ref z0, ref z1);
            }

            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dx == 0 ? 1 : dy / dx;


            //poczatek
            int xEnd = round(x0);
            float yEnd = y0 + gradient * (xEnd - x0);
           
            int xPixel1 = xEnd;
            int yPixel1 = ipart(yEnd);



            if (krok)
            {
                //idziemy po Y
                ColorPixel(new Vector3(yPixel1, xPixel1, z0), dp, contur);
                ColorPixel(new Vector3(yPixel1 + 1, xPixel1, z0), dp, contur);
            }
            else
            {
                //idziemy po X
                ColorPixel(new Vector3(xPixel1, yPixel1, z0), dp, contur);
                ColorPixel(new Vector3(xPixel1, yPixel1 + 1, z0), dp, contur);
            }
            //pierwsze przeciecie y dla głownej pętli 
            float intery = yEnd + gradient;

            //koniec
            xEnd = round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
         
            int xPixel2 = xEnd;
            int yPixel2 = ipart(yEnd);


            if (krok)
            {
                //idziemy po Y
                ColorPixel(new Vector3(yPixel2, xPixel2, z1), dp, contur);
                ColorPixel(new Vector3(yPixel2 + 1, xPixel2, z1), dp, contur);
            }
            else
            {
                //idziemy po X
                ColorPixel(new Vector3(xPixel2, yPixel2, z1), dp, contur);
                ColorPixel(new Vector3(xPixel2, yPixel2 + 1, z1), dp, contur);
            }


            //pomiędzy
            //nachylenie >1 
            if (krok)
            {
                for (int x = (xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    //idziemy po Y głownie 
                    ColorPixel(new Vector3(ipart(intery), x, z1), dp, contur);
                    ColorPixel(new Vector3(ipart(intery) + 1, x, z1), dp, contur);
                    intery += gradient;
                }
            }
            else
            {

                for (int x = (xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                   
                    //idziemy po X głownie 
                    ColorPixel(new Vector3(x, ipart(intery), z1), dp, contur);
                    ColorPixel(new Vector3(x, ipart(intery) + 1, z1), dp, contur);
                    intery += gradient;
                }
            }
        }

        public void ColorPixel(Vector3 pixel, float dp,bool contur )
        {

            float r = 255.0f, g = 140.0f, b = 0.0f;
            r *= dp;
            g *= dp;
            b *= dp;
         
            Color color = new Color((byte)r, (byte)g, (byte)b);
           
         

            if (contur) color = Color.White;
            int X = (int)pixel.x;
            int Y = (int)pixel.y;
            if (X < 1023 && Y < 767 && X>=0 && Y>=0)  Bitmap[X ,Y] = color; 
        
        }
        public void Render(Scena scena, RenderTarget targer, RenderStates states)
        {
            foreach (Mesh item in scena.objectMeshList)
            {
                foreach (Triangle item2 in item.lista)
                {

                    FillTriangle(item2);
                   
                    //kontury 

                    // DrawLine(item2.v1, item2.v2,item2.dp,true);
                    //DrawLine(item2.v2, item2.v3,item2.dp,true);
                    //DrawLine(item2.v3, item2.v1,item2.dp,true);
                   
                   
                    
                }
            } 
            Image img = new Image(Bitmap);
            Texture text = new Texture(img);
            Sprite sprite = new Sprite(text);
            targer.Draw(sprite, states);

        }
    }
}
