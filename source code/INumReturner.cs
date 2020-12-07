using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface INumReturner
    {
        void GiveNumber(int num);
        event EventHandler<int> Number;
    }
