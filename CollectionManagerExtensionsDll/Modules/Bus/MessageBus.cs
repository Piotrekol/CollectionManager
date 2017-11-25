using System;
using CollectionManager.Modules;

namespace CollectionManagerExtensionsDll.Modules
{
    public static class MessageBus
    {
        private static MessageBroker messageBroker;

        private static MessageBroker Broker
        {
            get
            {
                if (messageBroker == null)
                    messageBroker = new MessageBroker();
                return messageBroker;
            }
        }

        public static void Send<T>(T message)
        {
            Broker.Send(message);
        }

        public static TResult SendWait<T1, TResult>(T1 message)
        {
            return Broker.SendWait<T1, TResult>(message);
        }

        public static void Register<T>(Action<T> action)
        {
            Broker.Register(action);
        }

        public static void RegisterFunc<T0,T1>(Func<T0, T1> action)
        {
            Broker.RegisterFunc<T0,T1>(action);
        }

    }
}