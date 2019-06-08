// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root of EF 6 for license information.

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.TestDoubles
{
    internal class InMemoryAsyncQueryProvider : IQueryProvider, IDbAsyncQueryProvider
    {
        private static readonly MethodInfo _createQueryMethod
            = typeof(InMemoryAsyncQueryProvider).GetTypeInfo().DeclaredMethods
                                                .Single(m => m.IsGenericMethodDefinition && m.Name == nameof(InMemoryAsyncQueryProvider.CreateQuery));

        private static readonly MethodInfo _executeMethod
            = typeof(InMemoryAsyncQueryProvider).GetTypeInfo().DeclaredMethods
                                                .Single(m => m.IsGenericMethodDefinition && m.Name == nameof(InMemoryAsyncQueryProvider.Execute));

        private readonly IQueryProvider _provider;
        private readonly Action<string, IEnumerable> _include;

        public InMemoryAsyncQueryProvider(IQueryProvider provider, Action<string, IEnumerable> include = null)
        {
            _provider = provider;
            _include = include;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return (IQueryable)_createQueryMethod
                .MakeGenericMethod(TryGetElementType(expression.Type))
                .Invoke(this, new object[] { expression });
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new InMemoryAsyncQueryable<TElement>(_provider.CreateQuery<TElement>(expression), _include);
        public object Execute(Expression expression) => _executeMethod.MakeGenericMethod(expression.Type).Invoke(this, new object[] { expression });
        public TResult Execute<TResult>(Expression expression) => _provider.Execute<TResult>(expression);
        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken) => Task.FromResult(Execute(expression));
        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Task.FromResult(Execute<TResult>(expression));
        private static Type TryGetElementType(Type type)
        {
            if (!type.GetTypeInfo().IsGenericTypeDefinition)
            {
                var interfaceImpl = type.GetInterfaces().Union(new[] { type })
                    .FirstOrDefault(t => t.GetType().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                if (interfaceImpl != null)
                    return interfaceImpl.GetGenericArguments().Single();
            }
            return type;
        }
    }
}
