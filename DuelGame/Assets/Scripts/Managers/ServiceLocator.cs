using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuelGame
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public static T Get<T>()
        {
            return (T)_services[typeof(T)];
        }
    }
}

