namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 主键接口
    /// </summary>
    public interface IEntity: IPrivateEntity
    {
    }

    /// <summary>
    /// 数据库实体依赖接口（禁止外部继承）
    /// </summary>
    public interface IPrivateEntity
    {
    }
}
