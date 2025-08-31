using Newtonsoft.Json;
using System;
using System.Globalization;
using UnityEngine;

namespace CORE
{
    public class ObjectConvert : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(float) || objectType == typeof(float?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType == typeof(float?))
                    return null;

                Debug.LogError("res");
                return -1f;
            }

            else if(reader.TokenType == JsonToken.Boolean)
            {
                return Convert.ToBoolean(reader.Value);
            }

            else if(reader.TokenType == JsonToken.String)
            {
                string value = (string)reader.Value;

                if(float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float resFloat))
                    return resFloat;
                else if(bool.TryParse(value, out bool resBool))
                    return resBool;
                else
                    return value;
            }

            return Convert.ToSingle(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}