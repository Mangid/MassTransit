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
namespace MassTransit.Transports.Loopback
{
    public class LoopbackTransportFactory :
        ITransportFactory
    {
        IMessageNameFormatter _messageNameFormatter;

        public LoopbackTransportFactory()
        {
            _messageNameFormatter = new DefaultMessageNameFormatter("::", "--", ":", "-");
        }

        public string Scheme
        {
            get { return "loopback"; }
        }

        public IDuplexTransport BuildLoopback(ITransportSettings settings)
        {
            return new LoopbackTransport(settings.Address);
        }

        public IInboundTransport BuildInbound(ITransportSettings settings)
        {
            return BuildLoopback(settings);
        }

        public IOutboundTransport BuildOutbound(ITransportSettings settings)
        {
            return BuildLoopback(settings);
        }

        public IOutboundTransport BuildError(ITransportSettings settings)
        {
            return new LoopbackTransport(settings.Address);
        }

        public IMessageNameFormatter MessageNameFormatter
        {
            get { return _messageNameFormatter; }
        }

        public void Dispose()
        {
        }
    }
}