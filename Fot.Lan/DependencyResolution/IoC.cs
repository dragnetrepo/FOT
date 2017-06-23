// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Fot.Lan.Models;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;
using StructureMap.Web.Pipeline;

namespace Fot.Lan.DependencyResolution {
    public static class IoC {

        public static string str = @"metadata=res://*/Models.FotLan.csdl|res://*/Models.FotLan.ssdl|res://*/Models.FotLan.msl;provider=System.Data.SqlServerCe.4.0;provider connection string='Data Source=|DataDirectory|\FotLan.sdf;Max Database Size=4091;Password=EnterGodMode24790'";

        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                x.For<LanContext>().HttpContextScoped().Use<LanContext>().Ctor<string>("str").Is(str);
            });
            return ObjectFactory.Container;
        }
    }
}