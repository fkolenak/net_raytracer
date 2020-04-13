using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    class BranchTree
    {
        Light light;

        public BranchTree()
        {
           
            light = new Light(0,2,-1);
            
        }
        
        public void setLight(int xPos, int yPos, int zPos)
        {
            this.light.setLight(xPos, yPos, zPos);
        }
    }
}
