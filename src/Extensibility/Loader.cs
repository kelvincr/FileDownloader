// <copyright file="Loader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Extensibility
{
    using System.Collections.Generic;
    using System.Composition.Hosting;
    using System.Reflection;

    public class Loader
    {
        public IEnumerable<T> Load<T>()
        {
            var assemblies = new[] {typeof(T).GetTypeInfo().Assembly};
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);
            using (var container = configuration.CreateContainer())
            {
                return container.GetExports<T>();
            }
        }
    }
}