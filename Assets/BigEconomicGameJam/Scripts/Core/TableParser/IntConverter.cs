using Newtonsoft.Json;
using System;

namespace CORE
{
    public class IntConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int) || objectType == typeof(int?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if(reader.TokenType == JsonToken.Null)
            {
                if(objectType == typeof(int?))
                    return null;

                return -1;
            }

            if(reader.TokenType == JsonToken.Float)
            {
                return Convert.ToInt32((double)reader.Value);
            }

            else if(reader.TokenType == JsonToken.String)
            {
                string value = (string)reader.Value;

                if(string.IsNullOrEmpty(value))
                    return -1;

                if(int.TryParse(value, out int result))
                    return result;
                else
                    return -1;
            }

            return Convert.ToInt32(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}