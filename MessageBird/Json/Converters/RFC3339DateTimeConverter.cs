﻿using System;

using MessageBird.Utilities;
using Newtonsoft.Json;

namespace MessageBird.Json.Converters
{
    class RFC3339DateTimeConverter : JsonConverter
    {
        // XXX: Format should be "yyyy-MM-dd'T'THH:mm:ssK".
        // However, due to bug the endpoint expects the current used format.
        // TODO: Need to be changed when the endpoint is updated!
        private const string Format = "yyyy-MM-dd'T'HH:mm";
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                writer.WriteValue(dateTime.ToString(Format));
            }
            else
            {
                throw new JsonSerializationException("Expected value of type 'DateTime'.");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type t = (ReflectionUtils.IsNullable(objectType))
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                return reader.Value;
            }
            throw new JsonSerializationException(String.Format("Unexpected token '{0}' when parsing date.", reader.TokenType));
        }

        public override bool CanConvert(Type objectType)
        {
            Type t = (ReflectionUtils.IsNullable(objectType))
               ? Nullable.GetUnderlyingType(objectType)
               : objectType;

            return t == typeof(DateTime);
        }
    }
}
