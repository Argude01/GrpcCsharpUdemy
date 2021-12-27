using Blog;
using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Blog.BlogService;

namespace server
{
    internal class BlogServiceImpl : BlogServiceBase
    {
        // Creamos el cliente
        private static MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
        // Creamos la base de datos
        private static IMongoDatabase mongoDatabase = mongoClient.GetDatabase("mybd");
        // Creamos la tabla
        private static IMongoCollection<BsonDocument> mongoCollection = mongoDatabase.GetCollection<BsonDocument>("blog");

        public override Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
        {
            var blog = request.Blog;
            BsonDocument doc = new BsonDocument("author_id", blog.AuthorId)
                                                .Add("title", blog.Title)
                                                .Add("content", blog.Content);
            mongoCollection.InsertOne(doc); // crea el id para el documento (registro)

            String id = doc.GetValue("_id").ToString();

            blog.Id = id;

            return Task.FromResult(new CreateBlogResponse()
            {
                Blog = blog,
            });
        }

    }
}
