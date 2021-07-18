namespace Platformer.Core
{
    public static partial class Simulation
    {
        /// <summary>
        /// 此类提供了一个容器，用于在模拟范围内为任何其他类创建单例。它通常用于保存仿真模型和配置类。这个类来自平台游戏Microgame模板。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static class InstanceRegister<T> where T : class, new()
        {
            public static T instance = new T();
        }
    }
}