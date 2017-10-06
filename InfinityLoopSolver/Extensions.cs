using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityLoopSolver
{
    public static class Extensions
    {
        private static uint _lastId;

        public static uint GetNewId()
        {
            return _lastId++;
        }
    }
}
