// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core
{
    internal sealed class CakeEngineActions
    {
        private readonly ICakeDataService _data;
        private readonly List<Action> _validations;

        public Action<ICakeContext> Setup { get; private set; }
        public Action<ITeardownContext> Teardown { get; private set; }
        public Action<ITaskSetupContext> TaskSetup { get; private set; }
        public Action<ITaskTeardownContext> TaskTeardown { get; private set; }
        public Type DataType { get; private set; }

        public CakeEngineActions(ICakeDataService data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _validations = new List<Action>();
        }

        public void RegisterSetup(Action<ICakeContext> action)
        {
            EnsureNotRegistered(Setup, "Setup");
            Setup = action;
            DataType = null;
        }

        public void RegisterSetup<TData>(Func<ICakeContext, TData> action)
            where TData : class
        {
            EnsureNotRegistered(Setup, "Setup");
            DataType = typeof(TData);
            Setup = context =>
            {
                _data.Set(action(context));
            };
        }

        public void RegisterTeardown(Action<ITeardownContext> action)
        {
            EnsureNotRegistered(Teardown, "Teardown");
            Teardown = action;
        }

        public void RegisterTeardown<TData>(Action<ITeardownContext, TData> action)
            where TData : class
        {
            EnsureNotRegistered(Teardown, "Teardown");
            RegisterValidationOfDataType<TData>("Teardown");
            Teardown = context =>
            {
                action(context, _data.Get<TData>());
            };
        }

        public void RegisterTaskSetup(Action<ITaskSetupContext> action)
        {
            EnsureNotRegistered(TaskSetup, "Task Setup");
            TaskSetup = action;
        }

        public void RegisterTaskSetup<TData>(Action<ITaskSetupContext, TData> action)
            where TData : class
        {
            EnsureNotRegistered(TaskSetup, "Task Setup");
            RegisterValidationOfDataType<TData>("Task Setup");
            TaskSetup = context =>
            {
                action(context, _data.Get<TData>());
            };
        }

        public void RegisterTaskTeardown(Action<ITaskTeardownContext> action)
        {
            EnsureNotRegistered(TaskTeardown, "Task Teardown");
            TaskTeardown = action;
        }

        public void RegisterTaskTeardown<TData>(Action<ITaskTeardownContext, TData> action)
            where TData : class
        {
            EnsureNotRegistered(TaskTeardown, "Task Teardown");
            RegisterValidationOfDataType<TData>("Task Teardown");
            TaskTeardown = context =>
            {
                action(context, _data.Get<TData>());
            };
        }

        public void Validate()
        {
            foreach (var validation in _validations)
            {
                validation();
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void EnsureNotRegistered(object value, string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (value != null)
            {
                throw new CakeException($"More than one {type.ToLowerInvariant()} action have been registered.");
            }
        }

        private void RegisterValidationOfDataType<TData>(string type)
        {
            // We want to postpone validation of data types until execution, so a script isn't
            // dependent on the order tasks are registered. This is why we check the data type
            // in an action and not directly.
            _validations.Add(() =>
            {
                // No data?
                if (DataType == null)
                {
                    throw new CakeException($"Trying to register a {type.ToLowerInvariant()} action that accepts data of type " +
                                            $"{typeof(TData).FullName}, but no such data has been setup.");
                }

                // Got data but of the wrong type?
                if (!DataType.IsAssignableFrom(typeof(TData)))
                {
                    throw new CakeException($"Trying to register a {type.ToLowerInvariant()} action that accepts data of type " +
                                            $"'{typeof(TData).FullName}', but no such data has been setup. " +
                                            $"There is available data registered of type '{DataType.FullName}' though, " +
                                            $"but it's not assignable from '{typeof(TData).FullName}'.");
                }
            });
        }
    }
}
