using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// .NET提供两种序列化技术：一种是二进制序列化，深序列化方式，用于将对象的私有字段、公有字段等转换为字节流，进而写入数据流，System.Runtime.Serialization命名空间提供；
    /// 一种是Xml序列化，浅序列化方式，仅序列化对象的公有属性和字段，在通过web传输数据的时候，这种方式符合XML的标准化的开放规则，System.Xml.Serialization命名空间提供；
    /// 特性可以声明式地为代码中的目标元素添加注释。
    /// 运行时可以通过查询这些托管模块中的元数据信息，达到改变目标元素运行时行为的目的
    /// 使用场合：将对象序列化到本地；把对象传输到网络上的另一个终端上；对象的粘贴复制等等
    /// </summary>
    [Serializable]
    public class SerializableClass
    {
        [NonSerialized]
        private string m_NoSeriablizeField;
        /// <summary>
        /// 不能将NonSerialized特性应用于属性上
        /// </summary>
        public string NoSeriablizeField
        {
            get { return m_NoSeriablizeField; }
            set { this.m_NoSeriablizeField = value; }
        }
        //事件一般都不要序列化，不能使用NonSerialized，需使用改进的field: NonSerialized
        [field: NonSerialized] 
        public event EventHandler NoSeriablizeEvent;
        public string FirstName;
        public string LastName;
        [NonSerialized]
        public string ChineseName;
        /// <summary>
        /// OnDeserializedAttribute特性应用于方法时，会指定在对象反序列化后立即调用此方法
        /// OnDeserializingAttribute特性应用于方法时，会指定在对象反序列化时调用此方法
        /// OnSerializedAttribute
        /// OnSerializingAttribute特性应用于方法时，会指定在对象序列化前调用此方法
        /// </summary>
        [OnDeserializedAttribute]
        public void OnSerialized()
        {
            ChineseName = string.Format("{0}{1}",LastName,FirstName);
        }

    }
    public class BinarySerializer
    {
        //将类型序列化为字符串
        public static string Serialize<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                return System.Text.Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        //将类型序列化为文件
        public static void SerializeToFile<T>(T t, string path, string fullName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fullPath = string.Format(@"{0}\{1}", path, fullName);
            using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                stream.Flush();
            }
        }

        //将字符串反序列化为类型
        public static TResult Deserialize<TResult>(string s) where TResult : class
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(s);
            using (MemoryStream stream = new MemoryStream(bs))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as TResult;
            }
        }

        //将文件反序列化为类型
        public static TResult DeserializeFromFile<TResult>(string path) where TResult : class
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as TResult;
            }
        }
    }

    [Serializable]
    public class PersonSerializable : ISerializable
    {
        public string FirstName;
        public string LastName;
        public string ChineseName;

        public PersonSerializable()
        {
        }

        protected PersonSerializable(SerializationInfo info, StreamingContext context)
        {
            FirstName = info.GetString("FirstName");
            LastName = info.GetString("LastName");
            ChineseName = string.Format("{0} {1}", LastName, FirstName);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FirstName", FirstName);
            info.AddValue("LastName", LastName);
        }
    }
    /// <summary>
    /// 忽略掉所有的类型序列化特性
    /// </summary>
    [Serializable]
    public class SerializableClassUseInterface : ISerializable
    {
        public string FirstName;
        public string LastName;
        public string ChineseName;
        protected SerializableClassUseInterface(SerializationInfo info,StreamingContext sContext)
        {
            //FirstName=info.GetString("FirstName");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //SetType负责告诉序列化器：这个对象会被反序列化成SerializableClassUseAnother对象
            //在版本升级中，能够处理类型因为字段变换而带来的问题
            info.SetType(typeof(SerializableClassUseAnother));
            info.AddValue("FirstName",FirstName);
        }
    }
    [Serializable]
    public class SerializableClassUseAnother : ISerializable
    {
        public string ChineseName;
        protected SerializableClassUseAnother(SerializationInfo info, StreamingContext sContext)
        {
            ChineseName = info.GetString("ChineseName");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
    [Serializable]
    public class EmployeeSerializable : PersonSerializable, ISerializable
    {
        public int Salary { get; set; }

        public EmployeeSerializable()
        {
        }

        protected EmployeeSerializable(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Salary = info.GetInt32("Salary");
        }
        /// <summary>
        /// 实现了ISerializable的子类也需要负责父类的序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Salary", Salary);
        }
    }
}
