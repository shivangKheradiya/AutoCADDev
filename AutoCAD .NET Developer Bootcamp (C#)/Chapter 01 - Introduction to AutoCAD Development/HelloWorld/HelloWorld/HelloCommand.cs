using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACADAS = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

namespace HelloWorld
{
    public class HelloCommand
    {
        [CommandMethod("HELLO")]
        public void Hello()
        {
            ACADAS.Application.ShowAlertDialog("Hello AutoCAD Developer!");
        }
    }
}
