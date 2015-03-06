using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            #region
            RMB r1, r2, r3, r4;

            // 记得小学时的某次捐款，我把口袋里藏好的一块钱加6张一毛钱以及13个一分钱的硬币都贡献出去了：（
            r1 = new RMB(1, 6, 13);
            // 其实当时其他人都已经交过了，他们总共交了：
            r2 = new RMB(46, 9, 3);
            // 那么加上我的就是：
            r3 = r1 + r2;
            Console.WriteLine("r3 = {0}", r3.ToString());

            // 隐式转换
            float f = r3;
            Console.WriteLine("float f= {0}", f);

            // 显式转换
            r4 = (RMB)f;
            Console.WriteLine("r4 = {0}", r4.ToString());
            //如果不进行显示转换,将出现错误 CS0266: 无法将类型“float”隐式转换为“Hunts.Keywords.RMB”。存在一个显式转换(是否缺少强制转换?)

            #endregion


            // 数据模型改变，删除数据库重新创建。
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BlogContext>());

            using (var db = new BlogContext())
            {
                db.Blogs.Add(new Blog { Name = "Another Blog " }); //调用这一步的时候才开始创建数据库和表 并非程序启动就创建数据库和表
                db.SaveChanges();

                foreach (var blog in db.Blogs)
                {
                    Console.WriteLine(blog.Name);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
    }


    #region 操作符重载 强制类型转换 与 隐式转换
    public struct RMB
    {
        // 注意:这些数的范围可能不能满足实际中的使用
        public uint Yuan;
        public uint Jiao;
        public uint Fen;

        public RMB(uint yuan, uint jiao, uint fen)
        {
            if (fen > 9)
            {
                jiao += fen / 10;
                fen = fen % 10;
            }
            if (jiao > 9)
            {
                yuan += jiao / 10;
                jiao = jiao % 10;
            }
            this.Yuan = yuan;
            this.Jiao = jiao;
            this.Fen = fen;
        }

        public override string ToString()
        {
            return string.Format("￥{0}元{1}角{2}分", Yuan, Jiao, Fen);
        }

        // 重载+操作符
        public static RMB operator +(RMB rmb1, RMB rmb2)
        {
            return new RMB(rmb1.Yuan + rmb2.Yuan, rmb1.Jiao + rmb2.Jiao, rmb1.Fen + rmb2.Fen);
        }

        // 隐式转换
        public static implicit operator float(RMB rmb)
        {
            return rmb.Yuan + (rmb.Jiao / 10.0f) + (rmb.Fen / 100.00f);
        }

        //显式转换
        public static explicit operator RMB(float f)
        {
            uint yuan = (uint)f;
            uint jiao = (uint)((f - yuan) * 10);
            uint fen = (uint)(((f - yuan) * 100) % 10);
            return new RMB(yuan, jiao, fen);
        }

        // more
    }
    #endregion
}