using Blog;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Channel channel = new Channel("localhost", 50052, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            var client = new BlogService.BlogServiceClient(channel);

            //CreateBlog(client);
            //var newBlog = CreateBlog(client);

            //ReadBlog(client);

            //UpdateBlog(client, newBlog);

            //DeleteBlog(client, newBlog);

            await ListBlog(client);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        private static Blog.Blog CreateBlog(BlogService.BlogServiceClient client)
        {
            var response = client.CreateBlog(new CreateBlogRequest()
            {
                Blog = new Blog.Blog()
                {
                    AuthorId = "Deyanira",
                    Title = "New blog!",
                    Content = "Hello world, this is a new blog"
                }
            });

            Console.WriteLine("The blog " + response.Blog.Id + "was created !");

            return response.Blog;
        }

        private static void ReadBlog(BlogService.BlogServiceClient client)
        {
            try 
            {
                var response = client.ReadBlog(new ReadBlogRequest()
                {
                    BlogId = "61cb215c94427875cf2e6bba"
                });
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

        private static void UpdateBlog(BlogService.BlogServiceClient client, Blog.Blog blog)
        {
            try
            {
                blog.AuthorId = "Updated author";
                blog.Title = "Updated title";
                blog.Content = "Updated content";

                var response = client.UpdateBlog(new UpdateBlogRequest()
                {
                    Blog = blog
                });

                Console.WriteLine(response.Blog.ToString());
            }
            catch(RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

        private static void DeleteBlog(BlogService.BlogServiceClient client, Blog.Blog blog)
        {
            try
            {
                var response = client.DeleteBlog(new DeleteBlogRequest() { BlogId = blog.Id });

                Console.WriteLine("The blog with id " + response.BlogId + " was deleted");
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Status.Detail);
            }
        }

        private static async Task ListBlog(BlogService.BlogServiceClient client)
        {
            var response = client.ListBlog(new ListBlogRequest() { });

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Blog.ToString());
            }
        }
    }
}
