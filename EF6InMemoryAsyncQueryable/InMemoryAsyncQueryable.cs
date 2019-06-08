// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity.TestDoubles
{
    public class InMemoryAsyncQueryable<T> : IOrderedQueryable<T>, IDbAsyncEnumerable<T>
    {
        private readonly IQueryable<T> _queryable;
        private readonly Action<string, IEnumerable> _include;

        public InMemoryAsyncQueryable(IEnumerable<T> enumerable, Action<string, IEnumerable> include = null)
            : this(enumerable.AsQueryable(), include) { }


        public InMemoryAsyncQueryable(IQueryable<T> queryable, Action<string, IEnumerable> include = null)
        {
            _queryable = queryable;
            _include = include;
        }

        public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Expression Expression => _queryable.Expression;

        public Type ElementType => _queryable.ElementType;

        public IQueryProvider Provider => new InMemoryAsyncQueryProvider(_queryable.Provider, _include);

        public IQueryable<T> Include(string path)
        {
            _include?.Invoke(path, _queryable);
            return this;
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator() => new InMemoryDbAsyncEnumerator<T>(GetEnumerator());
        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => GetAsyncEnumerator();
    }
}
