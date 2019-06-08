# Supporting async LInQ evaluation with in-memory mocks

Have you ever tried to mock your Entity Framework based code with in-memory data structures? If you have, you have also probably come across this nasty little exception:

__System.InvalidOperationException: 'The source IQueryable doesn't implement IDbAsyncEnumerable<System.String>. Only sources that implement IDbAsyncEnumerable can be used for Entity Framework asynchronous operations.__

The content in this repository does just what the exception message suggests and gives you an in-memory implementation of that magic interface. You can check out the Sample.cs class on how to use it, or go to my blog and read dotnetfalcon.com/supporting-async-linq-evaluation-on-iqueryable-mocks/ for more infor on how this works.

Note: all credit goes to Microsoft, because this is acutally their code to test EF 6. Check out https://github.com/aspnet/EntityFramework6 for more information about licensing and whatnot.

