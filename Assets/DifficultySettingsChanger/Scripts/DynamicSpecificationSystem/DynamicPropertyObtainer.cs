﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Timberborn.Common;
using UnityEngine;

namespace DifficultySettingsChanger
{
    public class DynamicPropertyObtainer
    {
        private readonly BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private readonly Type _serializedType = typeof(SerializeField);
        
        private readonly Dictionary<Type, FieldInfo[]> _fieldInfosCache = new();

        public DynamicProperty[] FromComponent(Type type, object instance)
        {
            return _fieldInfosCache
                .GetOrAdd(type, () => type.GetFields(_bindingFlags))
                .Where(field => field.IsDefined(_serializedType, true))
                .Where(ReflectionUtils.IsAllowedFieldType)
                .Where(ReflectionUtils.IsAllowedFieldName)
                .Select(field => new DynamicProperty(field.Name, field.GetValue(instance)))
                .ToArray();
        }
        
        public DynamicProperty[] FromSingleton(Type type, object instance)
        {
            return type
                .GetFields(_bindingFlags)
                .Where(ReflectionUtils.IsAllowedFieldType)
                .Where(ReflectionUtils.IsAllowedFieldName)
                .Where(info => !typeof(IDictionary).IsAssignableFrom(info.FieldType))
                .Select(property => new DynamicProperty(property.Name, property.GetValue(instance)))
                .ToArray();
        }
    }
}