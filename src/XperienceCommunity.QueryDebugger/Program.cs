using CMS.ContentEngine;
using CMS.Core;
using CMS.DataEngine;
using CMS.Membership;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Profiling;
using XperienceCommunity.QueryDebugger;

var builder = Host.CreateApplicationBuilder(args);
CMSApplication.PreInit(false);
Service.MergeDescriptors(builder.Services);

builder.Services.AddKenticoMiniProfiler();

var app = builder.Build();

Service.SetProvider(app.Services);
CMSApplication.Init();

var profiler = MiniProfiler.StartNew();

var exectutor = app.Services.GetRequiredService<IContentQueryExecutor>();
var b = new ContentItemQueryBuilder()
    .ForContentTypes(q =>
    {
        q.OfContentType("DancingGoat.Coffee");
    });
await profiler.PrintQuery("Coffee", async () =>
{
    var items = await exectutor.GetResult(b, i => i.ContentItemID);
});

var userProvider = Service.Resolve<IInfoProvider<UserInfo>>();
await profiler.PrintQuery("Users", async () =>
{
    var users = await userProvider.Get().GetEnumerableTypedResultAsync();
});