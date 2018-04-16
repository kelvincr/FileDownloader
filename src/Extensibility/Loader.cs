﻿// <copyright file="Loader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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

    public static class Loader
    {

        public static IList<string> ExtensionPaths { get; set; }

        private static IList<Assembly> Assemblies = new List<Assembly>();
        private static HashSet<string> LoadedAssemblies = new HashSet<string>();
        private static readonly ILogger Logger = AppLogger.LoggerFactory.CreateLogger("Loader");


        static Loader()
        {
            ExtensionPaths = new List<string> { "./" };
        }

        private static void LoadAssembly(AssemblyName refAssembly)
        {
            if (TryLoadAssembly(refAssembly, out var assembly))
            {
                Assemblies.Add(assembly);
                LoadedAssemblies.Add(refAssembly.FullName);
            }
        }

        public static T Load<T>()
        {
            LoadAssembly(typeof(T).GetTypeInfo().Assembly.GetName());
            LoadAssembly(Assembly.GetCallingAssembly().GetName());
            var dlls = ExtensionPaths.SelectMany(path => Directory.GetFiles(Path.GetFullPath(path), "*.dll").Select(Assembly.LoadFile));

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