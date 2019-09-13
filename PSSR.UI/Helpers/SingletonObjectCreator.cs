using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace PSSR.UI.Helpers
{
    public class SingletonObjectCreator
    {
        public readonly ConcurrentDictionary<string, Tuple<Guid, TimeSpan>> _currentprojectToUser;

        public SingletonObjectCreator()
        {
            _currentprojectToUser = new ConcurrentDictionary<string, Tuple<Guid, TimeSpan>>();
        }

        public static SingletonObjectCreator UniqueInstance
        {
            get { return ObjectCreator.UniqueInstance; }
        }

        class ObjectCreator
        {
            static ObjectCreator() { }
            internal static readonly SingletonObjectCreator UniqueInstance = new SingletonObjectCreator();
        }
    }
}
