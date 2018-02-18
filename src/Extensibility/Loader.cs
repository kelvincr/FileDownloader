// <copyright file="Loader.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Utility to load types with Mef.
    /// </summary>
    public static class Loader
    {
        private static readonly IList<Assembly> Assemblies = new List<Assembly>();
        private static readonly HashSet<string> LoadedAssemblies = new HashSet<string>();
        private static readonly ILogger Logger = AppLogger.LoggerFactory.CreateLogger("Loader");

        /// <summary>
        /// Initializes static members of the <see cref="Loader"/> class.
        /// </summary>
        static Loader()
        {
            ExtensionPaths = new List<string> { Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location) };
        }

        /// <summary>
        /// Gets or sets the extension paths.
        /// </summary>
        /// <value>
        /// The extension paths.
        /// </value>
        public static IList<string> ExtensionPaths { get; set; }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <typeparam name="T">Interface of desired type.</typeparam>
        /// <returns>An instance</returns>
        public static T Load<T>()
        {
            LoadAssembly(typeof(T).GetTypeInfo().Assembly.GetName());
            LoadAssembly(Assembly.GetCallingAssembly().GetName());
            var dlls = ExtensionPaths.SelectMany(path =>
                Directory.GetFiles(Path.GetFullPath(path), "*.dll")
                    .Select(Assembly.LoadFile));

            foreach (var dll in dlls)
            {
                LoadAssembly(dll.GetName());
            }

            var configuration = new ContainerConfiguration()
                .WithAssemblies(Assemblies);
            using (var container = configuration.CreateContainer())
            {
                container.TryGetExport<T>(out var instance);
                return instance;
            }
        }

        private static void LoadAssembly(AssemblyName refAssembly)
        {
            if (TryLoadAssembly(refAssembly, out var assembly))
            {
                Assemblies.Add(assembly);
                LoadedAssemblies.Add(refAssembly.FullName);
            }
        }

        private static bool TryLoadAssembly(AssemblyName assemblyName, out Assembly assembly)
        {
            if (!LoadedAssemblies.Contains(assemblyName.FullName))
            {
                try
                {
                    assembly = Assembly.Load(assemblyName);
                    return true;
                }
                catch (Exception e)
                {
                    Logger.LogWarning(string.Format(Resources.EWI002, e.Message, e.InnerException?.Message));
                }
            }

            assembly = null;
            return false;
        }
    }
}