// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Magnum.Extensions;
	using Saga;
	using Saga.SubscriptionConfigurators;
	using StructureMap;
	using StructureMapIntegration;
	using SubscriptionConfigurators;

	public static class StructureMapExtensions
	{
		public static void LoadFrom(this SubscriptionBusServiceConfigurator configurator, IContainer container)
		{
			IList<Type> concreteTypes = FindTypes<IConsumer>(container, x => !x.Implements<ISaga>());
			if (concreteTypes.Count > 0)
			{
				var consumerConfigurator = new StructureMapConsumerFactoryConfigurator(configurator, container);

				foreach (Type concreteType in concreteTypes)
				{
					consumerConfigurator.ConfigureConsumer(concreteType);
				}
			}

			IList<Type> sagaTypes = FindTypes<ISaga>(container, x => true);
			if (sagaTypes.Count > 0)
			{
				var sagaConfigurator = new StructureMapSagaFactoryConfigurator(configurator, container);

				foreach (Type type in sagaTypes)
				{
					sagaConfigurator.ConfigureSaga(type);
				}
			}
		}

		public static ConsumerSubscriptionConfigurator<TConsumer> Consumer<TConsumer>(
			this SubscriptionBusServiceConfigurator configurator, IContainer kernel)
			where TConsumer : class, IConsumer
		{
			var consumerFactory = new StructureMapConsumerFactory<TConsumer>(kernel);

			return configurator.Consumer(consumerFactory);
		}

		public static SagaSubscriptionConfigurator<TSaga> Saga<TSaga>(
			this SubscriptionBusServiceConfigurator configurator, IContainer kernel)
			where TSaga : class, ISaga
		{
			return configurator.Saga(kernel.GetInstance<ISagaRepository<TSaga>>());
		}

		static IList<Type> FindTypes<T>(IContainer container, Func<Type, bool> filter)
		{
			return container
				.Model
				.PluginTypes
				.Where(x => x.PluginType.Implements<T>())
				.Select(i => i.PluginType)
				.Concat(container.Model.InstancesOf<T>().Select(x => x.ConcreteType))
				.Where(filter)
				.Distinct()
				.ToList();
		}
	}
}