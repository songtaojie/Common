using Hx.Sdk.DependencyInjection;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库无键实体依赖基接口
    /// </summary>
    [SkipScan]
    public abstract class EntityNotKey : EntityNotKey<MasterDbContextLocator>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">数据库中定义名</param>
        public EntityNotKey(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// 数据库无键实体依赖基接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    [SkipScan]
    public abstract class EntityNotKey<TDbContextLocator1> : PrivateEntityNotKey
        where TDbContextLocator1 : class, IDbContextLocator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">数据库中定义名</param>
        public EntityNotKey(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// 数据库无键实体依赖基接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    [SkipScan]
    public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2> : PrivateEntityNotKey
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">数据库中定义名</param>
        public EntityNotKey(string name) : base(name)
        {
        }
    }
    

    /// <summary>
    /// 数据库无键实体基类（禁止外部继承）
    /// </summary>
    [SkipScan]
    public abstract class PrivateEntityNotKey : IPrivateEntityNotKey
    {
        /// <summary>
        /// 无键实体名
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">数据库中定义名</param>
        public PrivateEntityNotKey(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 获取视图名称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _name;
        }
    }
}