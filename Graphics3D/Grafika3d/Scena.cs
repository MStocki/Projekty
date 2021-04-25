using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grafika3d.Math;
using SFML.Graphics;

namespace Grafika3d
{
    class Scena: Drawable
    {
        
        public List<Mesh> objectMeshList;
        
        public Scena()
        {
            this.objectMeshList = new List<Mesh>();
        }
        public void Add(Mesh M)
        { 
            objectMeshList.Add(M);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Engine engine = new Engine((int)target.Size.X, (int)target.Size.Y);
            engine.Render(this, target, states);
           

        }

        public override  string ToString()
        {
            return objectMeshList[0].ToString();
        }
    }
}
