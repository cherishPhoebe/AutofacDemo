using System;
using Autofac;

namespace AutofacDemoApp
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleOutput>().As<IOutput>();
            // 如果注册两个类型一样的接口，解析时使用的时后注册的对象（后入先出）
            builder.RegisterType<TomorrowWriter>().As<IDateWriter>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            Container = builder.Build();

            WriteDate();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }

        public static void WriteDate() {
            using (var scope = Container.BeginLifetimeScope()) {
                IDateWriter write = scope.Resolve<IDateWriter>();
                write.WriteDate();
            }
        }
    }
}
