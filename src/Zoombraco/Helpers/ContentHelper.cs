// <copyright file="ContentHelper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Our.Umbraco.Ditto;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;

    using Zoombraco.Models;

    /// <summary>
    /// Provides optimized helper methods to return strong type models from the Umbraco back office.
    /// </summary>
    public class ContentHelper
    {
        /// <summary>
        /// The collection of registered types.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Type> RegisteredTypes
            = new ConcurrentDictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Initializes static members of the <see cref="ContentHelper"/> class.
        /// </summary>
        static ContentHelper()
        {
            List<Type> registerTypes = PluginManager.Current.ResolveTypes<Page>().ToList();
            registerTypes.AddRange(PluginManager.Current.ResolveTypes<Component>());

            foreach (Type type in registerTypes)
            {
                RegisterType(type);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHelper"/> class.
        /// </summary>
        /// <param name="helper">The Umbraco helper</param>
        public ContentHelper(UmbracoHelper helper)
        {
            this.UmbracoHelper = helper;
        }

        /// <summary>
        /// Gets the <see cref="UmbracoHelper"/> for querying published content or media.
        /// </summary>
        public UmbracoHelper UmbracoHelper { get; }

        /// <summary>
        /// Registers the given type to allow conversion.
        /// </summary>
        /// <param name="type">The type to register.</param>
        /// <param name="alias">Any alias for the given type.</param>
        public static void RegisterType(Type type, string alias = null)
        {
            RegisteredTypes.GetOrAdd(type.Name, t => type);
        }

        /// <summary>
        /// Gets the node matching the given id.
        /// </summary>
        /// <param name="nodeId">The id of the node to return.</param>
        /// <returns>
        /// The <see cref="Page"/> matching the id.
        /// </returns>
        public Page GetById(int nodeId) => this.GetById<Page>(nodeId);

        /// <summary>
        /// Gets the node matching the given id.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the node to return.</param>
        /// <returns>
        /// The given <see cref="Type"/> instance matching the id.
        /// </returns>
        public T GetById<T>(int nodeId)
            where T : class
        {
            return this.FilterAndParse<T>(this.UmbracoHelper.TypedContent(nodeId));
        }

        /// <summary>
        /// Gets the nodes matching the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="rootId">
        /// The id of the root node to start from. If not given the method will return
        /// all nodes matching the type.
        /// </param>
        /// <param name="level">The level to search.</param>
        /// <returns>
        /// The nodes matching the given <see cref="Type"/>.
        /// </returns>
        public IEnumerable<T> GetByNode<T>(int rootId = 0, int level = int.MaxValue)
            where T : class
        {
            // Get only the root and descendants of the given root.
            if (rootId > 0)
            {
                if (level == int.MaxValue)
                {
                    return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContent(rootId).Descendants());
                }

                return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContent(rootId).Descendants(level));
            }

            // Get all the nodes from all root nodes.
            List<T> nodes = new List<T>();
            foreach (IPublishedContent collection in this.UmbracoHelper.TypedContentAtRoot())
            {
                if (level == int.MaxValue)
                {
                    nodes.AddRange(this.FilterAndParseCollection<T>(collection.Descendants()));
                }

                nodes.AddRange(this.FilterAndParseCollection<T>(collection.Descendants(level)));
            }

            return nodes.AsEnumerable();
        }

        /// <summary>
        /// Gets the root nodes as instances of <see cref="Page"/>.
        /// </summary>
        /// <returns>
        /// The root node as an instance of <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetRootNodes() => this.GetRootNodes<Page>();

        /// <summary>
        /// Gets the root nodes matching the given type.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns>
        /// The root nodes matching the given <see cref="Type"/>.
        /// </returns>
        public IEnumerable<T> GetRootNodes<T>()
        {
            return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContentAtRoot());
        }

        /// <summary>
        /// Gets the first root node in the current site as an instance of <see cref="Page"/>.
        /// </summary>
        /// <returns>
        /// The root node as an instance of <see cref="Page"/>.
        /// </returns>
        public Page GetRootNode() => this.GetRootNode<Page>();

        /// <summary>
        /// Gets the first root node in the current site matching the given type.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns>
        /// The root node matching the given <see cref="Type"/>.
        /// </returns>
        public T GetRootNode<T>()
        {
            // We want to get the current site only.
            return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContentAtRoot()
                       .Where(p => p.UrlAbsolute().StartsWith(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)))).FirstOrDefault();
        }

        /// <summary>
        /// Gets the parent of the current instance as an instance of <see cref="Page"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <returns>
        /// The parent as an instance of <see cref="Page"/>.
        /// </returns>
        public Page GetParent(int nodeId) => this.GetParent<Page>(nodeId);

        /// <summary>
        /// Gets the parent of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <returns>
        /// The parent as an instance of the given <see cref="Type"/>.
        /// </returns>
        public T GetParent<T>(int nodeId)
        {
            return this.FilterAndParse<T>(this.UmbracoHelper.TypedContent(nodeId).Parent);
        }

        /// <summary>
        /// Gets the ancestors of the current instance as an <see cref="IEnumerable{Page}"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="maxLevel">The maximum level to search.</param>
        /// <returns>
        /// The <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetAncestors(int nodeId, int maxLevel = int.MaxValue) => this.GetAncestors<Page>(nodeId, maxLevel);

        /// <summary>
        /// Gets the ancestors of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="maxLevel">The maximum level to search.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetAncestors<T>(int nodeId, int maxLevel = int.MaxValue)
        {
            return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContent(nodeId).Ancestors(maxLevel));
        }

        /// <summary>
        /// Gets the children of the current instance as an <see cref="IEnumerable{Page}"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <returns>
        /// The <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetChildren(int nodeId) => this.GetChildren<Page>(nodeId);

        /// <summary>
        /// Gets the children of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetChildren<T>(int nodeId)
        {
            return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContent(nodeId).Children);
        }

        /// <summary>
        /// Gets the descendants of the current instance as an <see cref="IEnumerable{Page}"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="level">The level to search.</param>
        /// <returns>
        /// The <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetDescendants(int nodeId, int level = 0) => this.GetDescendants<Page>(nodeId, level);

        /// <summary>
        /// Gets the descendants of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="level">The level to search.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetDescendants<T>(int nodeId, int level = 0)
        {
            if (level == int.MaxValue)
            {
                return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContent(nodeId).Descendants());
            }

            return this.FilterAndParseCollection<T>(this.UmbracoHelper.TypedContent(nodeId).Descendants(level));
        }

        /// <summary>
        /// Gets the list of stored types.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{Type}"/> containing the registered types.
        /// </returns>
        public IEnumerable<Type> GetRegisteredTypes()
        {
            return RegisteredTypes.Values.ToArray();
        }

        /// <summary>
        /// Gets the stored type matching the given name.
        /// </summary>
        /// <param name="name">The name of the type to retrieve.</param>
        /// <returns>
        /// The stored <see cref="Type"/>.
        /// </returns>
        public Type GetRegisteredType(string name)
        {
            Type type;
            RegisteredTypes.TryGetValue(name, out type);
            return type;
        }

        /// <summary>
        /// Filters and parses a given item from the published content. If an interface is passed the type implementing that
        /// interface will be returned.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="content">The <see cref="IPublishedContent"/>.</param>
        /// <returns>
        /// The given type instance.
        /// </returns>
        private T FilterAndParse<T>(IPublishedContent content)
        {
            if (content == null)
            {
                return default(T);
            }

            Type returnType = typeof(T);

            // Filter the collection if necessary by our specific type.
            if (!this.IsInheritableType(returnType))
            {
                Type type = this.GetRegisteredType(content.DocumentTypeAlias);

                if (type != null)
                {
                    object meta = content.As(type);
                    if (meta != null)
                    {
                        return (T)meta;
                    }
                }
            }
            else
            {
                // If we are passed an interface or a base type then we want to return all types that implement that interface or type.
                Type type = this.GetRegisteredType(content.DocumentTypeAlias);

                if (type != null && returnType.IsAssignableFrom(type))
                {
                    object meta = content.As(type);
                    if (meta != null)
                    {
                        return (T)meta;
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Filters and parses the collection of published content to return a collection of the given type.
        /// If an interface is passed then types implementing that interface will be returned.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="contentList">The collection of <see cref="IPublishedContent"/></param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        private IEnumerable<T> FilterAndParseCollection<T>(IEnumerable<IPublishedContent> contentList)
        {
            // Prevent multiple enumeration.
            IEnumerable<IPublishedContent> publishedContent = contentList as IPublishedContent[] ?? contentList.ToArray();
            if (!publishedContent.Any())
            {
                yield break;
            }

            Type returnType = typeof(T);

            // Filter the collection if necessary by our specific type.
            if (!this.IsInheritableType(returnType))
            {
                publishedContent = publishedContent.Where(c => c.DocumentTypeAlias.InvariantEquals(returnType.Name));

                foreach (IPublishedContent content in publishedContent)
                {
                    Type type = this.GetRegisteredType(content.DocumentTypeAlias);

                    if (type != null)
                    {
                        object meta = content.As(type);
                        if (meta != null)
                        {
                            yield return (T)meta;
                        }
                    }
                }
            }
            else
            {
                // If we are passed an interface or a base type then we want to return all types that implement that interface or type.
                foreach (IPublishedContent content in publishedContent)
                {
                    Type type = this.GetRegisteredType(content.DocumentTypeAlias);

                    if (type != null && returnType.IsAssignableFrom(type))
                    {
                        object meta = content.As(type);
                        if (meta != null)
                        {
                            yield return (T)meta;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether a type is an interface or one of our known base types.
        /// </summary>
        /// <param name="returnType">The type.</param>
        /// <returns>The <see cref="bool"/></returns>
        private bool IsInheritableType(Type returnType)
        {
            bool isInterface = returnType.IsInterface;
            bool isPage = returnType == typeof(Page);
            bool isComponent = returnType == typeof(Component);

            return isInterface || isPage || isComponent;
        }
    }
}