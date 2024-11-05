
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

await profiler.PrintQuery(
    $"{Coffee.CONTENT_TYPE_NAME} default query",
    () =>
    {
        var b = new ContentItemQueryBuilder()
            .ForContentTypes(q => q.OfContentType(Coffee.CONTENT_TYPE_NAME));
        return exectutor.GetResult(b, i => i.ContentItemID);
    });

await profiler.PrintQuery(
    $"{nameof(ISEOFields)} with no cache",
    () =>
    {
        var b = new ContentItemQueryBuilder()
            .ForContentTypes(q => q.OfReusableSchema(ISEOFields.REUSABLE_FIELD_SCHEMA_NAME));
        return exectutor.GetResult(b, i => i.ContentItemID);
    },
    new() { PopulateCache = false });

await profiler.PrintQuery(
    "Coffee by taxonomy with image",
    () =>
    {
        var b = new ContentItemQueryBuilder()
            .ForContentTypes(q => q
                .OfContentType(Coffee.CONTENT_TYPE_NAME)
                .WithContentTypeFields()
                .WithLinkedItems(1))
            .Parameters(q => q
                .Where(w => w.WhereContainsTags(nameof(Coffee.CoffeeProcessing), [new Guid("9891389f-e404-476a-8eb8-55adea059d51")]))
                .Columns(nameof(Coffee.SystemFields.ContentItemID), nameof(Coffee.CoffeeProcessing), nameof(Coffee.ProductFieldsName), nameof(Coffee.ProductFieldsImage))
                .OrderBy(nameof(Coffee.ProductFieldsName))
                .TopN(5)
                .IncludeTotalCount());
        return exectutor.GetResult(b, i => i.ContentItemID);
    });

var userProvider = Service.Resolve<IInfoProvider<UserInfo>>();
await profiler.PrintQuery(
    $"{UserInfo.OBJECT_TYPE} verbose",
    () => userProvider.Get().GetEnumerableTypedResultAsync(),
    new() { Verbose = true });
