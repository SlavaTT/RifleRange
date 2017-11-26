using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace RifleRange.DAL
{	
    public class RecordMapper
    {
        private static Dictionary<Type, RecordMapperInstance> recordMappers = new Dictionary<Type, RecordMapperInstance>();

        public static void Register(Type classType)
        {
            Register(classType, null);
        }

        public static void Register(Type classType, Type propertyAttributeType = null)
        {
            lock (recordMappers)
            {
                recordMappers.Add(classType, new RecordMapperInstance(classType, propertyAttributeType));
            }
        }

        public static void ReadObject(object objToFill, SqlDataReader reader)
        {
            Type classType = objToFill.GetType();
            RecordMapperInstance recMapper;

            try
            {
                recMapper = recordMappers[classType];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("RecordMapper for class " + classType.Name + " not found", e);
            }

            recMapper.ReadObject(objToFill, reader);
        }
    }

    class RecordMapperInstance
    {
        private Type classType;
        private Type propertyAttributeType;
        private Hashtable propertiesForFill = new Hashtable();

        public RecordMapperInstance(Type classType, Type propertyAttributeType = null)
        {
            this.classType = classType;
            this.propertyAttributeType = propertyAttributeType;

            this.RegisterProperties();
        }

        private void RegisterProperties()
        {
            foreach (PropertyInfo propInfo in classType.GetProperties())
            {
                if (this.propertyAttributeType == null || propInfo.GetCustomAttribute(propertyAttributeType) != null)
                {
                    this.propertiesForFill.Add(propInfo.Name, propInfo);
                }
            }
        }

        public void ReadObject(object destObj, SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed && reader.HasRows)
            {
                object[] val = new object[1];
                int cnt = reader.FieldCount;
                for (int i = 0; i < cnt; i++)
                {
                    PropertyInfo propInfo = GetPropertyInfo(reader.GetName(i));
                    if (propInfo != null)
                    {
                        val[0] = reader.GetValue(i);
                        if (val[0].GetType() != typeof(DBNull)) propInfo.SetMethod.Invoke(destObj, val);
                    }
                }
            }
        }

        private PropertyInfo GetPropertyInfo(string propName)
        {
            return (this.propertiesForFill.ContainsKey(propName) ? (PropertyInfo)this.propertiesForFill[propName] : null);
        }
    }

}