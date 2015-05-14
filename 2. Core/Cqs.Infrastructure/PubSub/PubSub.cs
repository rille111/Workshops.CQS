using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cqs.Infrastructure.PubSub
{
    /// <summary>
    ///  A simple but effective internal message broker, it does not stand on any queue or persistence system.
    /// </summary>
    public class PubSub : IPubSub
    {
        private readonly object _locker = new object();

        private readonly Dictionary<string, List<Action<object>>> _subscribers = new Dictionary<string, List<Action<object>>>();

        /// <summary>
        /// Determine if .Publish should invoke listening actions in sync or async.
        /// </summary>
        public bool WorkInAsync { get; set; }

        public Dictionary<string, List<Action<object>>> Subscribers
        {
            get
            {
                lock (_locker)
                {
                    return _subscribers;
                }
            }

        }

        public List<Action<object>> GetSubscribers(String topicName)
        {
            lock (_locker)
            {
                if (Subscribers.ContainsKey(topicName))
                {
                    return Subscribers[topicName];
                }

                return null;
            }
        }

        public void Subscribe(String topicName, Action<object> callback)
        {
            lock (_locker)
            {
                if (Subscribers.ContainsKey(topicName))
                {
                    if (!Subscribers[topicName].Contains(callback))
                    {
                        Subscribers[topicName].Add(callback);
                    }
                }
                else
                {
                    var newCallBackList = new List<Action<object>> { callback };
                    Subscribers.Add(topicName, newCallBackList);
                }
            }

        }

        public void RemoveSubscriber(String topicName, Action<object> subscriberCallbackReference)
        {
            lock (_locker)
            {
                if (Subscribers.ContainsKey(topicName))
                {
                    if (Subscribers[topicName].Contains(subscriberCallbackReference))
                    {
                        Subscribers[topicName].Remove(subscriberCallbackReference);
                    }
                }
            }
        }

        public void Publish(string topicName, object data)
        {
            var subscribers = GetSubscribers(topicName);
            if (subscribers == null) 
                return;

            foreach (var action in subscribers)
            {
                if (WorkInAsync)
                    action.BeginInvoke(data, action.EndInvoke, null);
                else
                    action.Invoke(data);
            }
        }
    }
}
