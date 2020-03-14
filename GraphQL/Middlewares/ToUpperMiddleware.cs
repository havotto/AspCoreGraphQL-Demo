using System.Threading.Tasks;
using HotChocolate.Resolvers;

namespace AspCoreGraphQL.GraphQL.Middlewares
{
    /// <summary>
    /// Field middleware that Converts string fields to uppercase
    /// </summary>
    public class ToUpperMiddleware
    {
        private readonly FieldDelegate next;

        public ToUpperMiddleware(FieldDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            await next(context);
            if (context.Result is string s)
            {
                context.Result = s.ToUpper();
            }
        }
    }
}