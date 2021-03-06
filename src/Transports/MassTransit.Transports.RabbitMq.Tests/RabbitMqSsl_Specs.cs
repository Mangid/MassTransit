﻿// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace MassTransit.Transports.RabbitMq.Tests
{
	using System;
	using Magnum.TestFramework;
	using NUnit.Framework;

	[Scenario]
	public class When_connecting_to_a_rabbit_mq_server_using_ssl
	{
		IServiceBus _bus;

		[When]
		public void Connecting_to_a_rabbit_mq_server_using_ssl()
		{
			Uri inputAddress = new Uri("rabbitmq://localhost:5671/test_queue");

			_bus = ServiceBusFactory.New(c =>
				{
					c.ReceiveFrom(inputAddress);
					c.UseRabbitMqRouting();
					c.UseRabbitMq(r =>
						{
							r.ConfigureHost(inputAddress, h =>
								{
									h.UseSsl(s =>
										{
											s.SetServerName(System.Net.Dns.GetHostName());
											s.SetCertificatePath("client.p12");
											s.SetCertificatePassphrase("Passw0rd");
										});
								});
						});
				});
		}

		[Finally]
		public void Finally()
		{
			_bus.Dispose();
		}

		[Then, Explicit]
		public void Should_connect_to_the_queue()
		{
			
		}

	}
}