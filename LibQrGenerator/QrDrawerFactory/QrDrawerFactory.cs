using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibQrGenerator.Enums;

namespace LibQrGenerator.QrDrawerFactory
{
    public class QrDrawerFactory
    {
        public GenericDrawer GetDriver(DrawDriverType? DriverType = null)
        {

            if(DriverType == null)
            {
                //more cross platform frinedly?
                return new ImageSharp();
            }

            if(DriverType == DrawDriverType.SkiaSharp)
            {
                return new SkiaSharp();
            }
            else
            {
                return new ImageSharp();
            }
        }
    }
}
