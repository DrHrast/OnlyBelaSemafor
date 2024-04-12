using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor
{
    internal class EventManager
    {
        public static void Subscribe<TEventArgs>(
        Action<object, TEventArgs> eventHandler,
        Action<object, TEventArgs> subscribeAction)
        {
            //subscribeAction(eventHandler);
        }

        public static void Unsubscribe<TEventArgs>(
            Action<object, TEventArgs> eventHandler,
            Action<object, TEventArgs> unsubscribeAction)
        {
            //unsubscribeAction(eventHandler);
        }
    }
}
