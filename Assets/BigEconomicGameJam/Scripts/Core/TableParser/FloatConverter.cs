using Newtonsoft.Json;
using System;
using System.Globalization;
using UnityEngine;

namespace CORE
{
    public class FloatConverter: JsonConverter
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

            if(reader.TokenType == JsonToken.Float)
            {
                return Convert.ToSingle(reader.Value);
            }

            else if(reader.TokenType == JsonToken.String)
            {
                string value = (string)reader.Value;

                if(string.IsNullOrEmpty(value))
                    return -1f;

                value = value.Replace(',', '.');

                if(float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                    return result;
                else
                    return -1f;
            }

            return Convert.ToSingle(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}