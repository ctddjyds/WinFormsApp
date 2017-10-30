using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace WinFormsAppFor157Recommend
{
    [Serializable]
    public class Tip14EmployeeClone : ICloneable
    {
        public string IDCode { get; set; }
        public int Age { get; set; }
        public DepartmentClass Department { get; set; }
        public object Clone()
        {
            //MemberwiseClone初始化一个新的对象，并遍历源对象的所有实例字段，然后值类型字段复制值副本，而引用对象只复制了引用地址
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 浅复制--将对象所有字段复制到新对象中，值类型字段复制值副本，而引用对象只复制了引用地址，而不是引用的对象
        /// </summary>
        /// <returns></returns>
        public Tip14EmployeeClone ShallowClone()
        {
            return this.Clone() as Tip14EmployeeClone;
        }
        /// <summary>
        /// 深复制--将对象所有字段复制到新对象中，值类型字段和引用类型字段都被重新创建并赋值，不会影响到源对象本身；
        /// 使用序列化的方式进行深复制可以应对当类字段修改后而不必修改复制方法；
        /// </summary>
        /// <returns></returns>
        public Tip14EmployeeClone DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                Object obj;
                IFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                obj= formatter.Deserialize(objectStream);
                objectStream.Close();
                return obj as Tip14EmployeeClone;
            }
        }
    }
    public class DepartmentClass
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return this.Name;
        }
    }
    public static class DeepCloneClass
    {
        //利用反射实现
        public static T DeepCopy<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }
        //利用xml序列化和反序列化实现
        public static T XmlDeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
        //利用二进制序列化和反序列化实现
        public static T BinaryDeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
        //
        public static T DataContractDeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = ser.ReadObject(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}
