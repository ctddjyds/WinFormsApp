using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codeplex.Data;

namespace WinFormsAppFor157Recommend
{
    public class DynamicJsonHelper
    {
        public void TestRead()
        {
            // 将Json字符串解析成DynamicJson对象
            var json = DynamicJson.Parse(@"{""foo"":""json"", ""bar"":100, ""nest"":{ ""foobar"":true } }");
            var r1 = json.foo; // "json" - string类型
            var r2 = json.bar; // 100 - double类型
            var r3 = json.nest.foobar; // true - bool类型
            var r4 = json["nest"]["foobar"]; // 还可以和javascript一样通过索引器获取
        }
        public void TestCURD()
        {
            // 将Json字符串解析成DynamicJson对象
            var json = DynamicJson.Parse(@"{""foo"":""json"", ""bar"":100, ""nest"":{ ""foobar"":true } }");

            // 判断json字符串中是否包含指定键
            var b1_1 = json.IsDefined("foo"); // true
            var b2_1 = json.IsDefined("foooo"); // false
            // 上面的判断还可以更简单，直接通过json.键()就可以判断
            var b1_2 = json.foo(); // true
            var b2_2 = json.foooo(); // false;


            // 新增操作
            json.Arr = new string[] { "NOR", "XOR" }; // 新增一个js数组
            json.Obj1 = new { }; // 新增一个js对象
            json.Obj2 = new { foo = "abc", bar = 100 }; // 初始化一个匿名对象并添加到json字符串中

            // 删除操作
            json.Delete("foo");
            json.Arr.Delete(0);
            // 还可以更简单去删除，直接通过json(键); 即可删除。
            json("bar");
            json.Arr(1);

            // 替换操作
            json.Obj1 = 5000;

            // 创建一个新的JsonObject
            dynamic newjson = new DynamicJson();
            newjson.str = "aaa";
            newjson.obj = new { foo = "bar" };

            // 直接序列化输出json字符串
            var jsonstring = newjson.ToString(); // {"str":"aaa","obj":{"foo":"bar"}}
        }
        public void TestForEach()
        {
            // 直接遍历json数组
            var arrayJson = DynamicJson.Parse(@"[1,10,200,300]");
            foreach (int item in arrayJson)
            {
                Console.WriteLine(item); // 1, 10, 200, 300
            }

            // 直接遍历json对象
            var objectJson = DynamicJson.Parse(@"{""foo"":""json"",""bar"":100}");
            foreach (KeyValuePair<string, dynamic> item in objectJson)
            {
                Console.WriteLine(item.Key + ":" + item.Value); // foo:json, bar:100
            }
        }
        public void TestDeserialize()
        {
            var arrayJson = DynamicJson.Parse(@"[1,10,200,300]");
            var objectJson = DynamicJson.Parse(@"{""foo"":""json"",""bar"":100}");

            // 将json数组转成C#数组
            // 方法一：
            var array1 = arrayJson.Deserialize<int[]>();
            // 方法二
            var array2 = (int[])arrayJson;
            // 方法三，这种最简单，直接声明接收即可，推荐使用
            int[] array3 = arrayJson;

            // 将json字符串映射成C#对象
            // 方法一：
            var foobar1 = objectJson.Deserialize<FooBar>();
            // 方法二：

            var foobar2 = (FooBar)objectJson;
            // 方法三，这种最简单，直接声明接收即可，推荐使用
            FooBar foobar3 = objectJson;

            // 还可以通过Linq进行操作
            var objectJsonList = DynamicJson.Parse(@"[{""bar"":50},{""bar"":100}]");
            var barSum = ((FooBar[])objectJsonList).Select(fb => fb.bar).Sum(); // 150
            var dynamicWithLinq = ((dynamic[])objectJsonList).Select(d => d.bar);
        }
        public void TestSerialize()
        {
            // 声明一个匿名对象
            var obj = new
            {
                Name = "Foo",
                Age = 30,
                Address = new
                {
                    Country = "Japan",
                    City = "Tokyo"
                },
                Like = new[] { "Microsoft", "Xbox" }
            };
            // 序列化
            // {"Name":"Foo","Age":30,"Address":{"Country":"Japan","City":"Tokyo"},"Like":["Microsoft","Xbox"]}
            var jsonStringFromObj = DynamicJson.Serialize(obj);

            // 还支持直接序列化数组，集合
            // [{"foo":"fooooo!","bar":1000},{"foo":"orz","bar":10}]
            var foobar = new FooBar[] {
               new FooBar { foo = "fooooo!", bar = 1000 },
               new FooBar { foo = "orz", bar = 10 }
           };
            // 序列化
            var jsonFoobar = DynamicJson.Serialize(foobar);
        }
        public void TestConfict()
        {
            var nestJson = DynamicJson.Parse(@"{""tes"":10,""nest"":{""a"":0}");

            nestJson.nest(); // 判断是否存在nest属性
            nestJson.nest("a"); // 删除nest属性中的a属性

            // 处理json中的键和C#的类型冲突导致编译失败，或语法提示错误，只需要在前面加@前缀即可
            var json = DynamicJson.Parse(@"{""int"":10,""event"":null}");
            var r1 = json.@int; // 10.0
            var r2 = json.@event; // null
        }
    }
    public class FooBar
    {
        public string foo { get; set; }
        public int bar { get; set; }
    }
}
