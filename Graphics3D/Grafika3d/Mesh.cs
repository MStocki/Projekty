using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grafika3d.Draw;

namespace Grafika3d.Math
{
     class Mesh
    {
        public List<Triangle> lista;

        public Mesh()
        {
            this.lista = new List<Triangle>();
        }
        public void Add(Triangle t)
        {
            lista.Add(t);
        }
        public void Sort()
        {
            lista.Sort();
        }
        public override string ToString()
        {
            return lista.Count.ToString();
        }
    }
}
