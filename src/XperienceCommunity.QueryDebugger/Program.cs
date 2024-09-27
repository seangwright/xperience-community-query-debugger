using CMS.ContentEngine;
using CMS.Core;
using CMS.DataEngine;
using CMS.Membership;

using DancingGoat.Models;

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
    .ForContentTypes(q => q.OfContentType(Coffee.CONTENT_TYPE_NAME));
await profiler.PrintQuery(Coffee.CONTENT_TYPE_NAME, () => exectutor.GetResult(b, i => i.ContentItemID));

var userProvider = Service.Resolve<IInfoProvider<UserInfo>>();
await profiler.PrintQuery(UserInfo.OBJECT_TYPE, () => userProvider.Get().GetEnumerableTypedResultAsync(), new() { Verbose = true });
