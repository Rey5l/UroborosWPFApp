using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UroborosApp.Model
{
    public class Entities
    {
        private static UroborosDBEntities _context;
        public static UroborosDBEntities GetContext()
        {
            if (_context == null)
                _context = new UroborosDBEntities();
            return _context;
        }
    }
}
