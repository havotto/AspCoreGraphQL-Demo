using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCoreGraphQL.Entities;
using AspCoreGraphQL.Entities.Context;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreGraphQL.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly DataContext db;
        public PostController(DataContext db)
        {
            this.db = db;
        }

        public IEnumerable<Post> Get() => db.Posts;
    }
}