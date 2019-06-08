// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root of EF6 for license information.

using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.TestDoubles
{

    internal class InMemoryDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public InMemoryDbAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public void Dispose()
        {
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(_enumerator.MoveNext());
        public T Current => _enumerator.Current;
        object IDbAsyncEnumerator.Current => Current;
    }
}
