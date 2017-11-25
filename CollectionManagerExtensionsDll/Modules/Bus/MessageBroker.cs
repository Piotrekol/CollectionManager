using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace CollectionManagerExtensionsDll.Modules
{
    public class MessageBroker
    {
        private readonly Dictionary<Type, object> consumers;
        private readonly Dictionary<Type, object> consumersFunc;
        private readonly SynchronizationContext synchronizationContext;

        public MessageBroker()
        {
            consumers = new Dictionary<Type, object>();
            consumersFunc = new Dictionary<Type, object>();
            synchronizationContext = AsyncOperationManager.SynchronizationContext;
        }

        public void Send<T>(T message)
        {
            var key = typeof(T);
            if (!consumers.ContainsKey(key)) return;

            var list = consumers[key] as List<Action<T>>;
            list.ForEach(action =>
                synchronizationContext.Post(m => action((T)m), message));
        }

        //public T0 SendWait<T0, T1>(T1 message)
        //{
        //    var key = typeof(T1);
        //    if (!consumers.ContainsKey(key)) return default(T0);

        //    var list = (consumersFunc[key] as List<Func<T1, T0>>).First();
        //    var result = default(T0);
        //    synchronizationContext.Send(x =>
        //    {
        //        result = list(message);
        //    }, message);
        //    return result;
        //}
        public TResult SendWait<T1, TResult>(T1 arg1)
        {
            var key = typeof(T1);
            if (!consumersFunc.ContainsKey(key))
                return default(TResult);

            var func = (consumersFunc[key] as List<Func<T1, TResult>>).First();
            TResult retval = default(TResult);
            synchronizationContext.Send(new SendOrPostCallback((x) =>
                {
                    retval = func(arg1);
                })
                , null);
            return retval;
        }
        public void Register<T>(Action<T> action)
        {
            var key = typeof(T);
            List<Action<T>> list;
            if (!consumers.ContainsKey(key))
            {
                list = new List<Action<T>>();
                consumers.Add(key, list);
            }
            else
                list = consumers[key] as List<Action<T>>;
            list.Add(action);
        }
        public void RegisterFunc<T1, TResult>(Func<T1, TResult> action)
        {
            var key = typeof(TResult);
            List<Func<T1, TResult>> list;
            if (!consumersFunc.ContainsKey(key))
            {
                list = new List<Func<T1, TResult>>();
                consumersFunc.Add(key, list);
            }
            else
                list = consumersFunc[key] as List<Func<T1, TResult>>;
            list.Add(action);
        }
    }
}