using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hx.Sdk.Swagger.Builders
{
    ///// <summary>
    ///// 规范化文档构建器
    ///// </summary>
    //internal class SwaggerDocumentBuilder2
    //{
    //    /// <summary>
    //    /// 规范化文档配置
    //    /// </summary>
    //    private static readonly SwaggerSettingsOptions _swaggerSettingsOptions;

    //    /// <summary>
    //    /// 分组信息
    //    /// </summary>
    //    private static readonly IEnumerable<GroupExtraInfo> _groupExtraInfos;

    //    /// <summary>
    //    /// 文档分组列表
    //    /// </summary>
    //    private static readonly IEnumerable<string> _groups;

    //    /// <summary>
    //    /// 带排序的分组名
    //    /// </summary>
    //    private static readonly Regex _groupOrderRegex;

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    static SwaggerDocumentBuilder()
    //    {
    //        //// 载入配置
    //        _swaggerSettingsOptions = Penetrates.GetSwaggerSettings();
    //        // 初始化常量
    //        _groupOrderRegex = new Regex(@"@(?<order>[0-9]+$)");
    //        GetActionGroupsCached = new ConcurrentDictionary<MethodInfo, IEnumerable<GroupExtraInfo>>();
    //        GetControllerGroupsCached = new ConcurrentDictionary<Type, IEnumerable<GroupExtraInfo>>();
    //        GetGroupOpenApiInfoCached = new ConcurrentDictionary<string, SwaggerOpenApiInfo>();
    //        GetControllerTagCached = new ConcurrentDictionary<ControllerActionDescriptor, string>();
    //        GetActionTagCached = new ConcurrentDictionary<ApiDescription, string>();

    //        // 默认分组，支持多个逗号分割
    //        _groupExtraInfos = new List<GroupExtraInfo> { ResolveGroupExtraInfo(_swaggerSettingsOptions.DefaultGroupName) };

    //        // 加载所有分组
    //        _groups = ReadGroups();
    //    }

    //    /// <summary>
    //    /// 构建Swagger全局配置
    //    /// </summary>
    //    /// <param name="swaggerOptions">Swagger 全局配置</param>
    //    /// <param name="configure"></param>
    //    internal static void Build(SwaggerOptions swaggerOptions, Action<SwaggerOptions> configure = null)
    //    {
    //        // 生成V2版本
    //        swaggerOptions.SerializeAsV2 = _swaggerSettingsOptions.FormatAsV2 == true;

    //        // 判断是否启用 Server
    //        if (_swaggerSettingsOptions.HideServers != true)
    //        {
    //            // 启动服务器 Servers
    //            swaggerOptions.PreSerializeFilters.Add((swagger, request) =>
    //            {
    //                // 默认 Server
    //                var servers = new List<OpenApiServer> {
    //                    new OpenApiServer { Url = $"{request.Scheme}://{request.Host.Value}{_swaggerSettingsOptions.VirtualPath}",Description="Default" }
    //                };
    //                servers.AddRange(_swaggerSettingsOptions.Servers);

    //                swagger.Servers = servers;
    //            });
    //        }

    //        // 配置路由模板
    //        swaggerOptions.RouteTemplate = _swaggerSettingsOptions.RouteTemplate;

    //        // 自定义配置
    //        configure?.Invoke(swaggerOptions);
    //    }

    //    /// <summary>
    //    /// Swagger 生成器构建
    //    /// </summary>
    //    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    //    /// <param name="configure">自定义配置</param>
    //    internal static void BuildSwaggerGen(SwaggerGenOptions swaggerGenOptions, Action<SwaggerGenOptions> configure = null)
    //    {
    //        //// 创建分组文档
    //        CreateSwaggerDocs(swaggerGenOptions);

    //        //// 加载分组控制器和动作方法列表
    //        LoadGroupControllerWithActions(swaggerGenOptions);

    //        // 配置 Swagger SchemaId
    //        ConfigureSchemaId(swaggerGenOptions);

    //        // 配置标签
    //        ConfigureTagsAction(swaggerGenOptions);

    //        // 配置 Action 排序
    //        ConfigureActionSequence(swaggerGenOptions);

    //        // 加载注释描述文件
    //        LoadXmlComments(swaggerGenOptions);

    //        // 配置授权
    //        ConfigureSecurities(swaggerGenOptions);

    //        //使得Swagger能够正确地显示Enum的对应关系
    //        swaggerGenOptions.SchemaFilter<EnumSchemaFilter>();

    //        //// 支持控制器排序操作
    //        swaggerGenOptions.DocumentFilter<TagsOrderDocumentFilter>();

    //        // 自定义配置
    //        configure?.Invoke(swaggerGenOptions);
    //    }

    //    /// <summary>
    //    /// Swagger UI 构建
    //    /// </summary>
    //    /// <param name="swaggerUIOptions"></param>
    //    /// <param name="routePrefix"></param>
    //    /// <param name="configure"></param>
    //    internal static void BuildUI(SwaggerUIOptions swaggerUIOptions, string routePrefix = default, Action<SwaggerUIOptions> configure = null)
    //    {
    //        // 配置分组终点路由
    //        CreateGroupEndpoint(swaggerUIOptions);

    //        // 配置文档标题
    //        swaggerUIOptions.DocumentTitle = _swaggerSettingsOptions.DocumentTitle;

    //        //// 配置UI地址
    //        swaggerUIOptions.RoutePrefix = _swaggerSettingsOptions.RoutePrefix ?? routePrefix ?? "swagger";

    //        // 文档展开设置
    //        swaggerUIOptions.DocExpansion(_swaggerSettingsOptions.DocExpansionState.Value);

    //        // 注入 MiniProfiler 组件
    //        InjectMiniProfilerPlugin(swaggerUIOptions);

    //        // 自定义配置
    //        configure?.Invoke(swaggerUIOptions);
    //    }

    //    /// <summary>
    //    /// 创建分组文档
    //    /// </summary>
    //    /// <param name="swaggerGenOptions">Swagger生成器对象</param>
    //    private static void CreateSwaggerDocs(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        foreach (var group in _groups)
    //        {
    //            var groupOpenApiInfo = GetGroupOpenApiInfo(group) as OpenApiInfo;
    //            swaggerGenOptions.SwaggerDoc(group, groupOpenApiInfo);
    //        }
    //    }

    //    /// <summary>
    //    /// 加载分组控制器和动作方法列表
    //    /// </summary>
    //    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    //    private static void LoadGroupControllerWithActions(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        swaggerGenOptions.DocInclusionPredicate((currentGroup, apiDescription) =>
    //        {
    //            if (!apiDescription.TryGetMethodInfo(out var method) || typeof(Controller).IsAssignableFrom(method.ReflectedType)) return false;

    //            return GetActionGroups(method).Any(u => u.Group == currentGroup);
    //        });
    //    }

    //    /// <summary>
    //    ///  配置标签
    //    /// </summary>
    //    /// <param name="swaggerGenOptions"></param>
    //    private static void ConfigureTagsAction(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        swaggerGenOptions.TagActionsBy(apiDescription =>
    //        {
    //            return new[] { GetActionTag(apiDescription) };
    //        });
    //    }

    //    /// <summary>
    //    ///  配置 Action 排序
    //    /// </summary>
    //    /// <param name="swaggerGenOptions"></param>
    //    private static void ConfigureActionSequence(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        swaggerGenOptions.OrderActionsBy(apiDesc =>
    //        {
    //            var apiDescriptionSettings = apiDesc.CustomAttributes()
    //                                   .FirstOrDefault(u => u.GetType() == typeof(ApiDescriptionSettingsAttribute))
    //                                   as ApiDescriptionSettingsAttribute ?? new ApiDescriptionSettingsAttribute();

    //            return (int.MaxValue - apiDescriptionSettings.Order).ToString()
    //                            .PadLeft(int.MaxValue.ToString().Length, '0');
    //        });
    //    }

    //    /// <summary>
    //    /// 配置 Swagger SchemaId
    //    /// </summary>
    //    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    //    private static void ConfigureSchemaId(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        // 本地函数
    //        static string DefaultSchemaIdSelector(Type modelType)
    //        {
    //            if (!modelType.IsConstructedGenericType) return modelType.FullName;

    //            var prefix = modelType.GetGenericArguments()
    //                .Select(genericArg => DefaultSchemaIdSelector(genericArg))
    //                .Aggregate((previous, current) => previous + current);

    //            // 通过 Of 拼接多个泛型
    //            return modelType.FullName.Split('`').First() + "Of" + prefix;
    //        }

    //        // 调用本地函数
    //        swaggerGenOptions.CustomSchemaIds(modelType => DefaultSchemaIdSelector(modelType));
    //    }

    //    /// <summary>
    //    /// 加载注释描述文件
    //    /// </summary>
    //    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    //    private static void LoadXmlComments(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        var xmlComments = _swaggerSettingsOptions.XmlComments;
    //        foreach (var xmlComment in xmlComments)
    //        {
    //            var assemblyXmlName = xmlComment.EndsWith(".xml") ? xmlComment : $"{xmlComment}.xml";
    //            var assemblyXmlPath = Path.Combine(AppContext.BaseDirectory, assemblyXmlName);
    //            if (File.Exists(assemblyXmlPath))
    //            {
    //                swaggerGenOptions.IncludeXmlComments(assemblyXmlPath, true);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 配置授权
    //    /// </summary>
    //    /// <param name="swaggerGenOptions">Swagger 生成器配置</param>
    //    private static void ConfigureSecurities(SwaggerGenOptions swaggerGenOptions)
    //    {
    //        // 判断是否启用了授权
    //        if (_swaggerSettingsOptions.EnableAuthorized != true || _swaggerSettingsOptions.SecurityDefinitions.Length == 0) return;

    //        //开启加权小锁
    //        swaggerGenOptions.OperationFilter<AddResponseHeadersFilter>();
    //        swaggerGenOptions.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

    //        // 在header中添加token，传递到后台
    //        swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
    //        ////生成安全定义
    //        foreach (var securityDefinition in _swaggerSettingsOptions.SecurityDefinitions)
    //        {
    //            // Id 必须定义
    //            if (string.IsNullOrWhiteSpace(securityDefinition.Id)) continue;
    //            swaggerGenOptions.AddSecurityDefinition(securityDefinition.Id, securityDefinition);
    //        }
    //    }

    //    /// <summary>
    //    /// 配置分组终点路由
    //    /// </summary>
    //    /// <param name="swaggerUIOptions"></param>
    //    private static void CreateGroupEndpoint(SwaggerUIOptions swaggerUIOptions)
    //    {
    //        foreach (var group in _groups)
    //        {
    //            var groupOpenApiInfo = GetGroupOpenApiInfo(group);

    //            // 替换路由模板
    //            var routeTemplate = _swaggerSettingsOptions.RouteTemplate.Replace("{documentName}", Uri.EscapeDataString(group));
    //            swaggerUIOptions.SwaggerEndpoint($"{_swaggerSettingsOptions.VirtualPath}/{routeTemplate}", groupOpenApiInfo?.Title ?? group);
    //        }
    //    }

    //    /// <summary>
    //    /// 注入 MiniProfiler 插件
    //    /// </summary>
    //    /// <param name="swaggerUIOptions"></param>
    //    private static void InjectMiniProfilerPlugin(SwaggerUIOptions swaggerUIOptions)
    //    {
    //        // 启用 MiniProfiler 组件
    //        var thisType = typeof(SwaggerDocumentBuilder);
    //        var thisAssembly = thisType.Assembly;

    //        // 自定义 Swagger 首页
    //        swaggerUIOptions.IndexStream = () =>
    //        {
    //            var stream = thisAssembly.GetManifestResourceStream($"{thisType.Namespace}.Assets.{(_swaggerSettingsOptions.EnabledMiniProfiler != true ? "index" : "index-mini-profiler")}.html");
    //            return stream;
    //        };
    //    }

    //    /// <summary>
    //    /// 获取分组信息缓存集合
    //    /// </summary>
    //    private static readonly ConcurrentDictionary<string, SwaggerOpenApiInfo> GetGroupOpenApiInfoCached;

    //    /// <summary>
    //    /// 获取分组配置信息
    //    /// </summary>
    //    /// <param name="group"></param>
    //    /// <returns></returns>
    //    private static SwaggerOpenApiInfo GetGroupOpenApiInfo(string group)
    //    {
    //        return GetGroupOpenApiInfoCached.GetOrAdd(group, Function);

    //        // 本地函数
    //        static SwaggerOpenApiInfo Function(string group)
    //        {
    //            return _swaggerSettingsOptions.GroupOpenApiInfos.FirstOrDefault(u => u.Group == group) ?? new SwaggerOpenApiInfo { Group = group };
    //        }
    //    }

    //    /// <summary>
    //    /// 读取所有分组信息
    //    /// </summary>
    //    /// <returns></returns>
    //    private static IEnumerable<string> ReadGroups()
    //    {
    //        // 获取所有的控制器和动作方法
    //        var controllers = Penetrates.EffectiveTypes.Where(u => Penetrates.IsApiController(u));
    //        if (!controllers.Any()) return new[] { _swaggerSettingsOptions.DefaultGroupName };

    //        var actions = controllers.SelectMany(c => c.GetMethods().Where(u => IsApiAction(u, c)));

    //        // 合并所有分组
    //        var groupOrders = controllers.SelectMany(u => GetControllerGroups(u))
    //            .Union(
    //                actions.SelectMany(u => GetActionGroups(u))
    //            )
    //            .Where(u => u != null && u.Visible)
    //            // 分组后取最大排序
    //            .GroupBy(u => u.Group)
    //            .Select(u => new GroupExtraInfo
    //            {
    //                Group = u.Key,
    //                Order = u.Max(x => x.Order),
    //                Visible = true
    //            });

    //        // 分组排序
    //        return groupOrders
    //            .OrderByDescending(u => u.Order)
    //            .ThenBy(u => u.Group)
    //            .Select(u => u.Group);
    //    }

    //    /// <summary>
    //    /// 获取控制器组缓存集合
    //    /// </summary>
    //    private static readonly ConcurrentDictionary<Type, IEnumerable<GroupExtraInfo>> GetControllerGroupsCached;

    //    /// <summary>
    //    /// 获取控制器分组列表
    //    /// </summary>
    //    /// <param name="type"></param>
    //    /// <returns></returns>
    //    private static IEnumerable<GroupExtraInfo> GetControllerGroups(Type type)
    //    {
    //        return GetControllerGroupsCached.GetOrAdd(type, Function);

    //        // 本地函数
    //        static IEnumerable<GroupExtraInfo> Function(Type type)
    //        {
    //            // 如果控制器没有定义 [ApiDescriptionSettings] 特性，则返回默认分组
    //            if (!type.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return _groupExtraInfos;

    //            // 读取分组
    //            var apiDescriptionSettings = type.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true);
    //            if (apiDescriptionSettings.Groups == null || apiDescriptionSettings.Groups.Length == 0) return _groupExtraInfos;

    //            // 处理分组额外信息
    //            var groupExtras = new List<GroupExtraInfo>();
    //            foreach (var group in apiDescriptionSettings.Groups)
    //            {
    //                groupExtras.Add(ResolveGroupExtraInfo(group));
    //            }

    //            return groupExtras;
    //        }
    //    }

    //    /// <summary>
    //    /// <see cref="GetActionGroups(MethodInfo)"/> 缓存集合
    //    /// </summary>
    //    private static readonly ConcurrentDictionary<MethodInfo, IEnumerable<GroupExtraInfo>> GetActionGroupsCached;

    //    /// <summary>
    //    /// 获取动作方法分组列表
    //    /// </summary>
    //    /// <param name="method">方法</param>
    //    /// <returns></returns>
    //    private static IEnumerable<GroupExtraInfo> GetActionGroups(MethodInfo method)
    //    {
    //        return GetActionGroupsCached.GetOrAdd(method, Function);

    //        // 本地函数
    //        static IEnumerable<GroupExtraInfo> Function(MethodInfo method)
    //        {
    //            // 如果动作方法没有定义 [ApiDescriptionSettings] 特性，则返回所在控制器分组
    //            if (!method.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return GetControllerGroups(method.ReflectedType);

    //            // 读取分组
    //            var apiDescriptionSettings = method.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true);
    //            if (apiDescriptionSettings.Groups == null || apiDescriptionSettings.Groups.Length == 0) return GetControllerGroups(method.ReflectedType);

    //            // 处理排序
    //            var groupExtras = new List<GroupExtraInfo>();
    //            foreach (var group in apiDescriptionSettings.Groups)
    //            {
    //                groupExtras.Add(ResolveGroupExtraInfo(group));
    //            }

    //            return groupExtras;
    //        }
    //    }

    //    /// <summary>
    //    /// <see cref="GetActionTag(ApiDescription)"/> 缓存集合
    //    /// </summary>
    //    private static readonly ConcurrentDictionary<ControllerActionDescriptor, string> GetControllerTagCached;

    //    /// <summary>
    //    /// 获取控制器标签
    //    /// </summary>
    //    /// <param name="controllerActionDescriptor">控制器接口描述器</param>
    //    /// <returns></returns>
    //    private static string GetControllerTag(ControllerActionDescriptor controllerActionDescriptor)
    //    {
    //        return GetControllerTagCached.GetOrAdd(controllerActionDescriptor, Function);

    //        // 本地函数
    //        static string Function(ControllerActionDescriptor controllerActionDescriptor)
    //        {
    //            var type = controllerActionDescriptor.ControllerTypeInfo;
    //            // 如果动作方法没有定义 [ApiDescriptionSettings] 特性，则返回所在控制器名
    //            if (!type.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return controllerActionDescriptor.ControllerName;

    //            // 读取标签
    //            var apiDescriptionSettings = type.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true);
    //            return string.IsNullOrWhiteSpace(apiDescriptionSettings.Tag) ? controllerActionDescriptor.ControllerName : apiDescriptionSettings.Tag;
    //        }
    //    }

    //    /// <summary>
    //    /// <see cref="GetActionTag(ApiDescription)"/> 缓存集合
    //    /// </summary>
    //    private static readonly ConcurrentDictionary<ApiDescription, string> GetActionTagCached;

    //    /// <summary>
    //    /// 获取动作方法标签
    //    /// </summary>
    //    /// <param name="apiDescription">接口描述器</param>
    //    /// <returns></returns>
    //    private static string GetActionTag(ApiDescription apiDescription)
    //    {
    //        return GetActionTagCached.GetOrAdd(apiDescription, Function);

    //        // 本地函数
    //        static string Function(ApiDescription apiDescription)
    //        {
    //            if (!apiDescription.TryGetMethodInfo(out var method)) return "unknown";

    //            // 获取控制器描述器
    //            var controllerActionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;

    //            // 如果动作方法没有定义 [ApiDescriptionSettings] 特性，则返回所在控制器名
    //            if (!method.IsDefined(typeof(ApiDescriptionSettingsAttribute), true)) return GetControllerTag(controllerActionDescriptor);

    //            // 读取标签
    //            var apiDescriptionSettings = method.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true);
    //            return string.IsNullOrWhiteSpace(apiDescriptionSettings.Tag) ? GetControllerTag(controllerActionDescriptor) : apiDescriptionSettings.Tag;
    //        }
    //    }

    //    /// <summary>
    //    /// 是否是动作方法
    //    /// </summary>
    //    /// <param name="method">方法</param>
    //    /// <param name="ReflectedType">声明类型</param>
    //    /// <returns></returns>
    //    private static bool IsApiAction(MethodInfo method, Type ReflectedType)
    //    {
    //        // 不是非公开、抽象、静态、泛型方法
    //        if (!method.IsPublic || method.IsAbstract || method.IsStatic || method.IsGenericMethod) return false;

    //        // 如果所在类型不是控制器，则该行为也被忽略
    //        if (method.ReflectedType != ReflectedType || method.DeclaringType == typeof(object)) return false;

    //        // 不是能被导出忽略的接方法
    //        if (method.IsDefined(typeof(ApiExplorerSettingsAttribute), true) && method.GetCustomAttribute<ApiExplorerSettingsAttribute>(true).IgnoreApi) return false;

    //        return true;
    //    }

    //    /// <summary>
    //    /// 解析分组附加信息
    //    /// </summary>
    //    /// <param name="group">分组名</param>
    //    /// <returns></returns>
    //    private static GroupExtraInfo ResolveGroupExtraInfo(string group)
    //    {
    //        string realGroup;
    //        var order = 0;

    //        if (!_groupOrderRegex.IsMatch(group)) realGroup = group;
    //        else
    //        {
    //            realGroup = _groupOrderRegex.Replace(group, "");
    //            order = int.Parse(_groupOrderRegex.Match(group).Groups["order"].Value);
    //        }

    //        var groupOpenApiInfo = GetGroupOpenApiInfo(realGroup);
    //        return new GroupExtraInfo
    //        {
    //            Group = realGroup,
    //            Order = groupOpenApiInfo.Order ?? order,
    //            Visible = groupOpenApiInfo.Visible ?? true
    //        };
    //    }
    //}
}
