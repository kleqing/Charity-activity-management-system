using Charity_Management_System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SingleOnBase<T> where T : class, new()
    {
        public DBContext _context = new DBContext();
        private static T _instace;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_instace == null)
                {
                    lock (_lock)
                    {
                        if (_instace == null)
                        {
                            _instace = new T();
                        }
                    }
                }
                return _instace;
            }
        }
    }
}

