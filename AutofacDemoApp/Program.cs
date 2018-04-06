using System;
using Autofac;
using Autofac.Core;

namespace AutofacDemoApp
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            // 每个依赖一个实例(Instance Per Dependency)
            // 等价于 builder.RegisterType<ConsoleOutput>().As<IOutput>().InstancePerDependency();
            // 当你解析一个每个依赖一个实例的组件时, 你每次获得一个新的实例.
            builder.RegisterType<ConsoleOutput>().As<IOutput>().As<IStartable>();
           
            //
            // 如果注册两个类型一样的接口，解析时使用的时后注册的对象（后入先出）
            builder.RegisterType<TomorrowWriter>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            // 通过lambda 表达式注册
            // 单一实例 在根容器和所有嵌套作用域内所有的请求都将会返回同一个实例.
            builder.Register(c => new ConfigReader("sectionName")).As<IConfigReader>().SingleInstance();
            //// 使用named 参数 注册
            //builder.RegisterType<ConfigReader>()
            //    .As<IConfigReader>()
            //    .WithParameter("configSectionName", "sectionName");

            //// 使用参数类型注册
            //builder.RegisterType<ConfigReader>()
            //    .As<IConfigReader>()
            //    .WithParameter(new TypedParameter(typeof(string),"sectionName"));

            //// Using a Resolved parameter
            //builder.RegisterType<ConfigReader>()
            //    .As<IConfigReader>()
            //    .WithParameter(
            //        new ResolvedParameter(
            //            (pi,ctx) => pi.ParameterType == typeof(string) && pi.Name == "configSectionName",
            //            (pi,ctx) => "sectionName"
            //            ));


            Container = builder.Build();

            WriteDate();
            Console.WriteLine("Hello World!");   
            Console.ReadKey();
        }

        public static void WriteDate() {
            using (var scope = Container.BeginLifetimeScope(
                    builder => {
                        builder.RegisterType<TomorrowWriter>().As<IDateWriter>();
                    }
                )) {

                Console.WriteLine("using Container Resolve");
                TomorrowWriter write = Container.Resolve<TomorrowWriter>();
                write.WriteDate();

                IDateWriter dateWriter = scope.Resolve<IDateWriter>();
                dateWriter.WriteDate();

                IConfigReader configReader = scope.Resolve<IConfigReader>(new NamedParameter("configSectionName", "sectionName"));

                IConfigReader c2 = Container.Resolve<IConfigReader>(new NamedParameter("configSectionName", "sectionName"));
                IConfigReader c3 = Container.Resolve<IConfigReader>(new NamedParameter("configSectionName", "sectionName"));

                Console.WriteLine(configReader.Equals(c2));
                Console.WriteLine(configReader.GetHashCode() + "####" + c2.GetHashCode());

            }
        }
    }
}
