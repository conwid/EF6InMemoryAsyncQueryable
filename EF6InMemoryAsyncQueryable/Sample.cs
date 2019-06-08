using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.TestDoubles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6InMemoryAsyncQueryable
{
    public class Sample
    {
        public async Task RunSample()
        {
            List<string> myList = new List<string> { "Akos1", "Bkos2", "Akos3" };
            var inMemoryQueryable = new InMemoryAsyncQueryable<string>(myList);
            var result = await inMemoryQueryable.Where(m => m.StartsWith("A")).ToListAsync();
        }
    }
}
