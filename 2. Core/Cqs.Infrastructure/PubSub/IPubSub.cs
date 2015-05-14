using System;
using System.Collections.Generic;

namespace Cqs.Infrastructure.PubSub
{
    public interface IPubSub
    {
        bool WorkInAsync { get; set; }
        Dictionary<string, List<Action<object>>> Subscribers { get; }
        List<Action<object>> GetSubscribers(String topicName);
        void Subscribe(String topicName, Action<object> callback);
        void RemoveSubscriber(String topicName, Action<object> subscriberCallbackReference);
        void Publish(string topicName, object data);
    }
}